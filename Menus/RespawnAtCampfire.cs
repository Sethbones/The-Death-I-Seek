using Godot;
using System;

public partial class RespawnAtCampfire : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_pressed()
	{
		globals.SpawnAtCheckpoint = true;
		GetTree().ChangeSceneToFile("res://Game.tscn");
	}

	[Export] AudioStreamPlayer2D MenuBlip;
	private void _on_mouse_entered()
	{
		MenuBlip.Play();
	}
}
