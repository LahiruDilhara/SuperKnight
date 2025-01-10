using Globals;
using Godot;
using System;

public partial class MainUi : Control
{
	private Button StartButton;
	private Button ExitButton;
	public override void _Ready()
	{
		this.StartButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/Start");
		this.ExitButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/Exit");

		// Connect to signals
		if (!StartButton.IsConnected("button_down", new Callable(this, nameof(this.OnStart))))
		{
			StartButton.Connect("button_down", new Callable(this, nameof(this.OnStart)));
		}
		if (!ExitButton.IsConnected("button_down", new Callable(this, nameof(this.OnExit))))
		{
			ExitButton.Connect("button_down", new Callable(this, nameof(this.OnExit)));
		}
	}

	private void OnStart()
	{
		LevelManager.Instance.LoadCurrentLevel(removeNode: this);
	}

	private void OnExit()
	{
		GameManager.Instance.ExitGame();
	}
}
