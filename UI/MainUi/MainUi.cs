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
	}

	private void OnStart()
	{
		SceneManager.Instance.LoadScene("res://Levels/Level1/Level1.tscn");
	}

	private void OnExit()
	{
		GameManager.Instance.ExitGame();
	}
}
