using Godot;
using System;

public partial class quake : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public int EnemyClass = 6;
	public override void _Ready()
	{
		AddToGroup("Enemy");
		var animinstance = (AnimationPlayer)GetNode("AnimationPlayer");
		animinstance.Play("Quack");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_animation_player_animation_finished(StringName anim_name)
	{
		QueueFree();
	}

	private void _on_area_entered(Area2D area)
	{
		if (area.IsInGroup("Player")){
			globals.playerClass = EnemyClass;
		}
	}
}






