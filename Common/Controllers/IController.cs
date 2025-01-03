using Godot;
using Types;

namespace Controllers
{
    public abstract partial class IController : Node
    {
        [Export]
        public int Gravity = 500;

        [Export]
        public int InAirProjectionSpeed = 80;

        public abstract MoveSpec WantToMove();
        public abstract JumpSpec WantToJump();
    }
}