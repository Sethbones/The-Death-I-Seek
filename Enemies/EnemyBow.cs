using Godot;
using System;
using System.Diagnostics;

public partial class EnemyBow : Sprite2D
{
	[Export]public PackedScene EnemyArrow;
	public float bowTimer = 0.5f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var arrowinstance = (Area2D)EnemyArrow.Instantiate();
		GetTree().Root.CallDeferred("add_child", arrowinstance);
		var Enemyinstance = (CharacterBody2D)GetParent().GetParent();
		var spawnpoint = (Node2D)GetParent();
		arrowinstance.Position = Enemyinstance.GlobalPosition;
		//Debug.Print(arrowinstance.Position.ToString());
		Debug.Print("where it is: " + Enemyinstance.Position.ToString() + ", " + "where it should be " + arrowinstance.Position.ToString());
		arrowinstance.Rotation = spawnpoint.GlobalRotation;
		arrowinstance.Scale = spawnpoint.GlobalScale;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (bowTimer > 0){
			bowTimer -= (float)delta;
		}

		if (bowTimer < 0){
			QueueFree();
		}
	}
}
