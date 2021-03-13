
using MatchCore;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestMatch3
{
    public class OperationHelper
    {
        private MatchingHelper _matchingHelper;
        private EntityManager _entityManager;

        public bool IsSCorutineInAction { get; private set; }
        public bool IsBitInAction => BitInAction > 0;
        public int BitInAction { get; private set; }
        public State State { get; set; }

        public OperationHelper(EntityManager entityManager, MatchingHelper matchingHelper)
        {
            State = State.Init;
            Debug.WriteLine("Init");
            _entityManager = entityManager;
            _matchingHelper = matchingHelper;
        }

        private void CreateBonuses(Cell[][] grid, List<BonusPoint> bombs, List<BonusPoint> linesH, List<BonusPoint> linesV)
        {
            CoroutineManager.StartCoroutine(this, CreateBonusesCoroutine(grid, bombs, linesH, linesV));
        }

        private IEnumerator<float> CreateBonusesCoroutine(Cell[][] grid, List<BonusPoint> bombs, List<BonusPoint> linesH, List<BonusPoint> linesV)
        {
            yield return 0;
            BitActionStart();
            
            foreach (var point in bombs)
            {
                BitAssign( point, ModifiType.Bomb);
            }
            foreach (var point in linesH)
            {
                BitAssign(point, ModifiType.HLine);
            }
            foreach (var point in linesV)
            {
                BitAssign(point, ModifiType.VLine);
            }
            BitActionComplete();
            void BitAssign(BonusPoint point, ModifiType type)
            {
                Bit bit = _entityManager.CreateBit(point.type, type);
                Cell cell = grid[point.point.X][point.point.Y];
                bit.Position = cell.Position;
                cell.AssignedBit = bit;
            }
        }

        public void CheckMatches(Cell[][] grid, List<Bit> unstableBits, List<Destroyer> destroyers, int colCount, int rowCount)
        {
            CoroutineManager.StartCoroutine(this, CheckMatchesCoroutine(grid, unstableBits, destroyers, colCount, rowCount));
        }

        public IEnumerator<float> CheckMatchesCoroutine(Cell[][] grid, List<Bit> unstableBits, List<Destroyer> destroyers, int colCount, int rowCount)
        {
            State = State.Matching;
            Debug.WriteLine("Matching");
            yield return 0;
            yield return 0;
            while (IsBitInAction) yield return 0;
            Bit firstBit = null;
            Bit secondBit = null;
            List<Match> horMatch = _matchingHelper.GetHorizontalMatches(grid, colCount, rowCount);
            List<Match> vertMatch = _matchingHelper.GetVerticalMatches(grid, colCount, rowCount);
            List<Match> forDestroy = new List<Match>();
            List<BonusPoint> bombs = _matchingHelper.GetCommonBombPoints(horMatch, vertMatch, forDestroy);
            List<BonusPoint> linesH = _matchingHelper.GetHorizontalLinesList(horMatch, forDestroy, firstBit, secondBit);
            List<BonusPoint> linesV = _matchingHelper.GetVerticalLinesList(vertMatch, forDestroy, firstBit, secondBit);
            List<BonusPoint> lineBombs = _matchingHelper.GetLineBombPoints(horMatch, vertMatch, firstBit, secondBit);
            bombs.AddRange(lineBombs);
            if (forDestroy.Count > 0)
            {
                DestroyMatchedBits(grid, destroyers, forDestroy);
                yield return 0;
                while (IsBitInAction) yield return 0;
                if (bombs.Count + linesH.Count+ linesV.Count+ lineBombs.Count >0)
                {
                    CreateBonuses(grid, bombs, linesH, linesV);
                }
                yield return 0;
                while (IsBitInAction) yield return 0;
                CrambleBits(grid, unstableBits);
                yield return 0;
                while (IsBitInAction) yield return 0;
                CheckMatches(grid, unstableBits, destroyers, colCount, rowCount);
            }
            else { State = State.Wait; Debug.WriteLine("Wait"); }
        }

        public IEnumerator<float> WhaitTimeEndExecute(float delay, Action callBack)
        {
            bool isWaite = true;
            while (isWaite)
            {
                delay -= (float)CoroutineManager.Time.ElapsedGameTime.Milliseconds * 0.001f;
                if (delay <= 0)
                {
                    callBack?.Invoke();
                    yield break;
                }
                yield return 0;
            }
        }

        public void WaitActionAndExecute(Action callBack)
        {
            CoroutineManager.StartCoroutine(this, WaitActionEndExecuteCoroutine(callBack));
        }

        public IEnumerator<float> WaitActionEndExecuteCoroutine(Action callBack)
        {
            while (IsBitInAction) yield return 0;
            callBack?.Invoke();
        }

        public IEnumerator<float> BitDestroyCoroutine(Cell cell)
        {
            if(cell.IsEmpty || cell.AssignedBit.IsDestroing) yield break;
            Bit bit = cell.AssignedBit;
            bit.IsDestroing = true;
            float speed = 0.002f;
            BitActionStart();
            while (true)
            {
                float deltaTime = speed * CoroutineManager.Time.ElapsedGameTime.Milliseconds;

                bit.Scale -= deltaTime;

                if (bit.Scale <= 0)
                {
                    TestMatch3Game.Score += GameSettings.scoreCost;
                    bit.Scale = 0;
                    cell.AssignedBit = null;
                    BitActionComplete();
                    yield break;
                }
                yield return 0;
            }
        }

        public void BitInterchange(Bit firstBit, Bit nextBit)
        {
            CoroutineManager.StartCoroutine(this, BitInterchangeCoroutine(firstBit, nextBit));
        }

        public IEnumerator<float> BitInterchangeCoroutine(Bit firstBit, Bit nextBit)
        {
            while (IsBitInAction) yield return 0;
            State = State.Change;
            Debug.WriteLine("Change");
            CoroutineManager.StartCoroutine(this, SimpleBitPositionChangeCoroutine(firstBit, nextBit.Position));
            CoroutineManager.StartCoroutine(this, SimpleBitPositionChangeCoroutine(nextBit, firstBit.Position));
        }

        public IEnumerator<float> SimpleBitPositionChangeCoroutine(Bit bit, Point nextBitPoint)
        {

            Point bitPoint = bit.Position;
            if (bitPoint == nextBitPoint) yield break;

            int speed = 2;
            bool isCrumble = true;
            Point shif = nextBitPoint - bitPoint;
            int signX = Math.Sign(shif.X);
            int signY = Math.Sign(shif.Y);
                
            BitActionStart();
            while (isCrumble)
            {
                int deltaTime = speed * CoroutineManager.Time.ElapsedGameTime.Milliseconds / 10;

                if(signX>0)
                {
                    bit.Position = new Point(bit.Position.X + deltaTime, bit.Position.Y);
                    if (bit.Position.X > nextBitPoint.X)
                    {
                        StopHandler();
                        yield break;
                    }
                }
                else if(signX < 0)
                {
                    bit.Position = new Point(bit.Position.X - deltaTime, bit.Position.Y);
                    if (bit.Position.X < nextBitPoint.X)
                    {
                        StopHandler();
                        yield break;
                    }
                }

                if (signY > 0)
                {
                    bit.Position = new Point(bit.Position.X , bit.Position.Y + deltaTime);
                    if (bit.Position.Y > nextBitPoint.Y)
                    {
                        StopHandler();
                        yield break;
                    }
                }
                else if (signY < 0)
                {
                    bit.Position = new Point(bit.Position.X , bit.Position.Y - deltaTime);
                    if (bit.Position.Y < nextBitPoint.Y)
                    {
                        StopHandler();
                        yield break;
                    }
                }
                yield return 0;
            }

            void StopHandler()
            {
                isCrumble = false;
                BitActionComplete();
                bit.Position = nextBitPoint;
            }
        }

        public void CrambleBits(Cell[][] grid, List<Bit> unstableBits)
        {
            CoroutineManager.StartCoroutine(this, CrambleBitsCoroutine(grid, unstableBits));
        }

        private IEnumerator<float> CrambleBitsCoroutine(Cell[][] grid, List<Bit> unstableBits)
        {
            while (IsBitInAction) yield return 0;
            foreach (var cells in grid)
            {
                List<Bit> list = CheckUnstableBits(cells);
                unstableBits.AddRange(list);
                foreach (var bit in list)
                {
                    CoroutineManager.StartCoroutine(bit, BitCrumbleDownCoroutine(cells, unstableBits, bit));
                }
            }
        }

        public void DestroyMatchedBits(Cell[][] grid, List<Destroyer> destroyers, List<Match> matches)
        {
            CoroutineManager.StartCoroutine(this, DestroyMatchedBitsCoroutine(grid, destroyers, matches));
        }

        IEnumerator<float> DestroyMatchedBitsCoroutine(Cell[][] grid, List<Destroyer> destroyers, List<Match> matches)
        {
            while (IsBitInAction) yield return 0;
            foreach (var match in matches)
            {
                CommonBitDestroy(grid, destroyers, match.Points);
            }

            foreach (var destr in destroyers) CoroutineManager.StartCoroutine(destr, DestroyerMoveCoroutine(grid, destroyers, destr));
        }

        private void CommonBitDestroy(Cell[][] grid, List<Destroyer> destroyers, List<Point> points)
        {
            foreach (var point in points)
            {
                Cell cell = grid[point.X][point.Y];
                if (cell.IsEmpty || cell.AssignedBit.IsDestroing) continue;
                Point pos = cell.AssignedBit.Position;
                switch (cell.AssignedBit.ModifiType)
                {
                    default:
                    case ModifiType.None:
                        {
                            CoroutineManager.StartCoroutine(cell, BitDestroyCoroutine(cell));
                            break;
                        }
                    case ModifiType.Bomb:
                        {
                            CoroutineManager.StartCoroutine(cell, BombExplosiveCoroutine(grid, destroyers, cell));
                            break;
                        }
                    case ModifiType.HLine:
                        {
                            HLineDestroyHandler(destroyers, cell, pos);
                            break;
                        }
                    case ModifiType.VLine:
                        {
                            VLineDestroyHandler(destroyers, cell, pos);
                            break;
                        }
                }
            }
        }

        private void HLineDestroyHandler(List<Destroyer> destroyers, Cell cell, Point pos)
        {
            Destroyer leftDestr = _entityManager.CreateDestroyer();
            leftDestr.Type = DestroyerType.Left;
            leftDestr.Position = pos;
            destroyers.Add(leftDestr);
            Destroyer rightDestr = _entityManager.CreateDestroyer();
            rightDestr.Type = DestroyerType.Right;
            rightDestr.Position = pos;
            destroyers.Add(rightDestr);
            CoroutineManager.StartCoroutine(cell, BitDestroyCoroutine(cell));
        }

        private void VLineDestroyHandler(List<Destroyer> destroyers, Cell cell, Point pos)
        {
            Destroyer topDestr = _entityManager.CreateDestroyer();
            topDestr.Type = DestroyerType.Top;
            topDestr.Position = pos;
            destroyers.Add(topDestr);
            Destroyer bottomDestr = _entityManager.CreateDestroyer();
            bottomDestr.Type = DestroyerType.Bottom;
            bottomDestr.Position = pos;
            destroyers.Add(bottomDestr);
            CoroutineManager.StartCoroutine(cell, BitDestroyCoroutine(cell));
        }

        public IEnumerator<float> BombExplosiveCoroutine(Cell[][] grid, List<Destroyer> destroyers, Cell cell)
        {
            float delay = 0.25f;
            bool isDelay = true;
            while (isDelay)
            {
                delay -=CoroutineManager.Time.ElapsedGameTime.Milliseconds * 0.001f;
                if(delay<=0)
                {
                    isDelay = false;
                    CoroutineManager.StartCoroutine(cell, BitDestroyCoroutine(cell));
                    List<Point> points = new List<Point>();

                    for (int x = cell.GridPosition.X - 1; x <= cell.GridPosition.X + 1; x++)
                    {
                        for (int y = cell.GridPosition.Y - 1; y <= cell.GridPosition.Y + 1; y++)
                        {
                            if (x >= 0 && x < GameSettings.boardColCount && y >= 0 && y < GameSettings.boardRowCount)
                            {
                                Point point = new Point(x, y);
                                points.Add(point);
                            }
                        }
                    }
                    CommonBitDestroy(grid, destroyers, points);
                }
                yield return 0;
            }
        }

        public IEnumerator<float> DestroyerMoveCoroutine(Cell[][] grid, List<Destroyer> destroyers, Destroyer destr)
        {
            int speed = 3;
            bool isCrumble = true;
            int lengthY = GameSettings.boardColCount-1;
            int lengthX = GameSettings.boardRowCount-1;
            BitActionStart();
            while (isCrumble)
            {
                int deltaTime = speed * CoroutineManager.Time.ElapsedGameTime.Milliseconds / 10;
                switch(destr.Type)
                {
                    case DestroyerType.Top: destr.Position = new Point(destr.Position.X, destr.Position.Y - deltaTime); break;

                    case DestroyerType.Bottom: destr.Position = new Point(destr.Position.X, destr.Position.Y + deltaTime); break;

                    case DestroyerType.Right: destr.Position = new Point(destr.Position.X + deltaTime, destr.Position.Y); break;

                    case DestroyerType.Left: destr.Position = new Point(destr.Position.X - deltaTime, destr.Position.Y); break;

                    default: destr.Position = new Point(-1, -1); break;
                }

                Point cellPoint = _matchingHelper.GetCellPoint(destr.CenterPos);
                if (cellPoint.Y <0 || cellPoint.Y > lengthY|| cellPoint.X > lengthX || cellPoint.X < 0)
                {
                    destroyers.Remove(destr);
                    destr = null;
                    BitActionComplete();
                    yield break;
                }
                else
                {
                    Cell cell = grid[cellPoint.X][cellPoint.Y];
                    if(!cell.IsEmpty)
                    {
                        switch (cell.AssignedBit.ModifiType)
                        {
                            case ModifiType.VLine:
                            case ModifiType.HLine:
                            case ModifiType.None: CoroutineManager.StartCoroutine(cell, BitDestroyCoroutine(cell)); break;
                            case ModifiType.Bomb: CoroutineManager.StartCoroutine(cell, BombExplosiveCoroutine(grid, destroyers, cell)); break;
                        }
                    }
                }
                yield return 0;
            }
        }

        public IEnumerator<float> BitCrumbleDownCoroutine(Cell[] column, List<Bit> unstableBits, Bit bit)
        {
            int speed = 3;
            bool isCrumble = true;
            int length = column.Length;
            BitActionStart();
            while (isCrumble)
            {
                int deltaTime = speed * CoroutineManager.Time.ElapsedGameTime.Milliseconds / 10;

                bit.Position = new Point(bit.Position.X, bit.Position.Y + deltaTime);

                Point cellPoint = _matchingHelper.GetCellPoint(bit.Position);
                int nextCell = cellPoint.Y + 1;
                if (nextCell >= 0)
                {
                    if (nextCell >= length || !column[nextCell].IsEmpty)
                    {
                        Cell cell = column[cellPoint.Y];
                        bit.Position = new Point(bit.Position.X, cell.Position.Y);
                        cell.AssignedBit = bit;
                        bit.CellPoint = cell.GridPosition;
                        unstableBits.Remove(bit);
                        cell.IsUnstable = false;
                        isCrumble = false;
                        BitActionComplete();
                        yield break;
                    }
                }
                yield return 0;
            }
        }

        public List<Bit> CheckUnstableBits(Cell[] column)
        {
            List<Bit> list = new List<Bit>();
            bool isStable = true;
            int emptyCount = 0;
            for (int i = column.Length - 1; i >= 0; i--)
            {
                Cell cell = column[i];
                if (isStable)
                {
                    if (cell.IsEmpty)
                    {
                        isStable = false;
                        emptyCount++;
                    }
                }
                else
                {
                    if (cell.IsEmpty)
                    {
                        emptyCount++;
                        continue;
                    }
                    list.Add(cell.AssignedBit);
                    cell.IsUnstable = true;
                    cell.AssignedBit = null;
                }
            }
            list.AddRange(_entityManager.CreateNewBits(column, GameSettings.BoardStep, emptyCount));
            return list;
        }

        private void BitActionComplete()
        {
            BitInAction--;
            if (BitInAction == 0) IsSCorutineInAction = false;
        }

        private void BitActionStart()
        {
            BitInAction++;
        }
    }
}