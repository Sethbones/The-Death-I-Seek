using Godot;
using System;
using System.Diagnostics;

public partial class Dragon : Area2D
{
	public Area2D PlayerArea;
	// Called when the node enters the scene tree for the first time.
	public bool IsActive = false;
	public const float ConstFireballTimer = 0.25f;
	public float FireballTimer = ConstFireballTimer;
	[Export] PackedScene Fireball;
	[Export] PackedScene Quake;
	public int phase = 0;
	public const float PhaseConst = 2f;
	public float TimeBetweenPhases = PhaseConst;
	//0 - Idle, Sitting Still
	//1 - fireballs
	//2 - claw earthquake
	//3 - mighty roar
	public int Health = 6;
	public int EnemyClass =  6;
	[Export]public PackedScene EndingScene;

	AnimationPlayer animinstance;

	public override void _Ready()
	{
		AddToGroup("Enemy");
		animinstance = (AnimationPlayer)GetNode("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		
		if (IsActive){
			if(PlayerArea != null){
				//on boss fight start basically
				var PlayerAreaParent = PlayerArea.GetParent<CharacterBody2D>();
				var Ray = (RayCast2D)GetNode("RayCast2D");
				var direction = PlayerAreaParent.Position - GlobalPosition;
				Ray.Rotation = direction.Angle() -1.5f;

				if (phase == 0){
					animinstance.Play("idle");
					if (TimeBetweenPhases > 0 ){
						TimeBetweenPhases -= (float)delta;
					}
					else{
						TimeBetweenPhases = PhaseConst;
						var selectphase = new Random().Next(1,4);
						phase = selectphase;
					}
				}
				else if(phase == 1){
					animinstance.Play("Breath");
					Debug.Print("in phase 1");

					if (FireballTimer > 0){
					FireballTimer -= (float)delta;
					}

					if (FireballTimer < 0){
						var fireballinstance = (Area2D)Fireball.Instantiate();
						GetTree().Root.CallDeferred("add_child", fireballinstance);
						fireballinstance.Position = Ray.GlobalPosition;
						fireballinstance.Rotation = Ray.GlobalRotation;
						FireballTimer = ConstFireballTimer;
					}
				}
				else if(phase == 2){
					animinstance.Play("Quake");
					Debug.Print("in phase 2");
					if (GetCurrentFrameNumber() == 0.6f){
						var quakeinstance = (Area2D)Quake.Instantiate();
						var clawpoint = (Node2D)GetNode("ClawL");
						GetTree().Root.CallDeferred("add_child", quakeinstance);
						quakeinstance.Position = clawpoint.GlobalPosition;
					}
					//Debug.Print(GetCurrentFrameNumber().ToString());
				}
				else if(phase == 3){
					animinstance.Play("Quake");
					Scale = new Vector2 (-1,1);
					Debug.Print("in phase 3");
					if (GetCurrentFrameNumber() == 0.6f){
						var quakeinstance = (Area2D)Quake.Instantiate();
						var clawpoint = (Node2D)GetNode("ClawL");
						GetTree().Root.CallDeferred("add_child", quakeinstance);
						quakeinstance.Position = clawpoint.GlobalPosition;
					}
					//Debug.Print(GetCurrentFrameNumber().ToString());
				}
			}
		}
		if (Health <= 0){
			GetTree().ChangeSceneToPacked(EndingScene);
			QueueFree();
		}
	}

	private void _on_player_detection_zone_area_entered(Area2D area)
	{
		if (area.IsInGroup("Player")){
			PlayerArea = area;
			IsActive = true;
		}
	}

	

	private void _on_animation_player_animation_finished(StringName anim_name)
	{
		if (anim_name == "Breath" && phase == 1){
			phase = 0;
		}
		else if (anim_name == "Quake" && phase == 2){
			phase = 0;
		}
		else if (anim_name == "Quake" && phase == 3){
			phase = 0;
			Scale = new Vector2 (1,1);
		}
		// Replace with function body.
	}

	private float GetCurrentFrameNumber()
	{
		float currentTime = (float)animinstance.CurrentAnimationPosition;
		float frameLength = (float)(animinstance.CurrentAnimationLength / animinstance.GetPlayingSpeed());
		float currentFrame = (float)Math.Round(currentTime / frameLength, 2);
		return currentFrame;
	}

	private void _on_area_entered(Area2D area)
	{
		if (area.IsInGroup("Player")){
			globals.playerClass = EnemyClass;
		}
	}

	private void _on_player_detection_zone_area_exited(Area2D area)
	{
		if (area.IsInGroup("Player")){
			PlayerArea = null;
			IsActive = false;
		}
	}
}









