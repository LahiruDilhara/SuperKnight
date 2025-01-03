using Godot;
using System;

namespace Player
{
    public partial class InAir : State
    {

        [Export]
        public int Gravity = 980;

        [Export]
        public int InAirProjectionSpeed = 150;

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

            var direction = inputHandler.GetMovementDirection();

            if (direction == -1)
            {
                this.Animation.FlipH = true;
            }
            else if (direction == 1)
            {
                this.Animation.FlipH = false;
            }

            VelocityVector.X = direction * this.controller.InAirProjectionSpeed;

            Character.Velocity = VelocityVector;
            Character.MoveAndSlide();
        }
    }
}