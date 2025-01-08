using System;
using System.Net.Sockets;
using System.Text;

class SimpleTcpClient
{
    static void Main()
    {
        using (TcpClient client = new TcpClient("127.0.0.1", 5000))
        using (NetworkStream stream = client.GetStream())
        {
            Console.WriteLine("Connected to server.");
            byte[] buffer = new byte[1024];

            while (true)
            {
                // 1. Відправити повідомлення серверу
                Console.Write("Client message: ");
                string message = Console.ReadLine() ?? "";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                if (message.Trim().ToLower() == "exit")
                {
                    Console.WriteLine("Client stops.");
                    break;
                }

                // 2. Отримати відповідь від сервера
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead <= 0)
                {
                    Console.WriteLine("Server closed connection.");
                    break;
                }
                string serverMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Server: {serverMessage}");
                if (serverMessage.Trim().ToLower() == "exit")
                {
                    Console.WriteLine("Server wants to exit. Closing...");
                    break;
                }
            }
        }
    }
}
