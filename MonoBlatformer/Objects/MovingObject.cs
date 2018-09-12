using Microsoft.Xna.Framework;
using MonoBlatformer.MapThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBlatformer.Objects
{
    public struct AABB //axis-aligned bounding box
    {
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public AABB(Vector2 position, int width, int height)
        {
            Position = position;
            Width = width;
            Height = height;
        }
    }

    public class MovingObject
    {
        private AABB _AABB;
        private Map _map;
        private Vector2 _speed;
        private Vector2 _oldAABBPosition;
        private bool _isOnGround;
        private bool _hasLeftWall;
        private bool _hasRightWall;
        private bool _isAtCeiling;

        public virtual void Initialize(Vector2 position, int width, int height, Map map)
        {
            _AABB = new AABB(position, width, height);
            _map = map;
            _speed = Vector2.Zero;
        }

        public void UpdateGroundCollision()
        {
            float leftX = _AABB.Position.X + 1;
            float rightX = _AABB.Position.X + _AABB.Width - 1;
            float bottomY = _AABB.Position.Y + _AABB.Height + 1;
            
            if (_speed.Y >= 0)
            {
                Vector2 mapTileCoords;
                Tile tile;
                for (var checkedTile = leftX; ; checkedTile += _map.TileWidth)
                {
                    checkedTile = Math.Min(checkedTile, rightX);
                    mapTileCoords = _map.GetTileFromCoordinates(checkedTile, bottomY);
                    tile = _map.GetTile(mapTileCoords);
                    if (tile.IsGround)
                    {
                        _isOnGround = true;
                        Vector2 worldTileCoords = _map.GetCoordinatesFromTile((int)mapTileCoords.X, (int)mapTileCoords.Y);
                        _AABB.Position = new Vector2(_AABB.Position.X, worldTileCoords.Y - _AABB.Height);
                        break;
                    }
                    else
                    {
                        _isOnGround = false;
                    }
                    if (checkedTile >= rightX)
                        break;
                }
            }
        }
    }
}
