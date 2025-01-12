using Godot;
using System;
using Types;

namespace Player
{
    public partial class InAir : State
    {
        [Export]
        protected State Idle;

        protected MoveSpec moveSpec = null;

        public override void Enter()
        {
            base.Enter();
            this.moveSpec = null;
        }

        public override void PhysicsUpdate(float delta)
        {
            var VelocityVector = Character.Velocity;
            VelocityVector.Y += this.controller.Gravity * delta;

            if (moveSpec != null)
            {
                VelocityVector.X = moveSpec.Direction.X * this.controller.InAirProjectionSpeed;
            }
            Character.Velocity = VelocityVector;
            Character.MoveAndSlide();
        }

        public override void ProcessUpdate(float delta)
        {
            this.moveSpec = this.controller.WantToMove();

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

            }
        }
    }
}