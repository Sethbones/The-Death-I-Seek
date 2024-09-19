using Godot;
using System;
using System.Diagnostics;

public partial class CastleMusic : Area2D
{

	public AudioStreamPlayer GameMusic;
	// Called when the node enters the scene tree for the first time.
	[Export] AudioStream ForestGameMusic;
	[Export] AudioStream CastleGameMusic;
	public override void _Ready()
	{
		GameMusic = (AudioStreamPlayer)GetNode("AudioStreamPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_area_entered(Area2D area)
	{
		if (area.IsInGroup("Player")){
			Debug.Print("Findingsoos");
			GameMusic.Stop();
			GameMusic.Stream = CastleGameMusic;
			GameMusic.Play(0f);
		}
	}


	private void _on_area_exited(Area2D area)
	{
		if (area.IsInGroup("Player")){
			GameMusic.Stop();
			GameMusic.Stream = ForestGameMusic;
			GameMusic.Play(0f);
		}
	}
}



