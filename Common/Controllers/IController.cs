using Godot;
using Types;

namespace Controllers
{
    public abstract partial class IController : Node
    {
        public int Gravity = 980;

        public int InAirProjectionSpeed = 150;
        public abstract MoveSpec WantToMove();
        public abstract JumpSpec WantToJump();
    }
}