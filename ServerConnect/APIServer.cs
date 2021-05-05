using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MarketUITest.ServerConnect
{
    /// <summary>
    /// Класс для работы с сервером приложения
    /// </summary>
    public class APIServer
    {
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client = new TcpClient();
        static NetworkStream stream;
        int deley = 2000;

        /// <summary>
        /// Работа с сервером приложения
        /// </summary>
        /// <param name="del">Время ожидания ответов от сервера</param>
        public APIServer(int del) 
        {
            deley = del;
        }

        /// <summary>
        /// Работа с сервером приложения
        /// </summary>
        /// <remarks>Время ожидания ответа 2 секунды</remarks>
        public APIServer() { }

        /// <summary>
        /// Удаление продукта
        /// </summary>
        /// <param name="name">Имя продукта</param>
        /// <param name="price">Цена продукта</param>
        public void delete(string name, string price)
        {
            client.Connect(host, port);
            stream = client.GetStream();

            string message = "delete " + name + " " + price;
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Возвращает нужную часть сообщения от сервера
        /// </summary>
        /// <returns>True - все ОК, иначе False</returns>
        public string ReceiveMessage()
        {
            string[] answer = GetMessage().Trim().Split(':');
            if(answer.Length != 2)
            {
                return "";
            }
            return answer[1].Trim();
        }

        /// <summary>
        /// Возвращает полное сообщение от сервера, ждет сообщения в течении времени определенного deley
        /// </summary>
        /// <returns>Полное сообщение</returns>
        private string GetMessage()
        {
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (DateTime.Now.Subtract(startTime) >= new TimeSpan(0, 0, 0, 0, deley))
                {
                    return "";
                }
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    return message;
                }
                catch
                {
                    Disconnect();
                }
            }
        }

        /// <summary>
        /// Выполняет закрытие соединения с сервером
        /// </summary>
        public void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
        }
    }
}
