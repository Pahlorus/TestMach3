
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchCore
{
    public struct RendererData
    {
        public float layer;
        public Color color;
        public Rectangle rect;
        public Texture2D texture;
        public Texture2D auxTexture;
    }

    public struct ButtonData
    {
        public string name;
        public RendererData data;
    }

    public struct BitData
    {
        public ModifiType modifiType;
        public RendererData mainRendererData;
        public RendererData modifiRendererData;
    }

    public struct GameBoardData
    {
        public int step;
        public int columnCount;
        public int rowCount;
        public Color evenCellColor;
        public Color oddCellColor;
        public RendererData background;
        public RendererData cellData;
    }
}