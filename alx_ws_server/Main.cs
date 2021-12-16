using System;
using System.Collections.Generic;
using CitizenFX.Core;
using WebSocketSharp.Server;
using static CitizenFX.Core.Native.API;

namespace alx_ws_server
{
    public class Main : BaseScript
    {
        public Main()
        {
            EventHandlers["onServerResourceStart"] += new Action<String>(OnResourceStart);
        }
        
        private void OnResourceStart(String name)
        {
            if (name != GetCurrentResourceName()) return;
            WebSocketServer server = new WebSocketServer();
            server.AddWebSocketService<BaseBehavior>("/echo");
            RegisterCommand("ws-reload", new Action<int, List<Object>, String>((source, args, raw) =>
            {
                server.Stop();
                server.Start();
                TriggerClientEvent("chat:addMessage", new {color = new[] {0, 255, 255}, multiline = true, args = new[] {"alx_ws", "WebSocket Server Reloaded !"}}, source);
            }), true);

            EventHandlers["alx_ws:registerEndpoint"] += new Action<String, Action<String, String>, Action<String>, Action<String, String>>(
            (endpoint, onMessage, onOpen, onClose) =>
            {
                server.AddWebSocketService(endpoint, () => new CustomBehavior(this, onMessage, onOpen, onClose));
            });

            EventHandlers["alx_ws:broadcast"] += new Action<String, String>((endpoint, message) =>
            {
                server.WebSocketServices[endpoint].Sessions.Broadcast(message);
            });
            
            EventHandlers["alx_ws:send"] += new Action<String, String, String>((endpoint, id, message) =>
            {
                server.WebSocketServices[id].Sessions.SendTo(message, id);
            });
        }

        public void SendEvent(String name, params Object[] args)
        {
            TriggerEvent(name, args);
        }
    }
}