using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBlatformer.Helpers
{
    public class Animation
    {
        private Texture2D _spriteStrip;
        private float _scale;
        private int _elapsedTime;
        private int _frameTime;
        private int _frameCount;
        private int _currentFrame;
        private Color _color;
        private Rectangle _sourceRect;
        private Rectangle _destinationRect;

        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public bool Active { get; set; }
        public bool Looping { get; set; }
        public Vector2 Position { get; set; }

        public void Initialize(Texture2D spriteStrip, float scale, int frameTime, int frameCount,
            Color color, int frameWidth, int frameHeight, bool looping)
        {
            _spriteStrip = spriteStrip;
            _scale = scale;
            _frameTime = frameTime;
            _frameCount = frameCount;
            _color = color;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            Active = true;
            Looping = looping;
            Position = Vector2.Zero;
            _elapsedTime = 0;
            _currentFrame = 0;
        }

        public void Reset()
        {
            _currentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (Active == false)
            {
                _currentFrame = 0;
                return;
            }

            // Update the elapsed time
            _elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (_elapsedTime > _frameTime)
            {
                // Move to the next frame
                _currentFrame++;

                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (_currentFrame == _frameCount)
                {
                    _currentFrame = 0;
                    // If we are not looping deactivate the animation
                    if (Looping == false)
                    {
                        //Active = false;
                        _currentFrame = _frameCount - 1;
                        Active = false;
                    }
                }

                // Reset the elapsed time to zero
                _elapsedTime = 0;
            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            _sourceRect = new Rectangle(_currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            _destinationRect = new Rectangle((int)Position.X,
                                            (int)Position.Y,
                                            (int)(FrameWidth * _scale),
                                            (int)(FrameHeight * _scale));
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects fx)
        {
            spriteBatch.Draw(_spriteStrip, _destinationRect, _sourceRect, _color, 0, Vector2.Zero, fx, 0);
        }
    }
}
