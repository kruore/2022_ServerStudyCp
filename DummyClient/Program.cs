using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;


namespace DummyClient
{
    public class DummyClient
    {
        static void Main(string[] args)
        {

            string host = Dns.GetHostName();
            int port = 4545;
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endpoint = new IPEndPoint(ipAddr, 4545);



            while (true)
            {
                try
                {
                    //소켓은 새로운 접속을 진행할 때 마다 한번 씩 집어넣어 해결해야한다.
                    Socket server = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                  
                    // EndPoint 로 소켓의 접속을 진행  
                    server.Connect(endpoint);
                    Console.WriteLine(server.RemoteEndPoint.ToString());

                    // 보낸다.
                    byte[] sendBuff = Encoding.UTF32.GetBytes("Welcome to yokoso japparipark a~");
                    server.Send(sendBuff);

                    // 받는다.
                    byte[] recvBuff = new byte[1024];
                    int recvBytes = server.Receive(recvBuff);
                    string data = Encoding.UTF32.GetString(recvBuff, 0, recvBytes);
                    Console.WriteLine(data);


                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Thread.Sleep(10);
            }
        }
    }
}