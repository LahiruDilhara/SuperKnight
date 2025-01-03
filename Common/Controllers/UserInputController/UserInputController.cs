using Controllers;
using Globals;
using Godot;
using System;
using Types;

namespace Controllers
{
	public partial class UserInputController : IController
	{

		public override JumpSpec WantToJump()
		{
			if (InputManager.Instance.IsActionPressed("jump"))
			{
				return new JumpSpec();
			}
			return null;
		}

		public override MoveSpec WantToMove()
		{
			if (InputManager.Instance.IsActionPressed("left"))
			{
				return new MoveSpec
				{
					Direction = new Vector2(-1, 0)
				};
			}
			else if (InputManager.Instance.IsActionPressed("right"))
			{
				return new MoveSpec
				{
					Direction = new Vector2(1, 0)
				};
			};
			return null;
		}

	}

}
