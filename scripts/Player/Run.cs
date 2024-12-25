using Godot;
using System;

namespace Player
{
	public partial class Run : OnGround
	{

		[Export]
		private State Idel;

		[Export]
		public String AnimationName = "run";
		public override void Enter()
		{
			Animation?.Play(AnimationName);
		}
		public override void ProcessUpdate(float delta)
		{
			if (inputHandler.GetMovementDirection() == 0f)
			{
				ChangeState(Idel);
				return;
			}
			else if (inputHandler.WantToJump())
			{
				ChangeState(Jump);
				return;
			}
			else if (!Character.IsOnFloor())
			{
				ChangeState(Fall);
				return;
			}
			var VelocityVector = Character.Velocity;

			var direction = inputHandler.GetMovementDirection();

			if (direction == -1)
			{
				Animation.FlipH = true;
			}
			else if (direction == 1)
			{
				Animation.FlipH = false;
			}

			VelocityVector.X = direction * this.RunSpeed;
			Character.Velocity = VelocityVector;
			Character.MoveAndSlide();
		}
	}
}
