
using Microsoft.Xna.Framework;

namespace MatchCore
{
    public abstract class GameObject
    {
        public bool Enabled { get; set; }
        public virtual Point Position { get; set; }

        public virtual void Update(GameTime gameTime)
        {
            if (!Enabled) return;
        }
    }
}