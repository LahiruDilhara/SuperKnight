using Godot;
using System;

namespace Player
{
	public partial class Jump : InAir
	{

		[Export]
		public String AnimationName = "jump";

		private int _JumpVelocity = -400;

		[Export]
		public int JumpVelocity
		{
			get
			{
				return _JumpVelocity;
			}
			set
			{
				if (value > 0)
				{
					this._JumpVelocity = -1 * value;
				}
				else
				{
					this._JumpVelocity = value;
				}
			}
		}

		[Export]
		private State Fall;

		public override void Enter()
		{
			GD.Print("Jump State");
			// set jump animation
			Animation?.Play(AnimationName);

			// set jump velocity
			Character.Velocity += new Vector2(x: 0, y: this.JumpVelocity);
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
