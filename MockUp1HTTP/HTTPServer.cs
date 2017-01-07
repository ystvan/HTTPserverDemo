using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MockUp1HTTP
{
    public class HttpServer
    {
        private IPAddress _address;
        private int _port;
        private bool _isAlive = true;

        private TcpListener _listener;

        public HttpServer(IPAddress address, int port)
        {
            _address = address;
            _port = port;
        }

        public void Listen()
        {
            _listener = new TcpListener(_address, _port);
            _listener.Start();

            try
            {
                while (_isAlive)
                {
                    TcpClient client = _listener.AcceptTcpClient();

                    Console.WriteLine("Server activated by the client!");

                    HttpService service = new HttpService(client);
                    Thread thread = new Thread(new ThreadStart(service.Run));
                    thread.Start();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Shutdown server" + exception.Message);
            }
            finally
            {
                _listener.Stop();
                Console.WriteLine("Server: Web-Server is stopped!");
            }

        }
    }
}
