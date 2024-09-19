using Godot;
using System;
using System.Diagnostics;

public partial class EnemyArrow : Area2D
{
	public float arrowspeed = 75f;
	public int EnemyClass = 3;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("Enemy");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		var speed = arrowspeed * (float)delta;
		float angle = Rotation;
		Vector2 vector = new Vector2(1, 0);
		Vector2 rotatedVector = vector.Rotated(angle); 
		Position += (rotatedVector * speed) * Scale;
		//Debug.Print(Rotation.ToString());
	}

	private void _on_area_entered(Node2D area)
	{
		if (area.IsInGroup("Player")){
			Debug.Print("Fuckir");
			globals.playerClass = EnemyClass;
		}
	}

	private void _on_body_entered(Node2D body)
	{
		if (body.IsClass("TileMap")){
			QueueFree();
		}
	}
}
