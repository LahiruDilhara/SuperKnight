using Godot;
using System;

namespace Player
{
	public partial class Idle : State
	{
		[Export]
		private State Run;

		[Export]
		private State Jump;

		[Export]
		public String AnimationName = "idle";
		public override void Enter()
		{
			base.Enter();
			
			// set idel animation
			Animation?.Play(AnimationName);

			this.Character.Velocity = new Vector2(x: 0, y: 0);
		}
		public override void ProcessUpdate(float delta)
		{
			base.ProcessUpdate(delta);
			if (inputHandler.GetMovementDirection() != 0f)
			{
				ChangeState(Run);
				return;
			}
			if(inputHandler.WantToJump()){
				ChangeState(Jump);
				return;
			}
			Character.MoveAndSlide();
		}

		public override void Exit()
		{
			base.Exit();
		}
	}

}
