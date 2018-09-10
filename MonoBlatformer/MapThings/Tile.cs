using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBlatformer.MapThings
{
    public enum TileType { Solid, Empty, OneWay }

    public class Tile
    {
        private Rectangle _sourceRect;
        private int _width;
        private int _height;

        public TileType TileType { get; set; }
    }
}
