using Components;
using Godot;
using System;

public partial class InstantDamage : Damage
{

	protected override void Attack(Hitbox hitbox)
	{
		if (hitbox == null) return;
		if (!IsDamagable(hitbox)) return;
		int damage = hitbox.InstantDamage();
		EmitSignal(nameof(OnAreaDamage), damage);
	}

	protected override void OnAreaEntered(Area2D area)
	{
		if (area is Hitbox hitbox)
		{
			Attack(hitbox);
		}
	}
}
