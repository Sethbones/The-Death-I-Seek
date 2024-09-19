using Godot;
using System;
using System.Diagnostics;

public partial class Bow : Sprite2D
{
	[Export]public PackedScene Arrow;

	public float bowTimer = 0.5f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var arrowinstance = (Area2D)Arrow.Instantiate();
		GetTree().Root.CallDeferred("add_child", arrowinstance);
		var playerinstance = (CharacterBody2D)GetParent().GetParent();
		var spawnpoint = (Node2D)GetParent();
		arrowinstance.Position = playerinstance.Position;
		arrowinstance.Rotation = spawnpoint.Rotation;
		arrowinstance.Scale = spawnpoint.Scale;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Debug.Print(bowTimer.ToString());
		if (bowTimer > 0){
			bowTimer -= (float)delta;
		}

		if (bowTimer < 0){
			QueueFree();
		}
	}
}
