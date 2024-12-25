using Godot;
using System;

namespace Player
{
    public partial class OnGround : State
    {
        [Export]
        protected State Fall;

        [Export]
        protected State Jump;
        public override void PhysicsUpdate(float delta)
        {
            Character.Velocity = new Vector2(Character.Velocity.X, 0);
            Character.MoveAndSlide();
        }
    }
}