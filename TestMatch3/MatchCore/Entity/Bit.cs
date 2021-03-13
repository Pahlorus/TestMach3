
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchCore
{
    public class Bit : VisualObject
    {
        public VisualObject _modifier;

        public bool IsDestroing { get; set; }
        public Point CellPoint { get; set; }
        public BitType BitType { get; set; }
        public ModifiType ModifiType { get; set; }

        public override float Scale
        {
            get => base.Scale;
            set
            {
                base.Scale = value;
                if (_modifier != null) _modifier.Scale = value;
            }
        }

        public override Point Position
        {
            get => base.Position;
            set
            {
                base.Position = value ;
                if (_modifier != null) _modifier.Position = value;
            }
        }

        public Texture2D ModifierTexture { set => _modifier.RendererTexture = value; }

        public Bit(BitData data) : base(data.mainRendererData)
        {
            ModifiType = data.modifiType;
            RendererData rendererData = data.mainRendererData;
            Enabled = true;
            _modifier = new VisualObject(data.modifiRendererData);
            _modifier.Enabled = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (ModifiType > 0 && _modifier != null)
            {
                _modifier.Draw(spriteBatch);
            }
            
        }
    }
}