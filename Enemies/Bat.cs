using Godot;
using System;
using System.Diagnostics;

public partial class Bat : Area2D
{
	public Vector2 BatPoint;
	public Node2D Area2D;
	public float MoveSpeed = 15f;
	[Export] public PackedScene DeathPoof;
	public int Health = 1;
	public bool IsActive = false;
	public int EnemyClass = 4;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("Enemy");
		BatPoint = GenerateRandomPointInsideCircle();
		//Debug.Print(GenerateRandomPointInsideCircle().ToString());
		AnimationPlayer BatAnims = (AnimationPlayer)GetNode("AnimationPlayer");
		BatAnims.Play("Move");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsActive){
			if (Position != BatPoint){
				Position = Position.MoveToward(BatPoint, MoveSpeed * (float)delta);
			}
			else{
				BatPoint = GenerateRandomPointInsideCircle();
			}
		}
		else{
			//Debug.Print("Fuf");
			//GlobalPosition = Position;
			//Position = Position.MoveToward(Position, 10000);
			var parent = (Node2D)GetParent();
			GlobalPosition = parent.Position;
		}
		

		if (Health <= 0){
			Die();
			GetParent().QueueFree();
		}
	}

	private Vector2 GenerateRandomPointInsideCircle()
	{
		var Areainstance = GetParent().GetNode("BatRange");
		var AreainstanceCollisionRange = (CollisionShape2D)Areainstance.GetNode("BatRangeExtants");
		float radius = (AreainstanceCollisionRange.Shape as CircleShape2D).Radius;
		float angle = (float)GD.RandRange(0, Mathf.Tau);
		float distance = (float)GD.RandRange(0, radius);
		float x = distance * Mathf.Cos(angle);
		float y = distance * Mathf.Sin(angle);
		return new Vector2(x, y);
	}

	public void Die(){
		Node2D poofspawn = (Node2D)DeathPoof.Instantiate();
		//GetTree().Root.AddChild(poofspawn);
		GetTree().Root.CallDeferred("add_child", poofspawn);
		poofspawn.GlobalPosition = GlobalPosition;
	}
	private void _on_area_entered(Area2D area)
	{
		if (area.IsInGroup("Player")){
			Debug.Print("Fuckir");
			globals.playerClass = EnemyClass;
		}
	}
}
