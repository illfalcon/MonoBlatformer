using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBlatformer.Helpers
{
    public class Input
    {
        private KeyboardState _keyboard;
        private KeyboardState _oldKeyboard;

        public bool LeftDown { get; set; }
        public bool RightDown { get; set; }
        public bool UpDown { get; set; }
        public bool DownDown { get; set; }
        public bool UpPressed { get; set; }
        public bool DownPressed { get; set; }
        public bool OldUpDown { get; set; }
        public bool OldDownDown { get; set; }

        public void Initialize()
        {
            _keyboard = new KeyboardState();
            _oldKeyboard = new KeyboardState();
        }

        public void Update()
        {
            OldUpDown = UpDown;
            OldDownDown = DownDown;
            LeftDown = RightDown = UpDown = DownDown = UpPressed = DownPressed = false;
            _keyboard = Keyboard.GetState();
            if (_keyboard.IsKeyDown(Keys.Left)) LeftDown = true;
            if (_keyboard.IsKeyDown(Keys.Right)) RightDown = true;
            if (_keyboard.IsKeyDown(Keys.Up)) UpDown = true;
            if (_keyboard.IsKeyDown(Keys.Down)) DownDown = true;
            if (UpDown && !OldUpDown) UpPressed = true;
            if (DownDown && !OldDownDown) DownPressed = true;
        }
    }
}
