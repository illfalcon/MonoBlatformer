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
        protected AABB _AABB;
        protected Map _map;
        protected Vector2 _speed;
        protected Vector2 _oldAABBPosition;
        protected bool _isOnGround;
        protected bool _hasLeftWall;
        protected bool _hasRightWall;
        protected bool _isAtCeiling;

        public virtual void Initialize(Vector2 position, int width, int height, Map map)
        {
            _AABB = new AABB(position, width, height);
            _map = map;
            _speed = Vector2.Zero;
        }

        public bool HasGround(out float groundY)
        {
            Vector2 oldBottomLeft = new Vector2(_oldAABBPosition.X + 1, _oldAABBPosition.Y + _AABB.Height + 1);
            Vector2 newBottomLeft = new Vector2(_AABB.Position.X + 1, _AABB.Position.Y + _AABB.Height + 1);

            int endY = (int)_map.GetTileFromCoordinates(newBottomLeft.X, newBottomLeft.Y).Y;
            int begY = Math.Min(endY, (int)_map.GetTileFromCoordinates(oldBottomLeft.X, oldBottomLeft.Y).Y);
            float dist = Math.Max(endY - begY, 1);

            for (int tileY = begY; tileY <= endY; tileY++)
            {
                var bottomLeft = Vector2.Lerp(oldBottomLeft, newBottomLeft, (tileY - begY) / dist);
                var bottomRight = bottomLeft + new Vector2(_AABB.Width - 2, 0);

                Vector2 mapTileCoords;
                Tile tile;
                for (var checkedTile = bottomLeft.X; ; checkedTile += _map.TileWidth)
                {
                    checkedTile = Math.Min(checkedTile, bottomRight.X);
                    mapTileCoords = _map.GetTileFromCoordinates(checkedTile, bottomLeft.Y);
                    tile = _map.GetTile(mapTileCoords);
                    if (tile.IsGround)
                    {
                        groundY = _map.GetCoordinatesFromTile((int)mapTileCoords.X, (int)mapTileCoords.Y).Y;
                        return true;
                    }
                    if (checkedTile >= bottomRight.X)
                        break;
                }
            }

            groundY = 0;
            return false;
        }

        public void UpdateGroundCollision()
        {
            float groundY = 0;
            if (_speed.Y >= 0 && _AABB.Position.Y >= _oldAABBPosition.Y && HasGround(out groundY))
            {
                _speed.Y = 0;
                _AABB.Position = new Vector2(_AABB.Position.X, groundY - _AABB.Height);
                _isOnGround = true;
            }
            else
            {
                _isOnGround = false;
            }
        }
    }
}
