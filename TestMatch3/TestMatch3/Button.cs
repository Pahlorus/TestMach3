
using MatchCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TestMatch3
{
    public class Button
    {
        private Point _point;
        private SpriteRenderer _renderer;

        public bool IsVisible
        {
            get => _renderer.Visible;
            set => _renderer.Visible = value;
        }
        public bool IsPressed { get; private set; }
        public string Name { get; set; }
        public Action OnClick;
        public Texture2D DefaultTex { get; set; }
        public Texture2D PressedTex { get; set; }
        public Texture2D Texture
        {
            get => _renderer.Texture;
            set => _renderer.Texture = value;
        }

        public SpriteFont Font{ get; set; }

        public Point Point
        {
            get => _point;
            set
            {
                _point = value;
                _renderer.Rectangle = new Rectangle(value.X, value.Y, _renderer.Rectangle.Width, _renderer.Rectangle.Height);
            }
        }

        public Vector2 Position
        {
            get => new Vector2(Point.X + 85, Point.Y+30);
        }

        public Button(ButtonData data)
        {
            DefaultTex = data.data.texture;
            PressedTex = data.data.auxTexture;
            _renderer = new SpriteRenderer(data.data);
            _renderer.Visible = true;
        }

        private void ButtonClickHandler()
        {
            OnClick?.Invoke();
        }

        private void Select()
        {
            if (IsPressed) Texture = PressedTex;
            else Texture = DefaultTex;
        }

        private void ButtonClickListener()
        {
            if (TestMatch3Game.State != MatchCore.GameState.Game)
            {
                var mouseState = Mouse.GetState();

                if (_renderer.Rectangle.Contains(mouseState.X, mouseState.Y))
                {
                    if (!IsPressed && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        IsPressed = true;
                        Select();
                    }
                    else if (IsPressed && mouseState.LeftButton == ButtonState.Released)
                    {
                        IsPressed = false;
                        Select();
                        ButtonClickHandler();
                    }
                }
            }
            else
            {
                IsPressed = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            ButtonClickListener();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _renderer.Draw(spriteBatch);
            if(IsVisible) spriteBatch.DrawString(Font, Name, Position, Color.White);
        }
    }
}