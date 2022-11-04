using System;
using System.Collections;
using System.Collections.Generic;

namespace Maps
{
    public class Map
    {
        public int? MapId { get; set; }
        public int? CreatorId { get; set; }
        public DateTime? CreationDate { get; set; }
        public List<Point> Points { get; set; }
        public Map(List<Point> points, int? mapId = null, int? creatorId = null, DateTime? creationDate = null)
        {
            Points = points;
            MapId = mapId;
            CreatorId = creatorId;
            CreationDate = CreationDate;
        }
    }
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}