﻿using Artemis.System;
using FellSky.Framework;
using System.Linq;
using FellSky.Services;
using FellSky.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace FellSky
{
    class GameEngine: Microsoft.Xna.Framework.Game
    {
        public static GameEngine Instance { get; private set; }
        public GraphicsDeviceManager Graphics { get; private set; }
        public Properties.Settings Settings { get { return Properties.Settings.Default; } }
        
        public CoroutineService Coroutines { get; private set; }
        public KeyboardService Keyboard { get; private set; }
        public MouseService Mouse { get; private set; }
        public GameState State { get; set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public SpriteManagerService SpriteManager { get; private set; }
        public TimerService Timer { get; private set; }
        public GuiService Gui { get; private set; }
        public ShapeManagerService ShapeManager { get; private set; }

        private GameEngine()
        {
            Instance = this;
            ServiceLocator.Initialize(Services);
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = Settings.GraphicsWidth;
            Graphics.PreferredBackBufferHeight = Settings.GraphicsHeight;
            Graphics.IsFullScreen = Settings.FullScreen;
            Graphics.SynchronizeWithVerticalRetrace = Settings.VSync;
            IsFixedTimeStep = Settings.FrameLimit > 0;
            if(IsFixedTimeStep) TargetElapsedTime = TimeSpan.FromSeconds(1.0d / Settings.FrameLimit);
            Graphics.ApplyChanges();
            this.IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            Content.RootDirectory = Path.GetFullPath(Settings.DataFolder);
            Environment.CurrentDirectory = Path.GetFullPath(Settings.DataFolder);

            Services.AddService(Content);

            Timer = new TimerService();
            Services.AddService<ITimerService>(Timer);

            Coroutines = new CoroutineService();
            Services.AddService<ICoroutineService>(Coroutines);

            Services.AddService(GraphicsDevice);

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(SpriteBatch);
                       
            Keyboard = new KeyboardService(Coroutines);
            Services.AddService<IKeyboardService>(Keyboard);

            Services.AddService(Window);

            Mouse = new MouseService(Coroutines);
            Services.AddService<IMouseService>(Mouse);

            Services.AddService<IAnimationService>(new AnimationService(Coroutines, Timer));

            Gui = new GuiService();
            Services.AddService<IGuiService>(Gui);

            SpriteManager = new SpriteManagerService(Content);
            Services.AddService<ISpriteManagerService>(SpriteManager);

            ShapeManager = new ShapeManagerService();
            Services.AddService<IShapeManagerService>(ShapeManager);

            Services.AddService(new SpaceBackgroundGeneratorService());

            var testShips = new[] { "Ships/Jaeger.json", "Ships/Scimitar.json" }
                            .Select(s => Persistence.LoadFromFile<Game.Ships.Ship>(s))
                            .ToList();

            State = new ShipRefitState(testShips);
            //State = new MainGameState();
            State.LoadContent();

            EntityFactories.WeaponEntityFactory.LoadWeapons();
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Coroutines.RunCoroutines(gameTime.ElapsedGameTime);
            Timer.LastFrameUpdateTime = gameTime;
            State?.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Timer.LastFrameRenderTime = gameTime;
            State?.Draw(gameTime);
            base.Draw(gameTime);
        }

        static void Main(string[] args)
        {
            new GameEngine();
            Instance.Run();
        }
    }
}
