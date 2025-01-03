using Godot;
using System;
using Types;

namespace Player
{
	public partial class Run : OnGround
	{

		[Export]
		public String AnimationName = "run";

		[Export]
		private State Idel;
		public override void Enter()
		{
			Animation?.Play(AnimationName);
		}
		public override void ProcessUpdate(float delta)
		{
			MoveSpec moveSpec = controller.WantToMove();

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
			else if (moveSpec == null)
			{
				ChangeState(Idel);
				return;
			}
			var VelocityVector = Character.Velocity;

			if (moveSpec.Direction.X == -1)
			{
				Animation.FlipH = true;
			}
			else if (moveSpec.Direction.X == 1)
			{
				Animation.FlipH = false;
			}

			VelocityVector.X = moveSpec.Direction.X * moveSpec.Speed;
			Character.Velocity = VelocityVector;
			Character.MoveAndSlide();
		}
	}
}
