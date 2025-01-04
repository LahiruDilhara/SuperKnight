using Components.Pickables;
using Godot;
using System;

namespace Components
{
	public partial class PicableCollector : Area2D
	{
		[Signal]
		public delegate void PickedEventHandler(Node pickableNode);

		// Change the ability to pick a pickable
		[Export]
		public bool CanPick = true;

		public override void _Ready()
		{
			base._Ready();
			// Connect with the area entered signal
			if (!this.IsConnected("area_entered", new Callable(this, nameof(this.OnAreaEntered))))
			{
				this.Connect("area_entered", new Callable(this, nameof(this.OnAreaEntered)));
			}
		}

		public void Pick(Node pickableNode)
		{
			EmitSignal(nameof(this.Picked), pickableNode);
		}

		protected void OnAreaEntered(Area2D area)
		{
			if (!CanPick) return;
			if (area is not Pickable) return;
			Pickable pickable = area as Pickable;

			// Get a clone of the current pickable. so even the current pickable is destroyed, its data is preserved.
			Pick(pickable.PickedNode);

			// Call the pickable picked method
			pickable.Picked();
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			if (this.IsConnected("area_entered", new Callable(this, nameof(this.OnAreaEntered))))
			{
				this.Disconnect("area_entered", new Callable(this, nameof(this.OnAreaEntered)));
			}
		}
	}
}
