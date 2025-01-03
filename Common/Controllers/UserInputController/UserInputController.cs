using Controllers;
using Globals;
using Godot;
using System;
using Types;

namespace Controllers
{
	public partial class UserInputController : IController
	{
		[Export]
		private int JumpVelocity = -180;

		[Export]
		private int RunSpeed = 60;

		public override JumpSpec WantToJump()
		{
			if (InputManager.Instance.IsActionPressed("jump"))
			{
				return new JumpSpec()
				{
					JumpVelocity = JumpVelocity
				};
			}
			return null;
		}

		public override MoveSpec WantToMove()
		{
			if (InputManager.Instance.IsActionPressed("left"))
			{
				return new MoveSpec
				{
					Direction = new Vector2(-1, 0),
					Speed = RunSpeed
				};
			}
			else if (InputManager.Instance.IsActionPressed("right"))
			{
				return new MoveSpec
				{
					Direction = new Vector2(1, 0),
					Speed = RunSpeed
				};
			};
			return null;
		}

	}

}
