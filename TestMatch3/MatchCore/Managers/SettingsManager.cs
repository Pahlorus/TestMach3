
using MatchCore;
using Microsoft.Xna.Framework;

namespace TestMatch3
{
    public class SettingsManager
    {
        private ResourceManager _resourceManager;

        public SettingsManager(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public GameBoardData GetGameBoardData()
        {
            GameBoardData data = new GameBoardData();
            data.step = GameSettings.BoardStep;
            data.columnCount = GameSettings.boardColCount;
            data.rowCount = GameSettings.boardRowCount;
            data.evenCellColor = GameSettings.boardEvenCellColor;
            data.background = GetBoardRendererData();
            data.cellData = GetCellDataRenderer();
            return data;
        }

        public RendererData BackgroundRendererData()
        {
            RendererData data = new RendererData();
            data.color = Color.White;
            data.layer = GameSettings.backgroundLayer;

            data.rect = new Rectangle(0, 0, GameSettings.backgroundWidth, GameSettings.backgroundHeight);
            data.texture = _resourceManager.GetCommonTexture(GameSettings.gameTextureName);
            return data;
        }

        public RendererData MaskRendererData()
        {
            RendererData data = new RendererData();
            data.color = Color.White;
            data.layer = GameSettings.maskLayer;
            data.rect = new Rectangle(0, 0, GameSettings.maskWidth, GameSettings.MaskHeight);
            data.texture = _resourceManager.GetCommonTexture(GameSettings.maskTextureName);
            return data;
        }

        public RendererData GetBoardRendererData()
        {
            RendererData data = new RendererData();
            data.color = Color.White;
            data.layer = GameSettings.boardLayer;
            Point pos = GameSettings.BoardCoord();
            data.rect = new Rectangle(pos.X, pos.Y, GameSettings.BoardSize.X, GameSettings.BoardSize.Y);
            data.texture = _resourceManager.GetCommonTexture(GameSettings.boardTextureName);
            return data;
        }

        public RendererData GetCellDataRenderer()
        {
            RendererData data = new RendererData();
            data.color = Color.White;
            data.layer = GameSettings.celLayer;
            data.rect = new Rectangle(0, 0, GameSettings.cellSize, GameSettings.cellSize);
            data.texture = _resourceManager.GetCommonTexture(GameSettings.cellTextureName);
            return data;
        }

        public RendererData GetBitRendererData()
        {
            RendererData data = new RendererData();
            data.color = Color.White;
            data.layer = GameSettings.bitLayer;
            data.rect = new Rectangle(0, 0, GameSettings.bitSize, GameSettings.bitSize);
            data.texture = _resourceManager.GetCommonTexture(GameSettings.cellTextureName);
            return data;
        }

        public RendererData GetDestroyerRendererData()
        {
            RendererData data = new RendererData();
            data.color = Color.White;
            data.layer = GameSettings.destroyerLayer;
            data.rect = new Rectangle(0, 0, GameSettings.destroyerSize, GameSettings.destroyerSize);
            data.texture = _resourceManager.GetCommonTexture(GameSettings.destroyerTextureName);
            return data;
        }

        public RendererData GetBitModifierRendererData()
        {
            RendererData data = new RendererData();
            data.color = Color.White;
            data.layer = GameSettings.bitModifierLayer;
            data.rect = new Rectangle(0, 0, GameSettings.bitSize, GameSettings.bitSize);
            return data;
        }

        public ButtonData ButtonRendererData()
        {
            ButtonData data= new ButtonData();
            RendererData rendererData = new RendererData();
            rendererData.color = Color.White;
            rendererData.layer = GameSettings.buttonLayer;
            rendererData.rect = new Rectangle(0, 0, GameSettings.buttonWidth, GameSettings.buttonHeight);
            rendererData.texture = _resourceManager.GetCommonTexture(GameSettings.defaultButtonTextureName);
            rendererData.auxTexture = _resourceManager.GetCommonTexture(GameSettings.pressedButtonTextureName);
            data.data = rendererData;
            return data;
        }
    }
}