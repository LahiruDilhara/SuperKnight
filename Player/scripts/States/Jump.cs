using Godot;
using System;
using Types;

namespace Player
{
	public partial class Jump : InAir
	{

		[Export]
		public String AnimationName = "jump";

		[Export]
		private State Fall;

		public override void Enter()
		{
			base.Enter();
			JumpSpec jumpSpec = controller.WantToJump();

			if (jumpSpec == null)
			{
				ChangeState(Idle);
				return;
			}
			// set jump animation
			Animation?.Play(AnimationName);

			// set jump velocity
			Character.Velocity += new Vector2(x: 0, y: jumpSpec.JumpVelocity);
			Character.MoveAndSlide();
		}
		public override void ProcessUpdate(float delta)
		{
			base.ProcessUpdate(delta);
			if (Character.IsOnFloor())
			{
				ChangeState(Idle);
				return;
			}
			else if (Character.Velocity.Y > 0f)
			{
				ChangeState(Fall);
				return;
			}
		}
	}

}
