using Godot;
using System;

public partial class InputHandler : Node
{
	public float GetMovementDirection()
	{
		return Input.GetAxis("left", "right");
	}

	public bool WantToJump()
	{
		return Input.IsActionJustPressed("jump");
	}
}
