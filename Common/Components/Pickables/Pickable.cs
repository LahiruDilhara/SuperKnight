using Godot;
using System;

namespace Components.Pickables
{
	public partial class Pickable : Area2D
	{
		[Signal]
		public delegate void PickEventHandler();

		public Type NodeType = null;

		[Export]
		private bool RemoveOnPick = false;

		// Set the node which need to be removed when picked
		[Export]
		private Node RemovableNode = null;

		public override void _Ready()
		{
			this.RemovableNode = GetParent();
		}

		public void Picked()
		{
			EmitSignal(nameof(this.Pick));
			if (RemoveOnPick && RemovableNode != null)
			{
				RemovableNode.QueueFree();
			}
		}

		public virtual Pickable Clone()
		{
			return new Pickable();
		}
	}
}
