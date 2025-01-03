using Controllers;
using Godot;
using System;

namespace Player
{
	public partial class StateMachine : Node
	{
		private CharacterBody2D Character;

		private AnimatedSprite2D Animation;
		private State CurrentState = null;

		[Export]
		private State initialState;

		[Export]
		private InputHandler inputHandler;

		[Export]
		private IController controller;

		public void Init(CharacterBody2D character, AnimatedSprite2D Animation = null)
		{
			this.Character = character;
			this.Animation = Animation;

			this.CurrentState = initialState;
			ChangeState(this.initialState);
		}

		private void ChangeState(State state)
		{
			this.CurrentState?.Exit();
			this.CurrentState = state;

			this.CurrentState.Character = this.Character;
			this.CurrentState.Animation = this.Animation;
			this.CurrentState.inputHandler = this.inputHandler;
			this.CurrentState.controller = this.controller;

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
