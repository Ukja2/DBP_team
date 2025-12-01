using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;





namespace DBP_team
{
    /// <summary>
    /// Simple in-process TCP chat server.
    /// Protocol (per line):
    /// AUTH|userId
    /// MSG|fromUserId|toUserId|base64(message)
    /// READ|readerUserId|senderUserId
    /// QUIT|userId
    /// Server does not echo to sender; only routes to receiver.
    /// </summary>
    public static class ChatServer
    {
        private static readonly object _lock = new object();
        private static TcpListener _listener;
        private static bool _running;
        private static readonly Dictionary<int, List<ClientConn>> _clients = new Dictionary<int, List<ClientConn>>();
        private static Thread _acceptThread;
        private static int _port = 9000; // default

        private class ClientConn
        {
            public TcpClient Tcp; public StreamReader Reader; public StreamWriter Writer; public int UserId; public CancellationTokenSource Cts;
        }

        public static void Start(int port = 9000)
        {
            lock (_lock)
            {
                if (_running) return;
                _port = port;
                _listener = new TcpListener(IPAddress.Any, port);
                try
                {
                    _listener.Start();
                    _running = true;
                }
                catch (Exception ex)
                {
                    // Port already in use or start failure
                    Console.WriteLine("ChatServer start failed: " + ex.Message);
                    return;
                }
                _acceptThread = new Thread(AcceptLoop) { IsBackground = true };
                _acceptThread.Start();
                Console.WriteLine("ChatServer listening on " + port);
            }
        }

        public static void Stop()
        {
            lock (_lock)
            {
                _running = false;
                try { _listener?.Stop(); } catch { }
                foreach (var kv in _clients)
                {
                    foreach (var c in kv.Value)
                    {
                        try { c.Cts.Cancel(); } catch { }
                        try { c.Tcp.Close(); } catch { }
                    }
                }
                _clients.Clear();
            }
        }

        private static void AcceptLoop()
        {
            while (_running)
            {
                try
                {
                    var tcp = _listener.AcceptTcpClient();
                    var task = Task.Run(() => HandleClient(tcp));
                }
                catch (SocketException)
                {
                    if (!_running) break;
                }
                catch
                {
                    // ignore
                }
            }
        }

        private static void HandleClient(TcpClient tcp)
        {
            ClientConn conn = null;
            try
            {
                var ns = tcp.GetStream();
                var reader = new StreamReader(ns, Encoding.UTF8);
                var writer = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };
                var cts = new CancellationTokenSource();
                conn = new ClientConn { Tcp = tcp, Reader = reader, Writer = writer, UserId = 0, Cts = cts };
                while (_running && !cts.IsCancellationRequested)
                {
                    var line = reader.ReadLine();
                    if (line == null) break;
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split('|');
                    if (parts.Length == 0) continue;
                    var cmd = parts[0];
                    if (cmd == "AUTH" && parts.Length >= 2)
                    {
                        if (int.TryParse(parts[1], out int uid))
                        {
                            conn.UserId = uid;
                            RegisterClient(conn);
                            writer.WriteLine("AUTH_OK");
                        }
                        else writer.WriteLine("AUTH_FAIL");
                    }
                    else if (cmd == "MSG" && parts.Length >= 4)
                    {
                        if (conn.UserId == 0) { writer.WriteLine("ERR_NOT_AUTH"); continue; }
                        if (!int.TryParse(parts[1], out int from)) continue;
                        if (!int.TryParse(parts[2], out int to)) continue;
                        var msgText = DecodeBase64(parts[3]);
                        // persist to DB
                        TryInsertMessage(from, to, msgText);
                        // route
                        RouteMessage(from, to, msgText);
                        writer.WriteLine("SENT");
                    }
                    else if (cmd == "READ" && parts.Length >= 3)
                    {
                        if (!int.TryParse(parts[1], out int readerId)) continue;
                        if (!int.TryParse(parts[2], out int senderId)) continue;
                        TryMarkRead(readerId, senderId);
                        NotifyRead(readerId, senderId);
                        writer.WriteLine("READ_OK");
                    }
                    else if (cmd == "QUIT")
                    {
                        break;
                    }
                }
            }
            catch
            {
                // ignore
            }
            finally
            {
                if (conn != null)
                {
                    UnregisterClient(conn);
                    try { conn.Tcp.Close(); } catch { }
                }
            }
        }

        private static void RegisterClient(ClientConn conn)
        {
            lock (_lock)
            {
                if (!_clients.TryGetValue(conn.UserId, out var list))
                {
                    list = new List<ClientConn>();
                    _clients[conn.UserId] = list;
                }
                list.Add(conn);
            }
        }

        private static void UnregisterClient(ClientConn conn)
        {
            lock (_lock)
            {
                if (conn.UserId != 0 && _clients.TryGetValue(conn.UserId, out var list))
                {
                    list.Remove(conn);
                    if (list.Count == 0) _clients.Remove(conn.UserId);
                }
            }
        }

        private static void RouteMessage(int from, int to, string message)
        {
            var line = "MSG|" + from + "|" + to + "|" + EncodeBase64(message);
            List<ClientConn> targets = null;
            lock (_lock)
            {
                if (_clients.TryGetValue(to, out var list)) targets = new List<ClientConn>(list);
            }
            if (targets == null) return;
            foreach (var c in targets)
            {
                try { c.Writer.WriteLine(line); } catch { }
            }
        }

        private static void NotifyRead(int readerId, int senderId)
        {
            var line = "READ|" + readerId + "|" + senderId;
            List<ClientConn> targets = null;
            lock (_lock)
            {
                if (_clients.TryGetValue(senderId, out var list)) targets = new List<ClientConn>(list);
            }
            if (targets == null) return;
            foreach (var c in targets)
            {
                try { c.Writer.WriteLine(line); } catch { }
            }
        }

        private static void TryInsertMessage(int from, int to, string message)
        {
            try
            {
                DBManager.Instance.ExecuteNonQuery(
                    "INSERT INTO chat (sender_id, receiver_id, message, created_at, is_read) VALUES (@s,@r,@m,NOW(),0)",
                    new MySqlParameter("@s", from),
                    new MySqlParameter("@r", to),
                    new MySqlParameter("@m", message));
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB insert failed: " + ex.Message);
            }
        }

        private static void TryMarkRead(int readerId, int senderId)
        {
            try
            {
                DBManager.Instance.ExecuteNonQuery(
                    "UPDATE chat SET is_read = 1 WHERE receiver_id = @reader AND sender_id = @sender AND is_read = 0",
                    new MySqlParameter("@reader", readerId),
                    new MySqlParameter("@sender", senderId));
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB read update failed: " + ex.Message);
            }
        }

        private static string EncodeBase64(string s) => Convert.ToBase64String(Encoding.UTF8.GetBytes(s ?? ""));
        private static string DecodeBase64(string b)
        {
            try { return Encoding.UTF8.GetString(Convert.FromBase64String(b)); } catch { return ""; }
        }
    }
}
