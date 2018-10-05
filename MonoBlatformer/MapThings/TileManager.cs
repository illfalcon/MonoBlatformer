using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBlatformer.Helpers;
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

        public List<Tile> Tiles { get { return _tiles; } }

        public TileManager()
        {
            _tiles = new List<Tile>();
            JSONSerializer json = new JSONSerializer();
            _tiles = json.RestoreList<Tile>("Levels/Level1/tiles.json");
        }
    }
}
