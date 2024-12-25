using Godot;
using System;

namespace Player
{
	public partial class Jump : InAir
	{
		[Export]
		private State Fall;

		[Export]
		public String AnimationName = "jump";

		public override void Enter()
		{
			// set jump animation
			Animation?.Play(AnimationName);

			// set jump velocity
			Character.Velocity += new Vector2(x: 0, y: this.JumpHeight);
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
			else if (Character.Velocity.Y < 0f)
			{
				ChangeState(Fall);
				return;
			}

		}
	}

}
