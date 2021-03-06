﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBlatformer.Helpers;
using MonoBlatformer.MapThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBlatformer.Objects
{
    public enum PlayerState { Stand, Run, InAir, OnLedge }

    public class Character : MovingObject
    {
        private Animation _runAnimation;
        private Animation _flyAnimation;
        private Animation _jumpAnimation;
        private Animation _landAnimation;
        private Animation _idleAnimation;
        private Animation _ledgeAnimation;
        private Animation _curAnimation;
        private float _runSpeed;
        private float _jumpSpeed;
        private Input _input;
        private PlayerState _curState;
        private SpriteEffects _fx;

        private bool _climbingLeftFlag;
        private bool _climbingRightFlag;
        private bool _droppingFlag;


        private float _climbingGroundY;
        private float _climbingGroundX;

        public void Initialize(Vector2 position, int width, int height, Map map, Animation idle, Animation run, Animation fly,
            Animation jump, Animation land, Animation ledge, float runSpeed, float jumpSpeed)
        {
            base.Initialize(position, width, height, map);
            _idleAnimation = idle;
            _runAnimation = run;
            _flyAnimation = fly;
            _jumpAnimation = jump;
            _landAnimation = land;
            _ledgeAnimation = ledge;
            _runSpeed = runSpeed;
            _jumpSpeed = jumpSpeed;
            _input = new Input();
            _curAnimation = _idleAnimation;
        }

        public bool CheckLeftLedge(out float ledgeX, out float ledgeY)
        {
            ledgeX = 0;
            ledgeY = 0;
            if (_hasLeftWall && _speed.Y >= 0 && !_hasCeiling)
            {
                float leftX = _AABB.Position.X - 1;
                float topY = _AABB.Position.Y + 3;
                float bottomY = topY + 3;

                for (float checkedTile = topY; ; checkedTile++) //so it's better to return ground y
                {
                    int x = (int)_map.GetTileFromCoordinates(leftX, checkedTile).X;
                    int y = (int)_map.GetTileFromCoordinates(leftX, checkedTile).Y;
                    if (_map.GetTile(x, y).IsGround && !_map.GetTile(x, y - 1).IsGround)
                    {
                        ledgeX = _map.GetCoordinatesFromTile((int)_map.GetTileFromCoordinates(leftX, checkedTile).X, (int)_map.GetTileFromCoordinates(leftX, checkedTile).Y).X + _map.TileWidth;
                        ledgeY = _map.GetCoordinatesFromTile((int)_map.GetTileFromCoordinates(leftX, checkedTile).X + _map.TileWidth, (int)_map.GetTileFromCoordinates(leftX, checkedTile).Y).Y;
                        return true;
                    }
                    if (checkedTile >= bottomY)
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool CheckRightLedge(out float ledgeX, out float ledgeY)
        {
            ledgeX = 0;
            ledgeY = 0;
            if (_hasRightWall && _speed.Y >= 0 && !_hasCeiling)
            {
                float rightX = _AABB.Position.X + _AABB.Width + 1;
                float topY = _AABB.Position.Y + 3;
                float bottomY = topY + 3;

                for (float checkedTile = topY; ; checkedTile++) //so it's better to return ground y
                {
                    int x = (int)_map.GetTileFromCoordinates(rightX, checkedTile).X;
                    int y = (int)_map.GetTileFromCoordinates(rightX, checkedTile).Y;
                    if (_map.GetTile(x, y).IsGround && !_map.GetTile(x, y - 1).IsGround)
                    {
                        ledgeX = _map.GetCoordinatesFromTile((int)_map.GetTileFromCoordinates(rightX, checkedTile).X, (int)_map.GetTileFromCoordinates(rightX, checkedTile).Y).X;
                        ledgeY = _map.GetCoordinatesFromTile((int)_map.GetTileFromCoordinates(rightX, checkedTile).X, (int)_map.GetTileFromCoordinates(rightX, checkedTile).Y).Y;
                        return true;
                    }
                    if (checkedTile >= bottomY)
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void UpdatePlayer()
        {
            _input.Update();
            if (_curState == PlayerState.Stand)
            {
                _curAnimation = _idleAnimation;
                if (_isOnGround)
                {
                    if (_input.LeftDown != _input.RightDown)
                    {
                        _curState = PlayerState.Run;
                        _curAnimation = _runAnimation;
                    } else
                    {
                        _speed = Vector2.Zero;
                    }
                    if (_input.UpPressed)
                    {
                        _curState = PlayerState.InAir;
                        _speed.Y = -_jumpSpeed;
                        _isOnGround = false;
                        _curAnimation = _flyAnimation;
                    } else if (_input.DownPressed && CanFall())
                    {
                        _ignoringOneWays = true;
                        _isOnGround = false;
                        _curAnimation = _flyAnimation;
                        _curState = PlayerState.InAir;
                    }
                }
                else
                {
                    _curState = PlayerState.InAir;
                    _curAnimation = _flyAnimation;
                }
            }

            if (_curState == PlayerState.Run)
            {
                _curAnimation = _runAnimation;
                if (!_isOnGround)
                {
                    _curState = PlayerState.InAir;
                    _curAnimation = _flyAnimation;
                }
                else
                {
                    if (_input.LeftDown == _input.RightDown)
                    {
                        _speed.X = 0;
                        _curState = PlayerState.Stand;
                        _curAnimation = _idleAnimation;
                    }
                    else if (_input.LeftDown)
                    {
                        _fx = SpriteEffects.FlipHorizontally;
                        if (_hasLeftWall)
                        {
                            _speed.X = 0;
                            _curAnimation = _idleAnimation;
                        }
                        else
                        {
                            _speed.X = -_runSpeed;
                        }
                    }
                    else if (_input.RightDown)
                    {
                        _fx = SpriteEffects.None;
                        if (_hasRightWall)
                        {
                            _speed.X = 0;
                            _curAnimation = _idleAnimation;
                        }
                        else
                        {
                            _speed.X = _runSpeed;
                        }
                    }
                    if (_input.UpPressed)
                    {
                        _curState = PlayerState.InAir;
                        _speed.Y = -_jumpSpeed;
                        _isOnGround = false;
                        _curAnimation = _flyAnimation;
                    }
                    else if (_input.DownPressed && CanFall())
                    {
                        _ignoringOneWays = true;
                        _isOnGround = false;
                        _curAnimation = _flyAnimation;
                        _curState = PlayerState.InAir;
                    }
                }
            }

            if (_curState == PlayerState.InAir)
            {
                float ledgeX, ledgeY;
                _curAnimation = _flyAnimation;
                _ledgeAnimation.Reset();
                if (_isOnGround)
                {
                    _droppingFlag = false;
                    _curState = PlayerState.Stand;
                    _speed.Y = 0;
                    _curAnimation = _idleAnimation;
                }
                else if (!_droppingFlag && CheckLeftLedge(out ledgeX, out ledgeY))
                {
                    _curState = PlayerState.OnLedge;
                    _ledgeAnimation.Active = true;
                    _curAnimation = _ledgeAnimation;
                    _fx = SpriteEffects.FlipHorizontally;
                    _speed = Vector2.Zero;
                    _AABB.Position = new Vector2(_AABB.Position.X, ledgeY);
                    _climbingGroundY = ledgeY;
                    _climbingGroundX = ledgeX;
                }
                else if (!_droppingFlag && CheckRightLedge(out ledgeX, out ledgeY))
                {
                    _curState = PlayerState.OnLedge;
                    _ledgeAnimation.Active = true;
                    _curAnimation = _ledgeAnimation;
                    _fx = SpriteEffects.None;
                    _speed = Vector2.Zero;
                    _AABB.Position = new Vector2(_AABB.Position.X, ledgeY);
                    _climbingGroundY = ledgeY;
                    _climbingGroundX = ledgeX;
                }
                else
                {
                    _speed.Y += Constants.Gravity;
                }
            }
            if (_curState == PlayerState.OnLedge)
            {
                if (_hasRightWall && _input.UpPressed && !_climbingRightFlag)
                {
                    _climbingRightFlag = true;
                    _curAnimation = _flyAnimation;
                    ClimbRight(_climbingGroundX, _climbingGroundY);
                } else if (_hasLeftWall && _input.UpPressed && !_climbingLeftFlag)
                {
                    _climbingLeftFlag = true;
                    _curAnimation = _flyAnimation;
                    ClimbLeft(_climbingGroundX, _climbingGroundY);
                }
                else if (_input.DownPressed)
                {
                    _droppingFlag = true;
                    _curAnimation = _flyAnimation;
                    _curState = PlayerState.InAir;
                }
                if (_climbingRightFlag)
                    ClimbRight(_climbingGroundX, _climbingGroundY);
                if (_climbingLeftFlag)
                    ClimbLeft(_climbingGroundX, _climbingGroundY);
            }

        }

        public void UpdateCamera()
        {
            Camera.Position = new Vector2((int)(_AABB.Position.X - Camera.Width / 2), (int)_AABB.Position.Y - Camera.Height / 2);
            if (Camera.Position.X < Camera.MinX)
            {
                Camera.Position.X = Camera.MinX;
            }
            if (Camera.Position.X > Camera.MaxX)
            {
                Camera.Position.X = Camera.MaxX;
            }
            if (Camera.Position.Y < Camera.MinY)
            {
                Camera.Position.Y = Camera.MinY;
            }
            if (Camera.Position.Y > Camera.MaxY)
            {
                Camera.Position.Y = Camera.MaxY;
            }
        }

        public void ClimbRight(float groundX, float groundY)
        {
            if (_AABB.Position.Y + _AABB.Height >= groundY)
            {
                _AABB.Position = new Vector2(_AABB.Position.X, _AABB.Position.Y - _runSpeed);
            } else if (_AABB.Position.X <= groundX)
            {
                _AABB.Position = new Vector2(_AABB.Position.X + _runSpeed, _AABB.Position.Y);
            }
            else
            {
                _curState = PlayerState.InAir;
                _climbingRightFlag = false;
            }
        }

        public void ClimbLeft(float groundX, float groundY)
        {
            if (_AABB.Position.Y + _AABB.Height >= groundY)
            {
                _AABB.Position = new Vector2(_AABB.Position.X, _AABB.Position.Y - _runSpeed);
            }
            else if (_AABB.Position.X + _AABB.Width >= groundX)
            {
                _AABB.Position = new Vector2(_AABB.Position.X - _runSpeed, _AABB.Position.Y);
            }
            else
            {
                _curState = PlayerState.InAir;
                _climbingLeftFlag = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            _oldAABBPosition = _AABB.Position;
            UpdatePlayer();
            UpdatePhysics();
            _curAnimation.Position = new Vector2(_AABB.Position.X + (_AABB.Width - _curAnimation.FrameWidth) / 2, _AABB.Position.Y + (_AABB.Height - _curAnimation.FrameHeight)) - Camera.Position;
            _curAnimation.Update(gameTime);
            UpdateCamera();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _curAnimation.Draw(spriteBatch, _fx);
        }
    }
}
