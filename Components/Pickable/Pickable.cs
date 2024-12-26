using Godot;
using System;

namespace Compositions
{
	public partial class Pickable : Area2D
	{
		[Signal]
		public delegate void PickEventHandler(Node2D body);

		public Type NodeType = null;

		[Export]
		private bool RemoveOnPick = false;

		[Export]
		private Node Removable = null;

		public override void _Ready()
		{
			this.BodyEntered += OnBodyEntered;
		}

		private void OnBodyEntered(Node2D body)
		{
			if (NodeType != null)
			{
				if (body.GetType() == NodeType)
				{
					GD.Print("Called with specific node");
					EmitAndRemoveHandle(body);
				}
			}
			else
			{
				GD.Print("Called this");
				EmitAndRemoveHandle(body);
			}
		}

		private void EmitAndRemoveHandle(Node2D body)
		{
			EmitSignal(nameof(this.Pick), body);
			if (RemoveOnPick && Removable != null)
			{
				Removable.QueueFree();
			}
		}
	}
}
