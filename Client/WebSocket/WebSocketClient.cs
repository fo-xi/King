using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.WebSocket
{
	public class WebSocketClient
	{
		private const int BufferSize = 1024;

		private static Uri _uri = new Uri("ws://localhost:5050/ws");
		private static IPHostEntry _host = Dns.GetHostEntry(_uri.Host);
		private static IPAddress _ipAddress = _host.AddressList[0];
		private static IPEndPoint _ipEndPoint = new IPEndPoint(_ipAddress, _uri.Port);
		Socket _socket = new Socket(_ipAddress.AddressFamily,
			SocketType.Stream, ProtocolType.Tcp);

		private Socket _listener;
		private string _sessionID;
		private bool _isRunning;

		public event EventHandler DataChanged;

		public WebSocketClient()
		{
			Task.Run(Start);
		}

		public void StartGame(string playerName)
        {
			// Формируем запрос
			var json = new JObject();
			var newPropertySessionID = new JProperty("session_id", _sessionID);
			json.Last.AddAfterSelf(newPropertySessionID);
			var newPropertyPlayerName = new JProperty("player_name", playerName);
			json.Last.AddAfterSelf(newPropertyPlayerName);
			var newPropertyAction = new JProperty("action", "play");
			json.Last.AddAfterSelf(newPropertyAction);

			string jsonString = json.ToString();

			// Отправляем запрос серверу
			byte[] msg = Encoding.UTF8.GetBytes(jsonString);
			_listener.Send(msg);
		}

		public void CloseClient()
		{
			_isRunning = false;
			if (_listener != null && _listener.Connected)
			{
				_listener.Shutdown(SocketShutdown.Both);
				_listener.Close();
			}
		}

		private void Start()
        {
			try
			{
				_socket.Bind(_ipEndPoint);
				_socket.Listen(4);

				// Программа приостанавливается, ожидая входящее соединение
				_listener = _socket.Accept();
				_isRunning = true;

				// Начинаем слушать соединения
				while (_isRunning)
				{
					string data = null;

					// Мы дождались клиента, пытающегося с нами соединиться
					int bytesRec;

					do
					{
						byte[] bytes = new byte[BufferSize];
						bytesRec = _listener.Receive(bytes);
						data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
					}
					while (bytesRec == BufferSize);

					if (!string.IsNullOrEmpty(data))
					{
						IdentifyAnswer(data);
					}
				}
			}
			catch (SocketException e)
			{
				return;
			}
		}

		private void IdentifyAnswer(string data)
		{
			var json = JObject.Parse(data);

			if (json.ContainsKey("session_id") &&
				!json.ContainsKey("game_session_id"))
			{
				_sessionID = (string)json["session_id"];
				return;
			}

			if (json.ContainsKey("game_session_id") &&
				json.ContainsKey("state") &&
				json["state"].ToString() == "started")
			{
				//аьяи
				return;
			}
		}
	}
}
