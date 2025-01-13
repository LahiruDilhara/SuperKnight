using Globals;
using Godot;
using System;
using Types;

public partial class Level1 : Level
{
	public override void _Ready()
	{
		base._Ready();
		this.PlayerMaxHitpoints = 2500;
	}
}
