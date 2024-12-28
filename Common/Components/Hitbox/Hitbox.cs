using Godot;
using System;
using System.Buffers;

namespace Components
{
	public partial class Hitbox : Area2D
	{
		[Signal]
		public delegate void GetDamageEventHandler(Damage damageComponent);

		[Export]
		private bool _DamageImmuned = false;

		[Export]
		private bool _InstantDamageImmuned = false;

		public bool DamageImmuned => _DamageImmuned;

		public bool InstantDamageImmuned => _InstantDamageImmuned;

		[Export]
		public Hitpoint Hitpoint;

		[Export]
		public String Layer;

		public bool IsDead => Hitpoint.IsDied;

		public int Damage(int amount)
		{
			if (_DamageImmuned) return 0;
			int actualDamagedAmount = this.Hitpoint.Damage(amount);
			EmitSignal(nameof(this.GetDamage), actualDamagedAmount);
			return actualDamagedAmount;
		}

		public int InstantDamage()
		{
			if (_InstantDamageImmuned) return 0;
			int damage = this.Hitpoint.DeadInstantly();
			EmitSignal(nameof(this.GetDamage), damage);
			return damage;
		}

	}
}
