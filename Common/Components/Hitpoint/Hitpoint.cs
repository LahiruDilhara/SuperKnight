using Godot;
using System;

namespace Components
{
	public partial class Hitpoint : Node2D
	{
		[Signal]
		public delegate void HitpointChangeEventHandler(int hitpoint);

		[Signal]
		public delegate void DiedEventHandler();

		private int _MaxHitpoints = 0;

		private int _CurrentHitpoints = 0;

		[Export]
		public int MaxHitpoints
		{
			get => _MaxHitpoints;
			private set
			{
				_MaxHitpoints = value;

				// Update the current hitpoints to ensure it stays within bounds
				CurrentHitpoints = _CurrentHitpoints;
			}
		}

		public int CurrentHitpoints
		{
			get => _CurrentHitpoints;
			private set
			{
				int newValue = Mathf.Clamp(value, 0, _MaxHitpoints);
				if (newValue == _CurrentHitpoints) return;          // Avoid unnecessary updates
				this._CurrentHitpoints = newValue;

				// Emit the hitpoint change signal
				EmitSignal(SignalName.HitpointChange, _CurrentHitpoints);
				if (!HasHealthRemaining && !hasDied)
				{
					hasDied = true;
					EmitSignal(SignalName.Died);
				}
			}
		}

		private bool hasDied = false;

		[Export]
		private bool isImmuned = false;

		public bool IsDamaged => CurrentHitpoints < MaxHitpoints;

		public bool HasHealthRemaining => CurrentHitpoints > 0;

		public bool IsDied => hasDied;

		public override void _Ready()
		{
			this.CurrentHitpoints = this.MaxHitpoints;
		}

		public void Initialize(int MaxHitpoints)
		{
			this.MaxHitpoints = MaxHitpoints;
			this.CurrentHitpoints = MaxHitpoints;
		}

		public int Damage(int damage)
		{
			if (isImmuned) return 0;
			int damageAmount = (this.CurrentHitpoints < damage) ? CurrentHitpoints : damage;
			this.CurrentHitpoints -= damage;
			return damageAmount;
		}

		public int Heal(int hitpoints)
		{
			int amountToHeal = this.MaxHitpoints - this.CurrentHitpoints;
			int healAmount = (amountToHeal < hitpoints) ? amountToHeal : hitpoints;
			this.CurrentHitpoints += hitpoints;
			return healAmount;
		}

		public int HealToMax()
		{
			int amountToHeal = this.MaxHitpoints - this.CurrentHitpoints;
			this.CurrentHitpoints += MaxHitpoints;
			return amountToHeal;
		}

		public void SetMaxHitpoints(int hitpoints)
		{
			this.MaxHitpoints = hitpoints;
		}

		public int DeadInstantly()
		{
			int temp = this.CurrentHitpoints;
			this.CurrentHitpoints -= MaxHitpoints;
			return temp;
		}
	}

}
