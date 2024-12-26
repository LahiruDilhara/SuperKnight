using Godot;
using System;

namespace Player
{
	public partial class Run : OnGround
	{

		[Export]
		public String AnimationName = "run";

		[Export]
		public int RunSpeed = 80;

		[Export]
		private State Idel;
		public override void Enter()
		{
			GD.Print("Run State");
			Animation?.Play(AnimationName);
		}
		public override void ProcessUpdate(float delta)
		{
			if (inputHandler.WantToJump())
			{
				ChangeState(Jump);
				return;
			}
			else if (!Character.IsOnFloor())
			{
				ChangeState(Fall);
				return;
			}
			else if (inputHandler.GetMovementDirection() == 0f)
			{
				ChangeState(Idel);
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
