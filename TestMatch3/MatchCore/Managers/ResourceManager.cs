
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MatchCore
{
    public class ResourceManager
    {
        private SpriteFont _font;
        private Dictionary<string, Texture2D> commonTextures;
        private ContentManager _contentManager;

        public SpriteFont Font
        {
            get
            {
                if (_font == null)
                {
                    _font = _contentManager.Load<SpriteFont>("candara");
                    return _font;
                }
                return _font;
            }
        }

        public ResourceManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
            commonTextures = new Dictionary<string, Texture2D>();
        }

        public Texture2D GetCommonTexture(string name)
        {
            if(commonTextures.ContainsKey(name))
            {
                return commonTextures[name];
            }
            else
            {
                Texture2D texture = _contentManager.Load<Texture2D>(name);
                commonTextures.Add(name, texture);
                return texture;
            }
        }
    }
}