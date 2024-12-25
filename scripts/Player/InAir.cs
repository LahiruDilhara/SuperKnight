using Godot;
using System;

namespace Player
{
    public partial class InAir : State
    {

        [Export]
        protected State Idle;
        public override void PhysicsUpdate(float delta)
        {
            var projectionY = this.Gravity * delta;
            Character.Velocity += new Vector2(y: projectionY, x: 0);
            Character.MoveAndSlide();
        }

        public override void ProcessUpdate(float delta)
        {
            var VelocityVector = Character.Velocity;

            var direction = inputHandler.GetMovementDirection();

            if (direction == -1)
            {
                this.Animation.FlipH = true;
            }
            else if (direction == 1)
            {
                this.Animation.FlipH = false;
            }

            VelocityVector.X = direction * this.JumpProjectionSpeed;

            Character.Velocity = VelocityVector;
            Character.MoveAndSlide();
        }
    }
}