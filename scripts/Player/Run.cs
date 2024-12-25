using Godot;
using System;

namespace Player
{
	public partial class Run : OnGround
	{

		[Export]
		private State Idel;

		[Export]
		private State Jump;

		[Export]
		public String AnimationName = "run";
		public override void Enter()
		{
			base.Enter();

			Animation?.Play(AnimationName);
		}
		public override void ProcessUpdate(float delta)
		{
			var VelocityVector = Character.Velocity;

			if(inputHandler.WantToJump()){
				ChangeState(Jump);
				return;
			}

			var direction = inputHandler.GetMovementDirection();
			if (direction == 0f)
			{
				ChangeState(Idel);
				return;
			}

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

		public override void Exit()
		{
			base.Exit();
		}
	}
}
