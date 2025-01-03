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

		private MoveSpec moveSpec;
		public override void Enter()
		{
			Animation?.Play(AnimationName);
		}

		public override void PhysicsUpdate(float delta)
		{
			if (this.moveSpec == null) return;
			var VelocityVector = Character.Velocity;

			VelocityVector.X = moveSpec.Direction.X * moveSpec.Speed;
			Character.Velocity = VelocityVector;
			Character.MoveAndSlide();
		}
		public override void ProcessUpdate(float delta)
		{
			this.moveSpec = controller.WantToMove();

			if (controller.WantToJump() != null)
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

			if (moveSpec.Direction.X == -1)
			{
				Animation.FlipH = true;
			}
			else if (moveSpec.Direction.X == 1)
			{
				Animation.FlipH = false;
			}
		}
	}
}
