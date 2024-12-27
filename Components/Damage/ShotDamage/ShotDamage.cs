using Components;
using Godot;
using System;

public partial class ShotDamage : Damage
{
	[Export]
	private int Amount = 0;

	protected override void Attack(Hitbox hitbox)
	{

	}
}
