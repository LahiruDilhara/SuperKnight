using Components;
using Godot;
using System;

public partial class InstantDamage : Damage
{

	protected override void Attack(Hitbox hitbox)
	{
		if (!IsDamagable(hitbox)) return;
		int damageAmount = hitbox.InstantDamage();
		EmitSignal(nameof(OnAreaDamage), damageAmount);
	}

	protected override void OnAreaEntered(Area2D area)
	{
		if (area is Hitbox hitbox)
		{
			Attack(hitbox);
		}
	}
}
