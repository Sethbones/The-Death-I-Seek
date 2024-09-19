using Godot;
using System;
using System.Diagnostics;

public partial class Arrow : Area2D
{
	public float arrowspeed = 100f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
		if (area.IsInGroup("Enemy")){
			Debug.Print(area.ToString());
			Debug.Print("hit enemy");
			area.GetParent().Set("Health", (int)area.GetParent().Get("Health") - 1);
			area.Set("Health", (int)area.Get("Health") - 1);
			QueueFree();
		}
		else if (area.IsInGroup("CamBounds")){
			QueueFree();
		}
	}

	private void _on_body_entered(Node2D body)
	{
		if (body.IsClass("TileMap")){
			QueueFree();
		}
	}
}



