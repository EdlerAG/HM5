using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class SimpleTcpServer
{
    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        Console.WriteLine("Server started on port 5000...");

        using (TcpClient client = listener.AcceptTcpClient())
        using (NetworkStream stream = client.GetStream())
        {
            Console.WriteLine("Client connected.");
            byte[] buffer = new byte[1024];

            while (true)
            {
                // 1. Приймаємо повідомлення від клієнта
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead <= 0)
                    break; // Клієнт закрив з’єднання

                string clientMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Client: {clientMessage}");
                if (clientMessage.Trim().ToLower() == "exit")
                {
                    Console.WriteLine("Client wants to exit. Closing connection.");
                    break;
                }

                // 2. Відправити відповідь
                Console.Write("Server message: ");
                string serverMessage = Console.ReadLine() ?? "";
                byte[] data = Encoding.UTF8.GetBytes(serverMessage);
                stream.Write(data, 0, data.Length);
                if (serverMessage.Trim().ToLower() == "exit")
                {
                    Console.WriteLine("Server stops.");
                    break;
                }
            }
        }

        listener.Stop();
        Console.WriteLine("Server closed.");
    }
}
