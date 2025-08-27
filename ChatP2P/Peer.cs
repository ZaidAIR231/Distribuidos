using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatP2P
{
    public class Peer
    {
        private readonly TcpListener _tcplistener;
        private TcpClient? _tcpClient;
        private StreamReader? _reader;
        private StreamWriter? _writer;

        private const int Port = 8080;

        public Peer() => _tcplistener = new TcpListener(IPAddress.Any, Port);

        public async Task ConnectToPeer(string ipAddress, string port)
        {
            try
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(IPAddress.Parse(ipAddress), Convert.ToInt32(port));
                Console.WriteLine($"Connected to peer at {ipAddress}:{port}");

                InitStreams();

                // recibir en segundo plano
                var receiveTask = ReceiveMessage();

                // bucle interactivo de envío (sin mensaje hardcodeado)
                await ReadConsoleAndSendLoop();

                // si yo salgo, cierro y espero a que termine el receptor
                Close();
                await receiveTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to peer: {ex.Message}");
                Close();
            }
        }

        public async Task StartListening()
        {
            try
            {
                _tcplistener.Start();
                Console.WriteLine($"Listening for incoming connections on 0.0.0.0:{Port}...");

                _tcpClient = await _tcplistener.AcceptTcpClientAsync();
                Console.WriteLine("Connection established with a client.");

                InitStreams();

                var receiveTask = ReceiveMessage();
                await ReadConsoleAndSendLoop();

                Close();
                await receiveTask;
            }
            catch (OperationCanceledException) { /* cierre normal */ }
            catch (Exception ex)
            {
                Console.WriteLine("Connection closed :( " + ex.Message);
                Close();
            }
        }

        public async Task ReceiveMessage()
        {
            try
            {
                if (_reader == null) return;

                while (true)
                {
                    var message = await _reader.ReadLineAsync();
                    if (message is null)
                    {
                        Console.WriteLine("Peer disconnected.");
                        break;
                    }
                    Console.WriteLine($"Peer: {message}");
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Connection closed by peer.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving message: {ex.Message}");
            }
            // NO cerramos aquí; el que inició el chat coordina el Close()
        }

        public async Task SendMessage(string message)
        {
            try
            {
                if (_writer == null) return;
                await _writer.WriteLineAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
            // NO cerramos aquí para mantener la sesión abierta
        }

        private void InitStreams()
        {
            var stream = _tcpClient!.GetStream();
            _reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
        }

        // Lee la consola y manda mensajes hasta /quit
        private async Task ReadConsoleAndSendLoop()
        {
            Console.WriteLine("Chat ready. Type messages and press Enter. Type /quit to exit.");
            while (true)
            {
                var line = Console.ReadLine();
                if (line is null) continue;
                if (line.Trim().Equals("/q", StringComparison.OrdinalIgnoreCase)) break;

                await SendMessage(line);
            }
        }

        private void Close()
        {
            try { _writer?.Dispose(); } catch { }
            try { _reader?.Dispose(); } catch { }
            try { _tcpClient?.Close(); } catch { }
            try { _tcplistener.Stop(); } catch { }
        }
    }
}


