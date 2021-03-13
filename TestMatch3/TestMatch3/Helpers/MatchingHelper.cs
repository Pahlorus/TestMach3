
using MatchCore;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TestMatch3
{
    public struct BonusPoint
    {
        public Point point;
        public BitType type;
    }

    public class MatchingHelper
    {
        public List<Match> GetHorizontalMatches(Cell[][] grid, int colCount, int rowCount)
        {
            List<Match> matches = new List<Match>();
            BitType type;
            Match match;
            for (int y = 0; y < rowCount; y++)
            {
                type = grid[0][y].AssignedBit.BitType;
                match = new Match();
                for (int x = 0; x < colCount; x++)
                {
                    Bit bit = grid[x][y].AssignedBit;
                    if (type == bit.BitType)
                    {
                        match.Add(new Point(x, y));
                        match.Type = type;
                    }
                    else
                    {
                        if (match.Count >= 3) matches.Add(match);
                        type = bit.BitType;
                        match = new Match();
                        match.Add(new Point(x, y));
                        match.Type = type;
                    }
                }
                if (match.Count >= 3) matches.Add(match);
            }
            return matches;
        }

        public List<Match> GetVerticalMatches(Cell[][] grid, int colCount, int rowCount)
        {
            List<Match> matches = new List<Match>();
            BitType type;
            Match match;
            for (int x = 0; x < colCount; x++)
            {
                type = grid[x][0].AssignedBit.BitType;
                match = new Match();
                for (int y = 0; y < rowCount; y++)
                {
                    Bit bit = grid[x][y].AssignedBit;
                    if (type == bit.BitType)
                    {
                        match.Add(new Point(x, y));
                        match.Type = type;
                    }
                    else
                    {
                        if (match.Count >= 3) matches.Add(match);
                        type = bit.BitType;
                        match = new Match();
                        match.Add(new Point(x, y));
                        match.Type = type;
                    }
                }
                if (match.Count >= 3) matches.Add(match);
            }
            return matches;
        }

        private BonusPoint GetBonusPoint(Match match, Point point)
        {
            BonusPoint bonusPoint = new BonusPoint();
            bonusPoint.point = point;
            bonusPoint.type = match.Type;
            return bonusPoint;
        }

        private BonusPoint GetBonusPoint(Bit bit, Point point)
        {
            BonusPoint bonusPoint = new BonusPoint();
            bonusPoint.point = point;
            bonusPoint.type = bit.BitType;
            return bonusPoint;
        }

        public List<BonusPoint> GetCommonBombPoints(List<Match> horMatches, List<Match> vertMatches, List<Match> destroy)
        {
            List<BonusPoint> points = new List<BonusPoint>();
            List<Match> excludeVert = new List<Match>();
            List<Match> excludeHor = new List<Match>();
            foreach (var hor in horMatches)
            {
                foreach (var vert in vertMatches)
                {
                    if (IsIntersect(hor, vert))
                    {
                        points.Add(GetBonusPoint(hor, new Point(vert.Pos.X,hor.Pos.Y)));
                        excludeHor.Add(hor);
                        excludeVert.Add(vert);
                    }
                }
            }
            destroy.AddRange(excludeVert);
            destroy.AddRange(excludeHor);
            foreach (var match in excludeVert) vertMatches.Remove(match);
            foreach (var match in excludeHor) horMatches.Remove(match);
            return points;
        }

        public List<BonusPoint> GetLineBombPoints(List<Match> horMatches, List<Match> vertMatches, Bit firstBit = null, Bit secondBit = null)
        {
            List<BonusPoint> points = new List<BonusPoint>();
            foreach (var hor in horMatches)
            {
                if (hor.Count >= 5)
                {
                    BonusPoint bombPoint = GetBonusPoint(hor, new Point(hor.Pos.X + 3, hor.Pos.Y)); 
                    if (firstBit != null && firstBit.Position.Y == hor.Pos.Y && firstBit.Position.X >= hor.Pos.X && firstBit.Position.X < hor.Pos.X + hor.Count)
                    {
                        bombPoint = GetBonusPoint(firstBit, new Point(firstBit.Position.X, firstBit.Position.Y));
                    }
                    if (secondBit != null && secondBit.Position.Y == hor.Pos.Y && secondBit.Position.X >= hor.Pos.X && secondBit.Position.X < hor.Pos.X + hor.Count)
                    {
                        bombPoint = GetBonusPoint(firstBit, new Point(secondBit.Position.X, secondBit.Position.Y));
                    }
       
                    points.Add(bombPoint);
                }
            }

            foreach (var vert in vertMatches)
            {
                if (vert.Count >= 5)
                {
                    BonusPoint bombPoint = GetBonusPoint(vert, new Point(vert.Pos.X, vert.Pos.Y+3));
                    if (firstBit != null && firstBit.Position.X == vert.Pos.X && firstBit.Position.Y >= vert.Pos.Y && firstBit.Position.Y < vert.Pos.Y + vert.Count)
                    {
                        bombPoint = GetBonusPoint(firstBit, new Point(firstBit.Position.X, firstBit.Position.Y));
                    }
                    if (secondBit != null && secondBit.Position.X == vert.Pos.X && secondBit.Position.Y >= vert.Pos.Y && secondBit.Position.Y < vert.Pos.Y + vert.Count)
                    {
                        bombPoint = GetBonusPoint(firstBit, new Point(secondBit.Position.X, secondBit.Position.Y));
                    }
                        points.Add(bombPoint);
                }
            }
            return points;
        }

        public List<BonusPoint> GetHorizontalLinesList(List<Match> horMatches, List<Match> destroy, Bit firstBit = null, Bit secondBit = null)
        {
            List<BonusPoint> points = new List<BonusPoint>();
            foreach (var hor in horMatches)
            {
                if (hor.Count == 4)
                {
                    BonusPoint bombPoint = GetBonusPoint(hor, new Point(hor.Pos.X + 1, hor.Pos.Y));
                    if (firstBit != null && firstBit.Position.Y == hor.Pos.Y && firstBit.Position.X >= hor.Pos.X && firstBit.Position.X < hor.Pos.X + hor.Count)
                    {
                        bombPoint = GetBonusPoint(firstBit, new Point(firstBit.Position.X, firstBit.Position.Y));
                    }

                    if (secondBit != null && secondBit.Position.Y == hor.Pos.Y && secondBit.Position.X >= hor.Pos.X && secondBit.Position.X < hor.Pos.X + hor.Count)
                    {
                        bombPoint = GetBonusPoint(firstBit, new Point(secondBit.Position.X, secondBit.Position.Y));
                    }
                    points.Add(bombPoint);
                }
     
                destroy.Add(hor);
            }
            return points;
        }

        public List<BonusPoint> GetVerticalLinesList(List<Match> vertMatches, List<Match> destroy, Bit firstBit = null, Bit secondBit = null)
        {
            List<BonusPoint> points = new List<BonusPoint>();

            foreach (var vert in vertMatches)
            {
                if (vert.Count == 4)
                {
                    BonusPoint bombPoint = GetBonusPoint(vert, new Point(vert.Pos.X, vert.Pos.Y+1));
                    if (firstBit != null && firstBit.Position.X == vert.Pos.X && firstBit.Position.Y >= vert.Pos.Y && firstBit.Position.Y < vert.Pos.Y + vert.Count)
                    {
                        bombPoint = GetBonusPoint(firstBit, new Point(firstBit.Position.X, firstBit.Position.Y));
                    }
                    if (secondBit != null && secondBit.Position.X == vert.Pos.X && secondBit.Position.Y >= vert.Pos.Y && secondBit.Position.Y < vert.Pos.Y + vert.Count)
                    {
                        bombPoint = GetBonusPoint(firstBit, new Point(secondBit.Position.X, secondBit.Position.Y));
                    }
                    points.Add(bombPoint);
                }
                destroy.Add(vert);
            }
            return points;
        }

        private bool IsIntersect(Match horMatch, Match vertMatch)
        {
            Point vert = vertMatch.Pos;
            Point hor = horMatch.Pos;
            if (horMatch.Type == vertMatch.Type)
            {
                if (hor.X <= vert.X && vert.X < hor.X + horMatch.Count)
                {
                    if (vert.Y <= hor.Y && hor.Y < vert.Y + vertMatch.Count)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsHaveMatchingVariants(Cell[][] grid, Point firstPoint, Point secondPoint)
        {
            BitType type1 = grid[secondPoint.X][secondPoint.Y].AssignedBit.BitType;
            BitType type2 = grid[firstPoint.X][firstPoint.Y].AssignedBit.BitType;
            return GetSameColorCrossCollapse(grid, firstPoint, type1, secondPoint) || GetSameColorCrossCollapse(grid, secondPoint, type2, firstPoint);
        }

        private bool GetSameColorCrossCollapse(Cell[][] grid, Point pos, BitType type, Point noCheckDirection)
        {
            int leftCount = GetIdenticalTypeCount(grid, pos, new Point(-1, 0), type, noCheckDirection);
            int rightCount = GetIdenticalTypeCount(grid, pos, new Point(1, 0), type, noCheckDirection);
            int topCount = GetIdenticalTypeCount(grid, pos, new Point(0, -1), type, noCheckDirection);
            int bottomCount = GetIdenticalTypeCount(grid, pos, new Point(0, 1), type, noCheckDirection);

            int horizontalCount = leftCount + rightCount;
            int verticalCount = topCount + bottomCount;
            return horizontalCount >= 2 || verticalCount >= 2;
        }

        private int GetIdenticalTypeCount(Cell[][] grid, Point pos, Point dir, BitType type, Point noCheckDirection)
        {
            int count = 0;
            Point nextPose = pos + dir;
            if (nextPose != noCheckDirection && IsPosInGrid(nextPose) && grid[nextPose.X][nextPose.Y].AssignedBit.BitType == type)
            {
                count++;
                nextPose += dir;
                if (IsPosInGrid(nextPose) && grid[nextPose.X][nextPose.Y].AssignedBit.BitType == type) count++;

            }
            return count;
        }

        public (bool isGridArea, Point cellPoint) IsClickInCrid(Point pos)
        {
            Point point = GetCellPoint(pos);
            return (IsPosInGrid(point), point);
        }

        public bool IsPosInGrid(Point pos)
        {
            return pos.X >= 0 && pos.X < GameSettings.boardColCount && pos.Y >= 0 && pos.Y < GameSettings.boardRowCount;
        }

        public Point GetCellPoint(Vector2 pos) //TODO: громоздко и избыточно
        {
            int step = GameSettings.BoardStep;
            Point boardPoint = GameSettings.BoardCoord();
            Vector2 boardPos = new Vector2(boardPoint.X, boardPoint.Y);
            Vector2 Step = new Vector2(step, step);
            Vector2 position = pos - boardPos - new Vector2(step, step) / 2;
            return new Point(Mathf.RoundToInt(position.X / step), Mathf.RoundToInt(position.Y / step));
        }

        public Point GetCellPoint(Point pos)
        {
            return GetCellPoint(new Vector2(pos.X, pos.Y));
        }
    }
}