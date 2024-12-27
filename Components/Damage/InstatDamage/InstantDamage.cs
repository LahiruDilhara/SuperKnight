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
		if (!IsDamagable(hitbox)) return;
		int damage = hitbox?.InstantDamage() ?? 0;
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
