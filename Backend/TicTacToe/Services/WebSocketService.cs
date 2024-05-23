using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using Models;
using GameLogic;

namespace Services
{
    public class WebSocketService
    {
        private readonly List<WebSocket> _sockets = new();

        public async Task HandleWebSocketConnection(WebSocket socket)
        {
            _sockets.Add(socket);
            try
            {
                var buffer = new byte[1024 * 2];
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), default);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, default);
                        break;
                    }

                    // Handle received message if needed

                    // Optionally, broadcast message to other clients
                    foreach (var s in _sockets.Where(s => s != socket && s.State == WebSocketState.Open))
                    {
                        await s.SendAsync(buffer[..result.Count], WebSocketMessageType.Text, true, default);
                    }
                }
            }
            finally
            {
                _sockets.Remove(socket);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "WebSocket connection closed", default);
            }
        }
    }
}