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
            JSONSerializer json = new JSONSerializer();
            int[,] tileIds = json.RestoreTwoDimArray<int>("Levels/Level1/map.json");
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _tiles[i, j] = _tileManager.Tiles.SingleOrDefault(t => t.Id == tileIds[i, j]);
                }
            }
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

        public void Draw(SpriteBatch spriteBatch)
        {
            int startCol = (int)GetTileFromCoordinates(Camera.Position.X, Camera.Position.Y).X;
            int startRow = (int)GetTileFromCoordinates(Camera.Position.X, Camera.Position.Y).Y;
            int endCol = (int)GetTileFromCoordinates(Camera.Position.X + Camera.Width, Camera.Position.Y).X;
            int endRow = (int)GetTileFromCoordinates(Camera.Position.X, Camera.Position.Y + Camera.Height).Y;

            if (endCol != _width)
                endCol += 1;
            if (endRow != _height)
                endRow += 1;

            Vector2 offset = new Vector2(GetCoordinatesFromTile(startCol, startRow).X - Camera.Position.X, GetCoordinatesFromTile(startCol, startRow).Y - Camera.Position.Y);

            for (int i = startCol; i < endCol; i++)
            {
                for (int j = startRow; j < endRow; j++)
                {
                    spriteBatch.Draw(_tileSet, new Rectangle((int)_tileWidth * (i - startCol) + (int)offset.X, (int)_tileHeight * (j - startRow) + (int)offset.Y, _tileWidth, _tileHeight), _tiles[i, j].SourceRectangle, Color.White);
                }
            }
        }
    }
}
