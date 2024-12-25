using Godot;
using System;

namespace Compositions
{
	public partial class Hitpoint : Node
	{
		[Signal]
		public delegate void HitpointChangeEventHandler(int hitpoint);

		[Signal]
		public delegate void DiedEventHandler();

		[Export]
		public int MaxHitpoints
		{
			get => MaxHitpoints;
			private set {
				MaxHitpoints = value;
			}
		}

		public int CurrentHitpoints{
			get => CurrentHitpoints;
			private set {
				CurrentHitpoints = Mathf.Clamp(value,0,MaxHitpoints);
				EmitSignal(SignalName.HitpointChange, CurrentHitpoints);
				if(!HasHealthRemaining && !hasDied){
					hasDied = true;
					EmitSignal(SignalName.Died);
				}
			}
		}

		private bool hasDied;

		public bool IsDamaged => CurrentHitpoints < MaxHitpoints;

		public bool HasHealthRemaining => !Mathf.IsEqualApprox(CurrentHitpoints,0);

        public override void _Ready()
        {
            this.CurrentHitpoints = this.MaxHitpoints;
        }

		public void Damage(int damage){
			this.CurrentHitpoints -= damage;
		}

		public void Heal(int hitpoints){
			this.CurrentHitpoints+= hitpoints;
		}

		public void SetMaxHitpoints(int hitpoints){
			this.MaxHitpoints = hitpoints;
		}

		public void InitializeHitpoints(){
			this.CurrentHitpoints = MaxHitpoints;
		}
    }

}
