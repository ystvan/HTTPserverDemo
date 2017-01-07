using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MockUp1HTTP
{
    public class HttpService
    {
        private TcpClient _conection;
        private string _path = @"C:/Users/IstvanMarki/Downloads/";
        private Stream _stream;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;
        private FileStream _fileStream;

        public HttpService(TcpClient conection)
        {
            this._conection = conection;
        }

        public void Run()
        {
            try
            {
                _stream = _conection.GetStream();
                _streamReader = new StreamReader(_stream);

                _streamWriter = new StreamWriter(new BufferedStream(_stream)) {AutoFlush = true};

                string request = _streamReader.ReadLine();
                if (string.IsNullOrEmpty(request))
                    throw new NullReferenceException();

                Console.WriteLine("Client: " + request);

                string[] content = request.Split(' ');

                if (content.Length != 3)
                {
                    sendError(_streamWriter, 400, "Illegal request");
                }
                else
                {
                    string method = content[0].ToUpper();
                    string uri = content[1];
                    string protocolVersion = content[2];

                    uri = _path + uri;

                    Console.WriteLine("Method: " + method);
                    Console.WriteLine("Uri: " + uri);
                    Console.WriteLine("Protocol: " + protocolVersion);

                    try
                    {

                    }
                    catch (FileNotFoundException e)
                    {
                        sendError(_streamWriter, 404, "Not found: " + uri);
                    }
                    finally
                    {
                        if (_fileStream != null) _fileStream.Dispose();
                        if (_conection != null) _conection.Close();
                    }
                }

            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.ToString());
                if (_fileStream != null) _fileStream.Dispose();
                if (_conection != null) _conection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (_fileStream != null) _fileStream.Dispose();
                if (_conection != null) _conection.Close();
            }
        }

        private void sendError(StreamWriter streamWriter, int errorCode, string errorMsg)
        {
            streamWriter.WriteLine($"HTTP/1.1 + {errorCode} + {errorMsg}");
            streamWriter.WriteLine($"Conncetion : close\n");
            streamWriter.WriteLine($"<html><head><title> + {errorMsg} + </title></head><body> + {errorCode} + {errorMsg} + </body></html>");

        }

        private void writeSuccess(string message)
        {
            _streamWriter.Write("HTTP/1.1 200 OK\r\n");
            _streamWriter.Write("Content-Type: " + message + "\r\n");
            _streamWriter.Write("\r\n");
        }

    }
}
