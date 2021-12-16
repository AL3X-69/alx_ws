using System;
using CitizenFX.Core;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace alx_ws_server
{
    public class BaseBehavior : WebSocketBehavior
    {
        protected override void OnClose(CloseEventArgs e)
        {
            Print("Websocket Connection Closed ("+e.Reason+")");
        }

        protected override void OnError(ErrorEventArgs e)
        {
            PrintErr(e.Message);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Send(e.Data);
        }

        protected override void OnOpen()
        {
            Print("Websocket Connection Opened");
        }

        private void Print(String msg)
        {
            Debug.WriteLine("[alx_ws] [INFO] "+msg);
        }

        private void PrintErr(String msg)
        {
            Debug.WriteLine("[alx_ws] [ERROR] "+msg);
        }
    }
}