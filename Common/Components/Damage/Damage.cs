using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Components
{
    abstract public partial class Damage : Area2D
    {

        [Signal]
        public delegate void OnAreaDamageEventHandler(int amount);

        [Export]
        protected bool DoDamage = true;

        public List<String> DamagableLayers = new List<String>();
        public List<String> UnDamagableLayers = new List<String>();

        public override void _Ready()
        {
            AreaEntered += OnAreaEntered;
            AreaExited += OnAreaExited;
        }

        abstract protected void Attack(Hitbox hitbox);

        protected virtual void Attack(Hitbox hitbox, int amount) { }
        protected virtual void OnAreaEntered(Area2D area) { }
        protected virtual void OnAreaExited(Area2D area) { }

        protected bool IsDamagable(Hitbox hitbox)
        {
            if (hitbox == null || !DoDamage) return false;
            if (hitbox.IsDead || !IsInstanceValid(hitbox)) return false;

            foreach (String unDamagableLayer in UnDamagableLayers)
            {
                if (hitbox.Layer == unDamagableLayer) return false;
            }

            if (!DamagableLayers.Any()) return true;

            foreach (String damagableLayer in DamagableLayers)
            {
                if (hitbox.Layer == damagableLayer) return true;
            }

            return false;
        }
        protected void SendAttackSignal(int damageAmount)
        {
            if (damageAmount != 0)
            {
                EmitSignal(nameof(OnAreaDamage), damageAmount);
            }
        }
    }
}

