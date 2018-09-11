using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBlatformer.MapThings
{
    public class Map
    {
        private int _width;
        private int _height;
        private int _tileWidth;
        private int _tileHeight;
        private Texture2D _tileSet;
        private TileManager _tileManager;
        private Tile[,] _tiles;

        public int Width { get { return _width; } } //in tiles
        public int Height { get { return _height; } } // in tiles
        public int TileWidth { get { return _tileWidth; } }
        public int TileHeight { get { return _tileHeight; } }
        //public TileManager TileManager { get { return _tileManager; } }
        public Tile[,] Tiles { get { return _tiles; } }

        public void Initialize(int width, int height, int tileWidth, int tileHeight, Texture2D tileSet)
        {
            _width = width;
            _height = height;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            _tileSet = tileSet;
            _tileManager = new TileManager();
            CreateMap();
        }

        //more hardcoding
        public void CreateMap()
        {
            _tiles = new Tile[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _tiles[i, j] = _tileManager.Tiles[0];
                }
            }
            for (int i = 0; i < Width; i++)
            {
                int j = i % 10 + 1;
                _tiles[i, Height - 2] = _tileManager.Tiles[j];
            }
            for (int i = 0; i < Width; i++)
            {
                _tiles[i, Height - 1] = _tileManager.Tiles[11];
            }
        }
    }
}
