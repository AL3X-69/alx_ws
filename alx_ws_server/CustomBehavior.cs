using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace alx_ws_server
{
    public class CustomBehavior : WebSocketBehavior
    {
        private readonly Main _baseScript;
        private readonly Action<String, String> _messageCallback;
        private readonly Action<String> _openedCallback;
        private readonly Action<String, String> _closedCallback;

        public CustomBehavior(Main baseScript, Action<string, string> messageCallback, Action<string> openedCallback, Action<string, string> closedCallback)
        {
            _baseScript = baseScript;
            _messageCallback = messageCallback;
            _openedCallback = openedCallback;
            _closedCallback = closedCallback;
        }

        protected override void OnClose(CloseEventArgs e)
        {
            _closedCallback(ID, e.Reason);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            _messageCallback(ID, e.Data);
            _baseScript.SendEvent("alx_ws:messageReceived",ID, e.Data);
        }

        protected override void OnOpen()
        {
            _openedCallback(ID);
        }
    }
}