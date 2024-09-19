using Godot;
using System;
using System.Diagnostics;

public partial class Sword : Godot.Area2D
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimationPlayer Swordanims = (AnimationPlayer)GetNode("AnimationPlayer");
		Swordanims.Play("Swing");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}



	private void _on_animation_player_animation_finished(StringName anim_name)
	{
		if (anim_name == "Swing"){
			QueueFree();
		}
	}

	private void _on_area_entered(Area2D area)
	{
		if (area.IsInGroup("Enemy")){
			Debug.Print(area.ToString());
			Debug.Print("hit enemy");
			area.GetParent().Set("Health", (int)area.GetParent().Get("Health") - 1);
			area.Set("Health", (int)area.Get("Health") - 1);
			//body.QueueFree();
		}
	}

}






