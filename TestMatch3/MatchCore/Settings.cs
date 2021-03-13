
using Microsoft.Xna.Framework;

namespace TestMatch3
{
   public static class GameSettings
    {
        //TODO: необходимо сериализовать и сохранять в файл.

        #region Game

        public const int timer = 60;
        public const int scoreCost = 1;

        #endregion

        #region Screen

        public const int screenWidth = 600;
        public const int screenHeight = 800;

        #endregion


        #region Button

        public const int buttonWidth = 200;
        public const int buttonHeight = 80;
        public const float buttonLayer = 0.5f;
        public const string defaultButtonTextureName = "Default";
        public const string pressedButtonTextureName = "Pressed";

        #endregion

        #region Bit

        public const int bitSize = 60;
        public const float bitLayer = 0.5f;
        public const float bitModifierLayer = 0.4f;

        #endregion

        #region Destroyer

        public const int destroyerSize = 60;
        public const float destroyerLayer = 0.3f;
        public const string destroyerTextureName = "Destroyer";
        #endregion

        #region Cell

        public const int cellSize = 60;
        public const float celLayer = 0.6f;
        public const string cellTextureName = "Cell";

        #endregion

        #region Background

        public const int backgroundWidth = 600;
        public const int backgroundHeight = 800;
        public const float backgroundLayer = 0.8f;
        public const string gameTextureName = "BackGame";
        public const string menuTextureName = "BackMenu";
        public const string gameOverTextureName = "BackGameOver";

        #endregion

        #region HeadMask

        public const int maskWidth = 600;
        public static int MaskHeight => screenHeight/2- BoardSize.Y/2;
        public const float maskLayer = 0.3f;
        public const string maskTextureName = "BackGame";

        #endregion

        #region GameBoard

        private static int _step;
        public const int gridLineWidth = 6;
        public const int boardColCount = 8;
        public const int boardRowCount = 8;

        public static int BoardStep
        {
            get
            {
                if(_step==0)
                {
                    _step = cellSize + gridLineWidth;
                }
                return _step;
            }
        }

        public const float boardLayer = 0.7f;

        public const string boardTextureName = "Board";

        private static Point _point;

        public static Color boardEvenCellColor = Color.DarkGray;
        public static Color boardOddCellColor = Color.Gray;
        public static Color boardBackolor = Color.Gold;
        public static Point BoardCoord() => new Point((screenWidth - BoardSize.X) / 2, (screenHeight - BoardSize.Y) / 2);

        public static Point BoardSize
        {
            get
            {
                if (_point.X == 0)
                {
                    _point = GetBoardSize();
                }
                return _point;
            }
        }
        #endregion

        private static Point GetBoardSize()
        {
            int width = boardColCount * (cellSize + gridLineWidth) - gridLineWidth;
            int height = boardRowCount * (cellSize + gridLineWidth) - gridLineWidth;
            return new Point(width, height);
        }

    }
}