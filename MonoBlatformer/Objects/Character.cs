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

        public void UpdatePlayer()
        {
            _input.Update();
            if (_curState == PlayerState.Stand)
            {
                if (_isOnGround == true)
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
                        _speed.Y -= _jumpSpeed;
                        _curAnimation = _flyAnimation;
                    }
                }
                else
                {
                    _curState = PlayerState.InAir;
                    _curAnimation = _flyAnimation;
                }
            }

            if (_curState == PlayerState.InAir)
            {
                if (_isOnGround)
                {
                    _curState = PlayerState.Stand;
                    _speed.Y = 0;
                    _curAnimation = _idleAnimation;
                }
                else
                {
                    _speed.Y += Constants.Gravity;
                }
            }
        }

        public void UpdatePhysics()
        {
            _AABB.Position += _speed;
            _curAnimation.Position = new Vector2(_AABB.Position.X + (_AABB.Width - _curAnimation.FrameWidth) / 2, _AABB.Position.Y + (_AABB.Height - _curAnimation.FrameHeight));
        }

        public void Update(GameTime gameTime)
        {
            UpdatePlayer();
            UpdatePhysics();
            UpdateGroundCollision();
            _curAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _curAnimation.Draw(spriteBatch, _fx);
        }
    }
}
