using Godot;
using System;
using System.Diagnostics;

public partial class Ghost : Area2D
{
	// Called when the node enters the scene tree for the first time.


	//so basically each point is not directly set by the ghost itself, each of them is a seperate empty node that just has coordinates for the ghost to use
	public Node2D PointA;
	public Node2D PointB;
	public Node2D PointC;
	public Node2D PointD;
	public Node2D CurrentPoint;
	public float MoveSpeed = 30f;

	public int EnemyClass = 2;
	public bool IsActive = false;


	public override void _EnterTree(){
		PointA = (Node2D)GetParent().Get("PointA");
		PointB = (Node2D)GetParent().Get("PointB");
		PointC = (Node2D)GetParent().Get("PointC");
		PointD = (Node2D)GetParent().Get("PointD");
	}
	public override void _Ready()
	{
		AddToGroup("Enemy");
		Position = PointA.Position;
		CurrentPoint = PointA;
		AnimationPlayer GhostAnims = (AnimationPlayer)GetNode("AnimationPlayer");
		GhostAnims.Play("Move");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (IsActive){
			if (Position == PointA.Position){
				if (CurrentPoint == PointA){
					Debug.Print("hitpointA");
					CurrentPoint = PointB;
				}
				}
				else if (Position == PointB.Position){
					if (CurrentPoint == PointB){
						CurrentPoint = PointC;
					}
				}
				else if (Position == PointC.Position){
					if (CurrentPoint == PointC){
						CurrentPoint = PointD;
					}
				}
				else if (Position == PointD.Position){
					if (CurrentPoint == PointD){
						CurrentPoint = PointA;
					}
				}
			Position = Position.MoveToward(CurrentPoint.Position, MoveSpeed * (float)delta);
		}
		else{
			GlobalPosition = PointA.GlobalPosition;
			CurrentPoint = PointA;
		}
		
	}

	private void _on_area_entered(Area2D area)
	{
		if (area.IsInGroup("Player")){
			Debug.Print("Fuckir");
			globals.playerClass = EnemyClass;
		}
	}
}



