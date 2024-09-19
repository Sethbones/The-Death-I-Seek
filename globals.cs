using Godot;
using System;

public partial class globals : Node
{
	public static int playerXlocation;
	public static int playerYlocation;
	public static int playerClass;
	public static int CameraXlocation;
	public static int CameraYlocation;
	public static bool SpawnAtCheckpoint = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
