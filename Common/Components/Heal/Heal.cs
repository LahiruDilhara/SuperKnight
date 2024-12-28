using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Components
{
    public abstract partial class Heal : Area2D
    {
        [Signal]
        public delegate void OnAreaHealEventHandler(int amount);

        [Export]
        protected bool DoHeal = true;

        public List<String> HelableLayers = new List<String>();

        public List<String> UnHelableLayers = new List<String>();

        public override void _Ready()
        {
            AreaEntered += OnAreaEntered;
            AreaExited += OnAreaExited;
        }

        abstract protected void Healing(HealBox healBox);

        protected void Healing(HealBox healBox, int amount) { }

        protected virtual void OnAreaEntered(Area2D area) { }
        protected virtual void OnAreaExited(Area2D area) { }

        protected bool IsHealable(HealBox healBox)
        {
            if (healBox == null || !DoHeal) return false;
            if (healBox.IsDied || !IsInstanceValid(healBox) || !healBox.IsDamaged) return false;

            foreach (String unHelableLayer in UnHelableLayers)
            {
                if (healBox.Layer == unHelableLayer) return false;
            }

            if (!HelableLayers.Any()) return true;

            foreach (String healableLayer in HelableLayers)
            {
                if (healBox.Layer == healableLayer) return true;
            }
            return false;
        }

        protected void SendHealSignal(int healAmount)
        {
            if (healAmount != 0)
            {
                EmitSignal(nameof(OnAreaHeal), healAmount);
            }
        }
    }
}