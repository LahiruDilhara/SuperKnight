using Godot;
using System;

namespace Player
{
	public partial class Jump : InAir
	{
		[Export]
		private State Idel;

		[Export]
		public String AnimationName = "jump";

		public override void Enter()
		{
			base.Enter();

			// set jump animation
			Animation?.Play(AnimationName);

			// set jump velocity
			Character.Velocity += new Vector2(x: 0, y: this.JumpHeight);
			Character.MoveAndSlide();
		}
		public override void ProcessUpdate(float delta)
		{
			base.ProcessUpdate(delta);

			var VelocityVector = Character.Velocity;

			var direction = inputHandler.GetMovementDirection();

			// change to idel if the Character hit ground
			if (Character.IsOnFloor())
			{
				ChangeState(Idel);
				return;
			}

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

		public override void Exit()
		{
			base.Exit();
		}
	}

}
