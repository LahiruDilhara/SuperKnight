using Godot;
using Types;

namespace Controllers
{
    public abstract partial class IController : Node
    {
        public abstract MoveSpec WantToMove();
        public abstract JumpSpec WantToJump();
    }
}