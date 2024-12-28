using Components;
using Godot;
using System;

namespace Components
{
	public partial class ShotDamage : Damage
	{
		[Export]
		protected int Amount = 0;

		[Export]
		protected bool OnEnter = true;

		[Export]
		protected bool OnExit = false;

		protected override void Attack(Hitbox hitbox)
		{
			if (!IsDamagable(hitbox)) return;
			int damageAmount = hitbox.Damage(Amount);
			EmitSignal(nameof(OnAreaDamage), damageAmount);
		}

		protected override void OnAreaEntered(Area2D area)
		{
			if (area is Hitbox hitbox && OnEnter)
			{
				Attack(hitbox);
			}
		}

		protected override void OnAreaExited(Area2D area)
		{
			if (area is Hitbox hitbox && OnExit)
			{
				Attack(hitbox);
			}
		}
	}

}
