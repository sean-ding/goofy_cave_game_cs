﻿using CaveGame.Generation;
using CaveGame.Managers;
using CaveGame.Scenes;
using SadConsole.Configuration;
using static CaveGame.GameSettings;
using static CaveGame.Generation.MainGeneration;
using static CaveGame.Managers.ChunkManager;

// :3

namespace CaveGame;

public static class Program
{
	private static int _seed;
	//private static Player _player;
	private static InputHandler _inputHandler;
	private static GameScreen _gameScreen;

	private static void Main()
	{
		Settings.WindowTitle = "Goofy Cave Game";

		Settings.ResizeMode = Settings.WindowResizeOptions.None;

		var gameStartup = new Builder()
			.SetScreenSize(1920 / 8, 1080 / 16)
			.SetStartingScreen<StartScreen>()
			.OnStart(Init)
			.ConfigureFonts((f, g) => { f.AddExtraFonts("Fonts/mdcurses16.font"); });

		Game.Create(gameStartup);
		//Game.Instance.FrameUpdate += Update;
		Game.Instance.Run();
		Game.Instance.Dispose();
	}

	private static void Init(object? sender, GameHost e)
	{
		SadConsole.Host.Global.GraphicsDeviceManager.PreferredBackBufferWidth =
			Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		SadConsole.Host.Global.GraphicsDeviceManager.PreferredBackBufferHeight =
			Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

		SadConsole.Host.Global.GraphicsDeviceManager.ApplyChanges();

		//Game.Instance.ToggleFullScreen();
	}

	private static void Update(object? sender, GameHost e)
	{
	}

	public static void Start()
	{
		// uncomment to enable custom config
		//Game.Instance.Screen = new CustomConfigScreen();

		var ugh = Deserializer.DeserializeJson<EntityType>(@"Data\entityData.json");
		
		int spawnY;
		int spawnX;
		while (true)
		{
			_seed = new Random().Next(int.MinValue, int.MaxValue);
			var startChunk = new Chunk(null, new[] { 0, 0 }, 0, new Cave(), _seed);
			var spawnArea = FindAreaThatIsOfProvidedArea(startChunk, MIN_SPAWN_AREA);
			if (spawnArea != null)
			{
				AddChunk(startChunk);
				LoadSurroundingChunks(startChunk.Position[0], startChunk.Position[1], 0);
				var spawnIndex = SHutil.Random(0, spawnArea.GetLength(0));
				spawnY = spawnArea[spawnIndex, 0];
				spawnX = spawnArea[spawnIndex, 1];
				//for (var i = 0; i < 500; i++)
				break;
			}

			System.Console.WriteLine("Region rejected!");
		}

		_inputHandler = new InputHandler();
		_gameScreen = new GameScreen();
		Game.Instance.Screen = _gameScreen;
		CaveGame();
	}

	private static async void CaveGame()
	{
		while (true)
		{

			var loadedChunks = GetLoadedChunks();

			ThreadPool.GetMaxThreads(out var maxThreads, out _);

			ThreadPool.GetAvailableThreads(out var availableThreads, out _);

			System.Console.WriteLine("Running Threads: " + (maxThreads - availableThreads));

			//ViewManager.UpdateView(_player.Position[0], _player.Position[1], _player.Layer);
		}
	}

	public static InputHandler GetInputHandler()
	{
		return _inputHandler;
	}

	public static GameScreen GetGameScreen()
	{
		return _gameScreen;
	}

	public static int GetSeed()
	{
		return _seed;
	}

	public static void Exit()
	{
		Game.Instance.Screen = new ExitScreen();
	}
}