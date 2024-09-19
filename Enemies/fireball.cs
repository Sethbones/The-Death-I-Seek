using Godot;
using System;

public partial class fireball : Area2D
{
	public const float FireballSpeed = 50f;
	public int EnemyClass = 6;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("Enemy");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var speed = FireballSpeed * (float)delta;
		float angle = GlobalRotation;
		Vector2 vector = new Vector2(0, 1);
		Vector2 rotatedVector = vector.Rotated(Rotation); 
		Position += rotatedVector * speed;
		//Debug.Print(Rotation.ToString());
	}
	private void _on_area_entered(Area2D area)
	{
		if (area.IsInGroup("Player")){
			globals.playerClass = EnemyClass;
		}
	}
}



