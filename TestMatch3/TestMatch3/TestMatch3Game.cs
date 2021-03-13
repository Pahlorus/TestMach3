using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MatchCore;
using System;

namespace TestMatch3
{
    public class TestMatch3Game : Game
    {
        private static bool _isStart;
        private float _timer;
        private string TimerText => "Timer: " + _timer.ToString("F0");

        private Vector2 scorePos;
        private Vector2 timerPos;
        private Vector2 gamoverPos;

        public static int Score { get; set; }
        public static string ScoreText
        {
            get => "Score: "+ Score.ToString();
        }

        public static string Time { get; set; }

        public static GameState State { get; set; }

        Button play;
        Button menu;

        SpriteFont Font;

        GameBoard board;
        ResourceManager resource;
        SettingsManager settingsManager;
        CoroutineManager coroutineManager;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public event Action<GameTime> OnUpdate;
        public event Action<GameTime> OnAddScore;

        public TestMatch3Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameSettings.screenWidth;
            graphics.PreferredBackBufferHeight = GameSettings.screenHeight;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            scorePos = new Vector2(50, 50);
            timerPos = new Vector2(50, 80);
            gamoverPos = new Vector2(245, 320);

            resource = new ResourceManager(Content);
            settingsManager = new SettingsManager(resource);
            coroutineManager = new CoroutineManager(this);
            Font = resource.Font;
            play = new Button(settingsManager.ButtonRendererData());
            play.Name = "Play";
            play.OnClick += Play;
            play.IsVisible = true;
            Point buttonPoint = new Point(GameSettings.screenWidth / 2 - GameSettings.buttonWidth / 2, GameSettings.screenHeight / 2 - GameSettings.buttonHeight / 2);
            play.Point = buttonPoint;
            play.Font = resource.Font;

            menu = new Button(settingsManager.ButtonRendererData());
            menu.Name = "Menu";
            menu.OnClick += Menu;
            menu.IsVisible = false;
            menu.Point = buttonPoint;
            menu.Font = resource.Font;
            
            State = GameState.Menu;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public static void SetStart()
        {
            _isStart = true;
        }

        protected void Play()
        {
            if(State == GameState.Menu)
            {
                _timer = GameSettings.timer;
                board = new GameBoard(resource, settingsManager, settingsManager.GetGameBoardData());
                play.IsVisible = false;
                State = GameState.Game;
            }
        }

        protected void Menu()
        {
            if(State == GameState.GameOver)
            {
                board = null;
                play.IsVisible = true;
                State = GameState.Menu;
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            OnUpdate?.Invoke(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            play.Update(gameTime);
            menu.Update(gameTime);
            if(board!=null) board.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront);

            switch(State)
            {
                case GameState.Game:
                    {
                        if (_isStart) _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (_timer < 0)
                        {
                            _timer = 0;
                            _isStart = false;
                            board = null;
                            State = GameState.GameOver;
                        }

                        spriteBatch.DrawString(Font, TimerText, timerPos, Color.White);
                        spriteBatch.DrawString(Font, ScoreText, scorePos, Color.White);
                        if (board != null) board.Draw(spriteBatch);
                        break;
                    }
                case GameState.Menu:
                    {
                        Score = 0;
                        play.Draw(spriteBatch);
                        break;
                    }
                case GameState.GameOver:
                    {
                        spriteBatch.DrawString(Font, TimerText, timerPos, Color.White);
                        spriteBatch.DrawString(Font, ScoreText, scorePos, Color.White);
                        spriteBatch.DrawString(Font, "GAME OVER", gamoverPos, Color.White);
                        menu.Draw(spriteBatch);
                        if (!menu.IsVisible) menu.IsVisible = true;
                            break;
                    }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
