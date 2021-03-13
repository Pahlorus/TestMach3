using System;
using System.Collections.Generic;
using System.Diagnostics;
using MatchCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestMatch3
{
    public class GameBoard:VisualObject
    {
        private bool _isLoaded;
        private bool _isMousePressed;
        private int _clickCount;
        private Random _random;
        private ResourceManager _resource;
        private SettingsManager _settingsManager;
        private EntityManager _entityManager;
        private MatchingHelper _matchingHelper;
        private OperationHelper _operationHelper;

        private SpriteRenderer _backGround;
        private SpriteRenderer _headMask;

        public int ColumnCount { get; }
        public int RowCount { get; }
        public int Step { get; }
        public State State
        {
            get => _operationHelper.State;
            set => _operationHelper.State = value;
        }
        
        public Cell[][] Grid;
        public List<Bit> unstableBits;
        public List<Destroyer> destroyers;
        public static Bit FirsSelectBit { get; set;}
        public static Bit SecondSelectBit { get; set;}
        public static Point FirstClickPoint { get; set; }
        public static Point SecondtClickPoint { get; set; }


        public GameBoard(ResourceManager resource, SettingsManager settingsManager, GameBoardData data):base(data.background)
        {
            Step = data.step;
            ColumnCount = data.columnCount;
            RowCount = data.rowCount;

            _resource = resource;
            _settingsManager = settingsManager;

            Init(data);
        }

        private void Init(GameBoardData data)
        {
            BackgroundInit();
            Enabled = true;
            _random = new Random();
            Grid = new Cell[ColumnCount][];
            _matchingHelper = new MatchingHelper();
            _entityManager = new EntityManager(_settingsManager, _resource);
            _operationHelper = new OperationHelper(_entityManager, _matchingHelper);
            unstableBits = new List<Bit>();
            destroyers = new List<Destroyer>();
            CreateGrid(data);
        }

        private void BackgroundInit()
        {
            RendererData data = _settingsManager.BackgroundRendererData();
            _backGround = new SpriteRenderer(data);
            _backGround.Visible = true;
            _backGround.Rectangle = data.rect;
            RendererData maskData = _settingsManager.MaskRendererData();
            _headMask = new SpriteRenderer(maskData);
            _headMask.Visible = true;
            _headMask.Rectangle = maskData.rect;
        }



        public void CreateGrid( GameBoardData data)
        {
            RendererData cellData = data.cellData;
            Color evenColor = data.evenCellColor;
            Color oddColor = data.oddCellColor;

            Point boardOffset = GameSettings.BoardCoord();
            for (int x=0; x< ColumnCount; x++)
            {
                Grid[x] = new Cell[RowCount];
                for(int y = 0; y< RowCount;y++)
                {
                    Point pos = boardOffset + new Point(x * Step, y * Step);
                    Cell cell = new Cell(cellData, evenColor);
                    cell.Position = pos;
                    cell.GridPosition = new Point(x,y);
                    Grid[x][y] = cell;
                }
            }
            _isLoaded = true;
            _operationHelper.CrambleBits(Grid, unstableBits);
            _operationHelper.CheckMatches(Grid, unstableBits, destroyers, ColumnCount, RowCount);
            TestMatch3Game.SetStart();
        }

        private void FirstClickHandler(Point pos)
        {
            FirstClickPoint = pos;
            Debug.WriteLine("Click pos1_" + FirstClickPoint);
            FirsSelectBit = Grid[pos.X][pos.Y].AssignedBit;
            FirsSelectBit.Select(SwitchType.On);
        }

        private void SecondClickHandler(Point pos)
        {
            CoroutineManager.StartCoroutine(this, SecondClickHandlerCoroutine(pos));
        }

        private IEnumerator<float> SecondClickHandlerCoroutine(Point pos)
        {
            SecondtClickPoint = pos;
            if(SecondtClickPoint == FirstClickPoint)
            {
                FirsSelectBit.Select(SwitchType.Off);
                FirsSelectBit = null;
                _clickCount = 0;
                yield break;
            }
            SecondSelectBit = Grid[pos.X][pos.Y].AssignedBit;

            Point shift = FirstClickPoint - SecondtClickPoint;
            Debug.WriteLine("Click pos2_" + SecondtClickPoint);
            if (Math.Abs(shift.X) + Math.Abs(shift.Y) != 1)
            {
                FirsSelectBit.Select(SwitchType.Off);
                _clickCount = 0;
            }
            else
            {
                FirsSelectBit.Select(SwitchType.Off);
                _operationHelper.BitInterchange(FirsSelectBit, SecondSelectBit);
                
                if (_matchingHelper.IsHaveMatchingVariants(Grid, FirstClickPoint, SecondtClickPoint))
                {
                    Grid[FirstClickPoint.X][FirstClickPoint.Y].AssignedBit = SecondSelectBit;
                    SecondSelectBit.CellPoint = Grid[FirstClickPoint.X][FirstClickPoint.Y].GridPosition;
                    Grid[SecondtClickPoint.X][SecondtClickPoint.Y].AssignedBit = FirsSelectBit;
                    FirsSelectBit.CellPoint = Grid[SecondtClickPoint.X][SecondtClickPoint.Y].GridPosition;
                    yield return 0;
                    _operationHelper.CheckMatches(Grid, unstableBits,destroyers, ColumnCount, RowCount);
                }
                else
                {
                    yield return 0;
                    _operationHelper.BitInterchange(FirsSelectBit, SecondSelectBit);
                    yield return 0;
                    _operationHelper.WaitActionAndExecute(ClickStatusReset);
                }
            }
        }

        private void ClickStatusReset()
        {
            FirsSelectBit = null;
            SecondSelectBit = null;
            FirstClickPoint = new Point(-1, -1);
            SecondtClickPoint = new Point(-1, -1);
            State = State.Wait;
            Debug.WriteLine("Wait");
        }


        private void ButtonClickHandler(Point pos)
        {

            var result = _matchingHelper.IsClickInCrid(pos);
            if(result.isGridArea)
            {
                _clickCount++;
                if (_clickCount == 1) FirstClickHandler(new Point(result.cellPoint.X, result.cellPoint.Y));
                else if (_clickCount == 2) SecondClickHandler(new Point(result.cellPoint.X, result.cellPoint.Y));
            }
        }

        public void ButtonClickListener()
        {
            if (State == State.Wait)
            {
                var mouseState = Mouse.GetState();
                if (!_isMousePressed && mouseState.LeftButton == ButtonState.Pressed) _isMousePressed = true;
                else if (_isMousePressed && mouseState.LeftButton == ButtonState.Released)
                {
                    _isMousePressed = false;
                    ButtonClickHandler(mouseState.Position);
                }
            }
            else
            {
                _clickCount = 0;
                _isMousePressed = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ButtonClickListener();
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (TestMatch3Game.State != GameState.Game) return;
            base.Draw(spriteBatch);
            if(_isLoaded)
            {
                foreach (var cells in Grid)
                {
                    foreach (var cell in cells)
                    {
                        cell.Draw(spriteBatch);
                    }
                }

                foreach(var bit in unstableBits)
                {
                    bit.Draw(spriteBatch);
                }

                foreach (var destroyer in destroyers)
                {
                    destroyer.Draw(spriteBatch);
                }
                _backGround.Draw(spriteBatch);
                _headMask.Draw(spriteBatch);
            }
        }
    }
}