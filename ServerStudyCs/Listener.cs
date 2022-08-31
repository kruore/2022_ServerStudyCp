﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerStudyCs
{
    class Listener
    {

        Socket _listenSocket;
        Action<Socket> _OnAcceptHandler;

        public void Init(IPEndPoint endPoint, int backlog, Action<Socket> OnAcceptHandler)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _OnAcceptHandler = OnAcceptHandler;

            _listenSocket.Bind(endPoint);
            _listenSocket.Listen(backlog);

            // 선언 후 재사용을 무한히 할 수 있다는 장점이 있다.
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            RegisterAccept(args);
        }
        /// <summary>
        /// 접속 전 데이터를 pending을 반환하여, 해당 데이터가 들어왔을 경우 접속 승인을 진행한다.
        /// </summary>
        /// <param name="args"></param>
        void RegisterAccept(SocketAsyncEventArgs args)
        {
            // 이벤트 재사용 시 , 초기화 시켜주는 것이 중요함
            args.AcceptSocket = null;

            var pending = _listenSocket.AcceptAsync(args);

            if (pending == false)
                OnAcceptCompleted(null, args);
        }


        /// <summary>
        /// 접속 가능 상태가 되었다면 pending==false로 전환되며 _OnAcceptHandler의 Invoke를 통해 args.AcceptSocket을 진행한다.
        /// </summary>
        /// <param name="sender">이벤트의 호출 델리게이트를 맞추기 위해 진행</param>
        /// <param name="args">SocketAsyncEventArgs를 호출하여 소켓의 이벤트 Args를 읽는다.</param>
        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {

            if (args.SocketError == SocketError.Success)
            {
                _OnAcceptHandler.Invoke(args.AcceptSocket);
            }
            else
            {
                Console.WriteLine(args.SocketError.ToString());
            }

            // 이벤트 재사용을 통한 효율성 증대 작업 진행
            RegisterAccept(args);
        }
        public Socket Accept()
        {
            return _listenSocket.Accept();
        }
    }
}
