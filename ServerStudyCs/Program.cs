using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace ServerStudyCs
{
    public class Server
    {
        static Listener listener = new Listener();
        static void OnAcceptHandler(Socket client)
        {
            try
            {
                Session session = new Session();
                session.Start(client);


                // 보낸다

                byte[] sendBuff = Encoding.UTF32.GetBytes("Welcome to yokoso japparipark a~");
                session.Send(sendBuff);
                Thread.Sleep(1000);
                session.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            int port = 4545;
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndpoint = new IPEndPoint(ipAddr, 4545);
            Socket socket = new Socket(ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // 리스너 선언 및 동작

            listener.Init(ipEndpoint, 10, OnAcceptHandler);
            while (true)
            {

                ;
            }
        }
    }
}
