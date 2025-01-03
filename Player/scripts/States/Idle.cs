using Godot;
using System;

namespace Player
{
	public partial class Idle : OnGround
	{
		[Export]
		public String AnimationName = "idle";

		[Export]
		private State Run;

		public override void Enter()
		{
			// set idel animation
			Animation?.Play(AnimationName);

			this.Character.Velocity = new Vector2(x: 0, y: 0);
		}
		public override void ProcessUpdate(float delta)
		{
			if (controller.WantToJump() != null)
			{
				ChangeState(Jump);
				return;
			}
			else if (controller.WantToMove() != null)
			{
				ChangeState(Run);
				return;
			}
			else if (!Character.IsOnFloor())
			{
				ChangeState(Fall);
				return;
			}
		}
	}

}
