using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MockUp1HTTP
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer server = new HttpServer(IPAddress.Parse("127.0.0.1"), 8080);
            Thread thread = new Thread(new ThreadStart(server.Listen));
            thread.Start();
        }
    }
}
