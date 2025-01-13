using Globals;
using Godot;
using System;

namespace Level
{
	public partial class Level1 : Level
	{
		public override void _Ready()
		{
			this.PlayerMaxHitpoints = 2500;
			base._Ready();
		}
	}

}
