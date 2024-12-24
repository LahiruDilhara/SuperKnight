using Godot;
using System;

namespace Libs
{
	public partial class State : Node
	{
		[Signal]
		public delegate void StateChangeEventHandler(State state);

		public Node SuperNode;

		public virtual void Enter() { }

		public virtual void ProcessUpdate(float delta) { }

		public virtual void InputUpdate(InputEvent @event) { }

		public virtual void PhysicsUpdate(float delta) { }

		public virtual void Exit() { }

		protected void ChangeState(State state)
		{
			EmitSignal(nameof(this.StateChange), state);
		}
	}
}