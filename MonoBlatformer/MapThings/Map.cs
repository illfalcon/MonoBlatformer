using Microsoft.Xna.Framework;
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
        public Tile[,] Tiles { get { return _tiles; } }
        public Texture2D TileSet { get { return _tileSet; } }

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
            for (int i = 11; i < Width; i++)
            {
                _tiles[i, Height - 3] = _tileManager.Tiles[11];
            }
            _tiles[11, Height - 4] = _tileManager.Tiles[14];
            for (int i = 12; i < Width; i++)
            {
                _tiles[i, Height - 4] = _tileManager.Tiles[15];
            }
            _tiles[10, Height - 2] = _tileManager.Tiles[12];
            _tiles[10, Height - 3] = _tileManager.Tiles[13];
            _tiles[10, Height - 4] = _tileManager.Tiles[13];
            _tiles[10, Height - 5] = _tileManager.Tiles[13];
        }

        public Vector2 GetTileFromCoordinates(float wX, float wY)
        {
            int mX = (int)(wX/_tileWidth);
            int mY = (int)(wY/_tileHeight);
            return new Vector2(mX, mY);
        }

        public Vector2 GetCoordinatesFromTile(int mX, int mY)
        {
            float wX = mX * _tileWidth;
            float wY = mY * _tileHeight;
            return new Vector2(wX, wY);
        }

        public Tile GetTile(int x, int y)
        {
            return Tiles[x, y];
        }

        public Tile GetTile(Vector2 coords)
        {
            return Tiles[(int)coords.X, (int)coords.Y];
        }
    }
}
