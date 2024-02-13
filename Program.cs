using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CafeteriaApp
{
    class CafeteriaClient
    {
        static async Task Main(string[] args)
        {
            using (ClientWebSocket clientWebSocket = new ClientWebSocket())
            {
                await clientWebSocket.ConnectAsync(new Uri("ws://localhost:9090"), CancellationToken.None);

                Console.WriteLine("Conectado al servidor de Cafetería.");

                // Bucle para recibir mensajes del servidor
                _ = Task.Run(async () =>
                {
                    while (true)
                    {
                        ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[1024]);
                        WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);
                        string receivedMessage = Encoding.UTF8.GetString(receiveBuffer.Array, 0, result.Count);
                        Console.WriteLine($"Mensaje del servidor: {receivedMessage}");
                    }
                });

                // Bucle para que los estudiantes entren en la cafetería
                while (true)
                {
                    Console.WriteLine("Presione 'Enter' para entrar a la cafetería...");
                    Console.ReadLine();

                    byte[] enterMessage = Encoding.UTF8.GetBytes("Quiero entrar a la cafetería");
                    await clientWebSocket.SendAsync(new ArraySegment<byte>(enterMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
