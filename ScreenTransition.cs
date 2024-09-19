using Godot;
using System;
using System.Diagnostics;

public partial class ScreenTransition : Area2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]public Camera2D camerainstance;

	[Export] string currentDir;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_body_entered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			if (currentDir == "Down"){

			}
			else if (currentDir == "Up"){

			}
			else if (currentDir == "Left"){
				camerainstance.Position = new Vector2(camerainstance.Position.X - 240,camerainstance.Position.Y);
			}
			else if (currentDir == "Right"){
				camerainstance.Position = new Vector2(camerainstance.Position.X + 240,camerainstance.Position.Y);
			}
		}
	}
}
