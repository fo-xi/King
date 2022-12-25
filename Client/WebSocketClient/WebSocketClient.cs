﻿using Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Client.WebSocketClient
{
	public class WebSocketClient
	{
		private string _sessionID;

		private Game _game;

		private WebSocket _webSocketClient;

		private int _playerID;

		private string _playerName;

		public Game Game
		{
			get
			{
				return _game;
			}
			set
			{
				_game = value;
				PlayerID = Game.GameState.Players.Find(x => x.Name.Contains(PlayerName)).ID;
				DataChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public int PlayerID
		{
			get
			{
				return _playerID;

			}
			set
			{
				_playerID = value;
			}
		}

		public string PlayerName
		{
			get
			{
				return _playerName;

			}
			set
			{
				_playerName = value;
			}
		}

		public event EventHandler DataChanged;

		public WebSocketClient()
		{
			Task.Run(Start);
		}

		public void StartGame(string playerName)
		{
			PlayerName = playerName;

			// Формируем запрос
			var data = new
			{
				session_id = _sessionID,
				player_name = playerName,
				action = "play",
			};

			// Трансформируем
			string jsonData = JsonConvert.SerializeObject(data);
			_webSocketClient.Send(jsonData);
		}

		public void CloseClient()
		{
			if (_webSocketClient != null && _webSocketClient.ReadyState == WebSocketState.Open)
			{
				_webSocketClient.Close();
			}
		}

		private async void Start()
		{
			_webSocketClient = new WebSocket("ws://localhost:8080/ws");
			try
			{
				_webSocketClient.OnMessage += (sender, e) => IdentifyAnswer(e.Data);
				_webSocketClient.Connect();

			}
			catch (WebSocketException e)
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

			var state = json.SelectToken("game_state.state").ToString();


            if (json.ContainsKey("game_session_id") &&
                !string.IsNullOrEmpty(state) &&
                state == "started")
			{
				Game = JsonConvert.DeserializeObject<Game>(json.ToString());
				return;
			}
		}
	}
}
