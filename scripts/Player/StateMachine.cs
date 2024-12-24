using Godot;
using Libs;
using System;

namespace Player
{
	public partial class StateMachine : Node
	{
		private Node rootNode;
		private State CurrentState = null;

		[Export]
		private State initialState;

		public void Init(Node rootNode)
		{
			this.rootNode = rootNode;
			this.CurrentState = initialState;
			ChangeState(this.initialState);
		}

		private void ChangeState(State state)
		{
			this.CurrentState?.Exit();
			this.CurrentState = state;
			this.CurrentState.SuperNode = this.rootNode;
			this.CurrentState.Enter();
			if (!this.CurrentState.IsConnected(nameof(this.CurrentState.StateChange), new Callable(this, nameof(this.ChangeState))))
			{
				this.CurrentState.Connect(nameof(this.CurrentState.StateChange), new Callable(this, nameof(this.ChangeState)));
			}
		}

		public void Process(double delta)
		{
			this.CurrentState?.ProcessUpdate((float)delta);
		}

		public void PhysicsProcess(double delta)
		{
			this.CurrentState?.PhysicsUpdate((float)delta);
		}

		public void Input(InputEvent @event)
		{
			this.CurrentState?.InputUpdate(@event);
		}
	}
}
