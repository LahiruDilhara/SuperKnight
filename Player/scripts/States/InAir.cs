using Godot;
using System;
using Types;

namespace Player
{
    public partial class InAir : State
    {
        [Export]
        protected State Idle;

        public override void PhysicsUpdate(float delta)
        {
            var projectionY = this.controller.Gravity * delta;
            Character.Velocity += new Vector2(y: projectionY, x: 0);
            Character.MoveAndSlide();
        }

        public override void ProcessUpdate(float delta)
        {
            var VelocityVector = Character.Velocity;
            MoveSpec moveSpec = this.controller.WantToMove();

            if (moveSpec != null)
            {
                if (moveSpec.Direction.X == -1)
                {
                    this.Animation.FlipH = true;
                }
                else if (moveSpec.Direction.X == 1)
                {
                    this.Animation.FlipH = false;
                }

                VelocityVector.X = moveSpec.Direction.X * this.controller.InAirProjectionSpeed;

            }
            Character.Velocity = VelocityVector;
            Character.MoveAndSlide();
        }
    }
}