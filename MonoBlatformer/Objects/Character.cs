using MonoBlatformer.Helpers;
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
    }
}
