using Godot;
using System;
using System.Diagnostics;

public partial class Knight : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
	public Area2D PlayerArea;
	public bool Z_PlayerInRange = false;
	public bool Z_PlayerDetected = false;
	private const float Z_Ray_Length = 50f;
	public float Z_MoveSpeed = 20f;

	[Export] public PackedScene DeathPoof;
	
	public object Z_LastPlayerPosition = null;
	public bool IsActive = false;

	public int EnemyClass = 5;
	public int Health = 1;

	public override void _Ready()
	{
		AddToGroup("Enemy");
		GetNode("GameCollision").AddToGroup("Enemy");
	}

	public override void _Process(double delta)
	{
		if (IsActive){
			var Ray = GetNode<RayCast2D>("RayCast2D");
			AnimationPlayer animinstnace = (AnimationPlayer)GetNode("AnimationPlayer");
			if (PlayerArea != null){
			
				var PlayerAreaParent = PlayerArea.GetParent<CharacterBody2D>();
				//Ray.Rotation = GetAngleTo(PlayerArea.Position);
				//Ray.Rotation = Ray.GetAngleTo(PlayerArea.Position);
				
				var direction = PlayerAreaParent.Position - GlobalPosition;
				Ray.Rotation = direction.Angle() -1.5f;
				//Debug.Print("playerAreaPosition " + PlayerArea.Position.ToString());
				//Debug.Print("ZombiePosition " + Position.ToString());
				//Debug.Print(Ray.Rotation.ToString());

				//Debug.Print(Ray.IsColliding().ToString());
				if(Ray.IsColliding()){
					//Debug.Print("something in range");
					var collider = (Node2D)Ray.GetCollider();
					// Debug.Print(collider.GetGroups().ToString());
					if (collider.IsInGroup("Player")){
						
						//Debug.Print("enemy position " + Position.ToString() + ", " + "player position " + PlayerAreaParent.Position.ToString());
						Z_LastPlayerPosition = PlayerAreaParent.Position;
						//Position = Position.MoveToward(PlayerAreaParent.Position, Z_MoveSpeed * (float)delta);
						//Z_LastPlayerPosition = PlayerArea.GetParent<CharacterBody2D>().Position;
						//Position = new Vector2((float)Math.Round(Position.X,0), (float)Math.Round(Position.Y,0));
						//Debug.Print("move to target");
						//Debug.Print("enemy position - player position " + (GlobalPosition - PlayerAreaParent.Position).Normalized().Round().ToString());
						//Debug.Print("enemy position - player position " + Position.ToString() + ", " + "player position " + PlayerAreaParent.Position.ToString());
						
					}
				}
			}
			if (Z_LastPlayerPosition != null){
				GlobalPosition = GlobalPosition.MoveToward((Vector2)Z_LastPlayerPosition, Z_MoveSpeed * (float)delta);
				var MoveVector = (GlobalPosition - PlayerArea.GetParent<CharacterBody2D>().Position).Normalized().Round();
				//Debug.Print(MoveVector.ToString());
				if (MoveVector == new Vector2(0,-1)){
					animinstnace.Play("Down");
				}
				else if (MoveVector == new Vector2(0,1)){
					animinstnace.Play("Up");
				}
				else if (MoveVector == new Vector2(1,0)){
					animinstnace.Play("Left");
				}
				else if (MoveVector == new Vector2(-1,0)){
					animinstnace.Play("Right");
				}
				if (GlobalPosition == (Vector2)Z_LastPlayerPosition){
					Z_LastPlayerPosition = null;
				}
			}
			else{
				animinstnace.Pause();
			}
		}
		else{
			var enemyparent = (Node2D)GetParent();
			//Debug.Print("position " + Position.ToString() + " GlobalPosition " + GlobalPosition.ToString());
			GlobalPosition = enemyparent.Position;
			Z_LastPlayerPosition = null;
		}
		//Debug.Print(IsActive.ToString());

		if (Health <= 0){
			Die();
			QueueFree();
		}
	}

	public override void _PhysicsProcess(double delta){
		MoveAndSlide();
	}
	
	private void _on_area_detection_area_entered(Area2D area)
	{
		if(area.IsInGroup("Player")){
			Debug.Print("player spotted");
			PlayerArea = area;
		}
		// Replace with function body.
	}

	private void _on_area_detection_area_exited(Area2D area)
	{
		if (area == PlayerArea){
			Debug.Print("player left");
			//PlayerArea = null;
		}
	}
	public void Die(){
		Node2D poofspawn = (Node2D)DeathPoof.Instantiate();
		//GetTree().Root.AddChild(poofspawn);
		GetTree().Root.CallDeferred("add_child", poofspawn);
		//poofspawn.Position = Position;
		poofspawn.GlobalPosition = GlobalPosition;
	}
	private void _on_game_collision_area_entered(Area2D area)
	{
		if (area.IsInGroup("Player")){
			Debug.Print("Fuckir");
			globals.playerClass = EnemyClass;
		}
	}
}
