using Godot;
using System;

public partial class Notes : Button
{
	public int menustate = 0;

	public AnimationPlayer animinstance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animinstance = (AnimationPlayer)GetNode("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_pressed()
	{
		if (menustate == 0){
			animinstance.Play("In");
			menustate = 1;
		}
		else if (menustate == 1){
			animinstance.Play("Out");
			menustate = 0;
		}
	}

	[Export] AudioStreamPlayer2D MenuBlip;
	private void _on_mouse_entered()
	{
		MenuBlip.Play();
	}
}
