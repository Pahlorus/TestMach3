
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MatchCore
{
    public class Match
    {
        public int Count => (Points == null)? -1: Points.Count;
        public BitType Type { get; set; }
        public Point Pos { get; private set; }
        public List<Point> Points { get; set; }

        public void Add(Point point)
        {
            if (Points == null)
            {
                Points = new List<Point>();
                Points.Add(point);
                Pos = point;
            }
            else Points.Add(point);
        }
    }
}