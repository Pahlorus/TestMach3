
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchCore
{
    public class Cell:VisualObject
    {
        public bool IsEmpty => AssignedBit == null;
        public bool IsUnstable { get; set; }
        public Point GridPosition { get; set; }
        public Bit AssignedBit { get; set; }

        public Cell(RendererData data, Color color) : base(data)
        {
            Enabled = true;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (!IsEmpty) AssignedBit.Draw(spriteBatch);
        }
    }
}