using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBlatformer.MapThings
{
    public enum TileType { Grass, Jungle, Empty, OneWay }

    public class Tile
    {
        private Rectangle _sourceRectangle;

        public TileType TileType { get; }
        public bool IsGround { get; }
        public bool IsSolid { get; }
        public bool IsEmpty { get; }
        public bool IsOneWay { get; }

        public Tile(TileType tileType, bool isGround, bool isSolid, bool isEmpty, bool isOneWay, Rectangle srcRect)
        {
            TileType = tileType;
            IsGround = isGround;
            IsSolid = isSolid;
            IsEmpty = IsEmpty;
            IsOneWay = isOneWay;
            _sourceRectangle = srcRect;
        }
    }
}
