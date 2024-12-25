using Godot;
using System;

namespace Player
{
	public partial class StateMachine : Node
	{
		private CharacterBody2D Character;

		private AnimatedSprite2D Animation;
		private State CurrentState = null;

		public int Gravity;

		public int JumpProjectionSpeed;

		public int JumpHeight;

		public int RunSpeed;

		[Export]
		private State initialState;

		[Export]
		private InputHandler inputHandler;

		public void Init(CharacterBody2D character, int Gravity = 980, int JumpProjectionSpeed = 200, int JumpHeight = -200, int RunSpeed = 200, AnimatedSprite2D Animation = null)
		{
			this.Character = character;
			this.Gravity = Gravity;
			this.JumpProjectionSpeed = JumpProjectionSpeed;
			this.JumpHeight = JumpHeight;
			this.RunSpeed = RunSpeed;
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
			this.CurrentState.Gravity = this.Gravity;
			this.CurrentState.JumpProjectionSpeed = this.JumpProjectionSpeed;
			this.CurrentState.JumpHeight = this.JumpHeight;
			this.CurrentState.RunSpeed = this.RunSpeed;
			this.CurrentState.inputHandler = this.inputHandler;

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
