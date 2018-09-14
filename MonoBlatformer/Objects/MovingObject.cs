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

        public virtual void Initialize(Vector2 position, int width, int height, Map map)
        {
            _AABB = new AABB(position, width, height);
            _map = map;
            _speed = Vector2.Zero;
        }

        public bool HasGround(out float groundY)
        {
            Vector2 oldBottomLeft = new Vector2(_oldAABBPosition.X, _oldAABBPosition.Y + _AABB.Height + 1);
            Vector2 newBottomLeft = new Vector2(_AABB.Position.X, _AABB.Position.Y + _AABB.Height + 1);

            int endY = (int)_map.GetTileFromCoordinates(newBottomLeft.X, newBottomLeft.Y).Y;
            int begY = Math.Min(endY, (int)_map.GetTileFromCoordinates(oldBottomLeft.X, oldBottomLeft.Y).Y);
            float dist = Math.Max(endY - begY, 1);

            for (int tileY = begY; tileY <= endY; tileY++)
            {
                var bottomLeft = Vector2.Lerp(oldBottomLeft, newBottomLeft, (tileY - begY) / dist);
                var bottomRight = bottomLeft + new Vector2(_AABB.Width, 0);

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

        public bool HasCeiling(out float ceilingY)
        {
            Vector2 oldTopLeft = new Vector2(_oldAABBPosition.X, _oldAABBPosition.Y - 1);
            Vector2 newTopLeft = new Vector2(_AABB.Position.X, _AABB.Position.Y - 1);

            int endY = (int)_map.GetTileFromCoordinates(newTopLeft.X, newTopLeft.Y).Y;
            int begY = Math.Max(endY, (int)_map.GetTileFromCoordinates(oldTopLeft.X, oldTopLeft.Y).Y);
            float dist = Math.Max(begY - endY, 1);

            for (int tileY = begY; tileY >= endY; tileY--)
            {
                var topLeft = Vector2.Lerp(oldTopLeft, newTopLeft, (tileY - endY) / dist);
                var topRight = topLeft + new Vector2(_AABB.Width, 0);

                Vector2 mapTileCoords;
                Tile tile;
                for (var checkedTile = topLeft.X; ; checkedTile += _map.TileWidth)
                {
                    checkedTile = Math.Min(checkedTile, topRight.X);
                    mapTileCoords = _map.GetTileFromCoordinates(checkedTile, topLeft.Y);
                    tile = _map.GetTile(mapTileCoords);
                    if (tile.IsSolid)
                    {
                        ceilingY = _map.GetCoordinatesFromTile((int)mapTileCoords.X, (int)mapTileCoords.Y).Y;
                        return true;
                    }
                    if (checkedTile >= topRight.X)
                        break;
                }
            }

            ceilingY = 0;
            return false;
        }

        public bool HasLeftWall(out float leftWallX)
        {
            Vector2 oldTopLeft = new Vector2(_oldAABBPosition.X - 1, _oldAABBPosition.Y);
            Vector2 newTopLeft = new Vector2(_AABB.Position.X - 1, _AABB.Position.Y);

            int endX = (int)_map.GetTileFromCoordinates(newTopLeft.X, newTopLeft.Y).X;
            int begX = Math.Max(endX, (int)_map.GetTileFromCoordinates(oldTopLeft.X, oldTopLeft.Y).X);
            float dist = Math.Max(begX - endX, 1);

            for (int tileX = begX; tileX >= endX; tileX--)
            {
                var topLeft = Vector2.Lerp(oldTopLeft, newTopLeft, (tileX - endX) / dist);
                var bottomLeft = topLeft + new Vector2(0, _AABB.Height);

                Vector2 mapTileCoords;
                Tile tile;
                for (var checkedTile = topLeft.Y; ; checkedTile += _map.TileHeight)
                {
                    checkedTile = Math.Min(checkedTile, bottomLeft.Y);
                    mapTileCoords = _map.GetTileFromCoordinates(topLeft.X, checkedTile);
                    tile = _map.GetTile(mapTileCoords);
                    if (tile.IsSolid)
                    {
                        leftWallX = _map.GetCoordinatesFromTile((int)mapTileCoords.X, (int)mapTileCoords.Y).Y;
                        return true;
                    }
                    if (checkedTile >= bottomLeft.X)
                        break;
                }
            }

            leftWallX = 0;
            return false;
        }

        public bool HasRightWall(out float rightWallX)
        {
            Vector2 oldTopRight = new Vector2(_oldAABBPosition.X + _AABB.Width + 1, _oldAABBPosition.Y);
            Vector2 newTopRight = new Vector2(_AABB.Position.X + _AABB.Width + 1, _AABB.Position.Y);

            int endX = (int)_map.GetTileFromCoordinates(newTopRight.X, newTopRight.Y).X;
            int begX = Math.Min(endX, (int)_map.GetTileFromCoordinates(oldTopRight.X, oldTopRight.Y).X);
            float dist = Math.Max(endX - begX, 1);

            for (int tileX = begX; tileX <= endX; tileX++)
            {
                var topRight = Vector2.Lerp(oldTopRight, newTopRight, (tileX - begX) / dist);
                var bottomRight = topRight + new Vector2(0, _AABB.Height);

                Vector2 mapTileCoords;
                Tile tile;
                for (var checkedTile = topRight.Y; ; checkedTile += _map.TileHeight)
                {
                    checkedTile = Math.Min(checkedTile, bottomRight.Y);
                    mapTileCoords = _map.GetTileFromCoordinates(topRight.X, checkedTile);
                    tile = _map.GetTile(mapTileCoords);
                    if (tile.IsSolid)
                    {
                        rightWallX = _map.GetCoordinatesFromTile((int)mapTileCoords.X, (int)mapTileCoords.Y).Y;
                        return true;
                    }
                    if (checkedTile >= bottomRight.X)
                        break;
                }
            }

            rightWallX = 0;
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

        public void UpdateCeilingCollision()
        {
            float ceilingY = 0;
            if (_speed.Y < 0 && _AABB.Position.Y < _oldAABBPosition.Y && HasCeiling(out ceilingY))
            {
                _speed.Y = 0;
                _AABB.Position = new Vector2(_AABB.Position.X, ceilingY);
            }
        }

        public void UpdateLeftWallCollision()
        {
            float leftWallX;
            if (HasLeftWall(out leftWallX))
            {
                _speed.X = 0;
                _AABB.Position = new Vector2(leftWallX, _AABB.Position.Y);
                _hasLeftWall = true;
            }
            else
            {
                _hasLeftWall = false;
            }
        }

        public void UpdateRightWallCollision()
        {
            float rightWallX;
            if (HasRightWall(out rightWallX))
            {
                _speed.X = 0;
                _AABB.Position = new Vector2(rightWallX - _AABB.Width, _AABB.Position.Y);
                _hasRightWall = true;
            }
            else
            {
                _hasRightWall = false;
            }
        }
    }
}
