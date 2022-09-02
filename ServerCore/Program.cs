using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using ServerCore;

class GameSession : Session
{
    public override void OnConneceted(EndPoint endpoint)
    {
        byte[] sendBuff = Encoding.UTF32.GetBytes("Welcome to yokoso japparipark a~");
        Send(sendBuff);
        Thread.Sleep(1000);
        Disconnect();
        Console.WriteLine("OnConnected");
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


public class Server
{
    static Listener listener = new Listener();

    static void Main(string[] args)
    {
        string host = Dns.GetHostName();
        int port = 4545;
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint ipEndpoint = new IPEndPoint(ipAddr, 4545);
        Socket socket = new Socket(ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        // 리스너 선언 및 동작

        listener.Init(ipEndpoint, 10, () => { return new GameSession(); });
        while (true)
        {
            ;
        }
    }
}