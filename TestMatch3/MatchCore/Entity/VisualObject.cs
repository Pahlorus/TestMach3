
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchCore
{
    public class VisualObject:GameObject
    {
        private bool _isSelected;
        private float _scale;
        private Point _position;
        private Rectangle _sourceRect;
        private Texture2D _mainTexture;
        private Texture2D _selectTexture;
        private SpriteRenderer _renderer;

        public bool IsSelected => _isSelected;
        public bool IsVisible
        {
            get => _renderer.Visible;
            set => _renderer.Visible = value;
        }

        public float Layer
        {
            get => _renderer.LayerDepth;
            set => _renderer.LayerDepth = value;
        }

        public virtual float Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                _renderer.Scale = _scale;
                SetRectScale(_scale);
            }
        }

        public override Point Position
        {
            get => _position;
            set
            {
                _position = value;
                SetRectPosition(_position);
            }
        }

        public Point CenterPos
        {
            get => Position + new Point(Rect.Width / 2, Rect.Height / 2);
        }

        public Rectangle Rect => _sourceRect;

        public Texture2D RendererTexture
        {
            get => _renderer.Texture;
            set => _renderer.Texture = value;
        }
        public VisualObject (RendererData settings)
        {
            _sourceRect = settings.rect;
            _renderer = new SpriteRenderer(settings);
            _mainTexture = settings.texture;
            _selectTexture = settings.auxTexture;
            _renderer.Rectangle = _sourceRect;
            IsVisible = true;
        }

        public void Select(SwitchType type)
        {
            switch (type)
            {
                default:
                case SwitchType.On:
                    {
                        if (!_isSelected)
                        {
                            _isSelected = true;
                            RendererTexture = _selectTexture;
                        }
                        break;
                    }
                case SwitchType.Off:
                    {
                        if (_isSelected)
                        {
                            _isSelected = false;
                            RendererTexture = _mainTexture;
                        }
                        break;
                    }
            }
        }

        private void SetRectScale(float scale)
        {
            var scaledWidth = Mathf.RoundToInt(_sourceRect.Width * scale);
            var scaledHeight = Mathf.RoundToInt(_sourceRect.Height * scale);
            Point posOfffset = PosScaleOffsetRecalc(scaledWidth, scaledHeight);
            _renderer.Rectangle = new Rectangle(_position.X + posOfffset.X, _position.Y+ posOfffset.Y, scaledWidth, scaledHeight);
        }

        private Point PosScaleOffsetRecalc(int scaledWidth, int scaledHeight)
        {
            var widthOffset = Mathf.RoundToInt((float)(_sourceRect.Width - scaledWidth)/2);
            var heightOffset = Mathf.RoundToInt((float)(_sourceRect.Height - scaledHeight)/2);
            return new Point(widthOffset, heightOffset);
        }

        private void SetRectPosition(Point position)
        {
            _renderer.Rectangle = new Rectangle(position.X, position.Y, _renderer.Rectangle.Width, _renderer.Rectangle.Height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Enabled) _renderer.Draw(spriteBatch);
        }
    }
}