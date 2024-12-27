using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Components
{
    abstract public partial class Damage : Area2D
    {
        public List<String> DamagableLayers = new List<String>();
        public List<String> UnDamagableLayers = new List<String>();

        abstract protected void Attack(Hitbox hitbox);

        protected virtual void OnAreaEntered(Area2D area) { }
        protected virtual void OnAreaExited(Area2D area) { }

        private bool IsDamagable(Hitbox hitbox)
        {
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
    }
}

