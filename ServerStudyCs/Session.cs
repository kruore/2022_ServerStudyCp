using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ServerStudyCs
{
    class Session
    {
        Socket _socket;
        int _disconnected = 0;
        public void Start(Socket socket)
        {
            _socket = socket;
            SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
            recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            recvArgs.UserToken = this;
            // 버퍼 세팅 버퍼는 이거구, offset부터 1024 사이즈까지
            recvArgs.SetBuffer(new byte[1024], 0, 1024);

            RegisterRecv(recvArgs);
        }

        public void Disconnect()
        {

            // 이 경우 멀티 스레드 환경에서 보호받지 못함.
            //if(socket!=null)

            // 같은 아이가 2번 디스커넥트 하는 환경을 조성하지 않도록
            // 디스커넥트의 체크를 lock을 통해 진행하는 것이 좋다.

            if (Interlocked.Exchange(ref _disconnected, 1)==1)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
        }

        public void Send(byte[] sendBuff)
        {
            _socket.Send(sendBuff);
        }
        #region Recv Data Region
        public void RegisterRecv(SocketAsyncEventArgs args)
        {
            bool pending = _socket.ReceiveAsync(args);

            if (pending == false)
            {
                OnRecvCompleted(null, args);
            }
        }

        public void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {

            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                try
                {
                    string recvData = Encoding.UTF32.GetString(args.Buffer, args.Offset, args.BytesTransferred);
                    RegisterRecv(args);
                    Console.WriteLine(recvData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
            }

        }
        #endregion
    }
}
