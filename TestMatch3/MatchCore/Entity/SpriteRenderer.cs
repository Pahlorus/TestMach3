
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchCore
{
    public class SpriteRenderer
    {
        public bool Visible { get; set; }
        public float Scale{ get; set; }
        public float Rotation { get; set; }
        public float LayerDepth { get; set; }
        public Color Color { get; set; }

        public Rectangle Rectangle { get; set; }
        public Texture2D Texture { get; set; }

        public SpriteRenderer(RendererData settings)
        {
            Rectangle = settings.rect;
            Color = settings.color;
            LayerDepth = settings.layer;
            Texture = settings.texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           if(Visible) spriteBatch.Draw(Texture, Rectangle, null, Color, Rotation, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }
    }
}