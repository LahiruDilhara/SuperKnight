using Components;
using Godot;
using System;

public partial class InstantDamage : Damage
{
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	protected override void Attack(Hitbox hitbox)
	{
		hitbox.InstantDamage();
	}

	protected override void OnAreaEntered(Area2D area)
	{
		if (area is Hitbox hitbox)
		{
			Attack(hitbox);
		}
	}
}
