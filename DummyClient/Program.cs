using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using ServerCore;

namespace DummyClient
{
    class GameSession : Session
    {
        public override void OnConneceted(EndPoint endpoint)
        {
            Console.WriteLine("OnConnected");

            for (int i = 0; i < 5; i++)
            {
                byte[] sendBuff = Encoding.UTF32.GetBytes("Welecome to KINLAB");
                Send(sendBuff);
            }
        }

        public override void OnDiscoonected(EndPoint endPoint)
        {
            Console.WriteLine("OnDisconnected");
        }

        public override void OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF32.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine(recvData);
        }

        public override void OnSend(int numOfBytes)
        {
        }
    }

    public class DummyClient
    {
        static void Main(string[] args)
        {

            string host = Dns.GetHostName();
            int port = 4545;
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endpoint = new IPEndPoint(ipAddr, 4545);

            Connector connector = new Connector();
            connector.Connect(endpoint, () => { return new GameSession(); });

            Func<Session> sessionFactory;

            while (true)
            {
                //소켓은 새로운 접속을 진행할 때 마다 한번 씩 집어넣어 해결해야한다.
                Socket server = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {

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