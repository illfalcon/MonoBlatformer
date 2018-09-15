using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBlatformer.MapThings
{
    public class TileManager
    {
        private List<Tile> _tiles;

        public Tile[] Tiles { get { return _tiles.ToArray(); } }

        //cannot think of a better way than to hardcode

        public TileManager()
        {
            _tiles = new List<Tile>();
            //0 empty
            _tiles.Add(new Tile(TileType.Empty, false, false, true, false, new Rectangle(0, 0, 16, 16)));
            //1 grass
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(0, 192, 16, 16)));
            //2 grass
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(16, 192, 16, 16)));
            //3 grass
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(32, 192, 16, 16)));
            //4 grass
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(48, 192, 16, 16)));
            //5 grass
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(64, 192, 16, 16)));
            //6 grass
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(80, 192, 16, 16)));
            //7 grass
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(96, 192, 16, 16)));
            //8 grass
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(112, 192, 16, 16)));
            //9 grass
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(128, 192, 16, 16)));
            //10 grass
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(144, 192, 16, 16)));
            //11 undergrass
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(0, 208, 16, 16)));
            //12 bottom right corner glass
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(96, 112, 16, 16)));
            //13 right grass wall
            _tiles.Add(new Tile(TileType.Grass, true, true, false, false, new Rectangle(96, 96, 16, 16)));
            //14 top left jungle
            _tiles.Add(new Tile(TileType.Jungle, true, false, false, true, new Rectangle(336, 192, 16, 16)));
            //15 top mid jungle
            _tiles.Add(new Tile(TileType.Jungle, true, false, false, true, new Rectangle(352, 192, 16, 16)));
            //16 top right jungle
            _tiles.Add(new Tile(TileType.Jungle, true, false, false, true, new Rectangle(368, 192, 16, 16)));
        }
    }
}
