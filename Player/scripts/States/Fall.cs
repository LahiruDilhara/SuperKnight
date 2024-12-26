using Godot;
using Player;
using System;

public partial class Fall : InAir
{

	[Export]
	public String AnimationName = "fall";

	public override void Enter()
	{
		// set idel animation
		Animation?.Play(AnimationName);
	}

	public override void ProcessUpdate(float delta)
	{
		base.ProcessUpdate(delta);
		if (Character.IsOnFloor())
		{
			ChangeState(Idle);
			return;
		}

	}
}
