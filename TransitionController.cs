using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

public partial class TransitionController : Area2D
{
	[Export] public Camera2D CameraInstance;
	[Export] public Player PlayerInstance;
	public bool TransitionRight = false;
	public bool TransitionLeft = false;
	public bool TransitionUp = false;
	public bool TransitionDown = false;

	public float MapRightPoint;
	public float MapLeftPoint;
	public float MapPointUp;
	public float MapPointDown;

	public float PlayerPointRight;
	public float PlayerPointLeft;
	public float PlayerPointUp;
	public float PlayerPointDown;

	[Export] public PackedScene Campfire;

	public float MapPointY;
	// Called when the node enters the scene tree for the first time.
	[Export] public float MapTransSpeed = 500f;

	public const float delayedStartConst = 0.01f;
	public float delayedStart = delayedStartConst;
	public bool delayedStartEnded = false;

	public override void _Ready()
	{
		if (globals.SpawnAtCheckpoint && globals.playerXlocation != 0 && globals.playerYlocation != 0){
			PlayerInstance.Position = new Vector2(globals.playerXlocation,globals.playerYlocation);
			CameraInstance.Position = new Vector2(globals.CameraXlocation,globals.CameraYlocation);
			Node2D campinstant = (Node2D)Campfire.Instantiate();
			GetTree().Root.CallDeferred("add_child", campinstant);
			campinstant.Position = PlayerInstance.Position;
			resetMapPoints();
		}
		MapPointDown = CameraInstance.Position.Y + 128;
		MapPointUp = CameraInstance.Position.Y - 128;
		MapRightPoint = CameraInstance.Position.X + 240;
		MapLeftPoint = CameraInstance.Position.X - 240;
		PlayerPointRight = CameraInstance.Position.X + 130;
		PlayerPointLeft = CameraInstance.Position.X - 130;
		PlayerPointUp = CameraInstance.Position.Y - 80;
		PlayerPointDown = CameraInstance.Position.Y + 80;
		UnfreezeEnemy();
	}

	public override void _Process(double delta){
		//dumbassary, i need a ready function that is delayed by a frame
		if (delayedStart > 0){
			delayedStart -= (float)delta;
		}
		else{
			if (!delayedStartEnded){
				UnfreezeEnemy();
				delayedStartEnded = true;
			}
		}


		if (Input.IsActionJustPressed("CreateCheckpoint")){
			if (PlayerInstance.canMove){
				foreach (Sprite2D AllCampfires in GetTree().GetNodesInGroup("Campfire"))
				{
					AllCampfires.QueueFree();
				}
				globals.playerXlocation = (int)PlayerInstance.Position.X;
				globals.playerYlocation = (int)PlayerInstance.Position.Y;
				globals.CameraXlocation = (int)CameraInstance.Position.X;
				globals.CameraYlocation = (int)CameraInstance.Position.Y;
				var CampFireInst = (Sprite2D)Campfire.Instantiate();
				GetTree().Root.CallDeferred("add_child", CampFireInst);
				CampFireInst.Position = new Vector2((int)PlayerInstance.Position.X,(int)PlayerInstance.Position.Y);
			}
		}
		else if (Input.IsActionJustPressed("LoadCheckpoint")){
			if (PlayerInstance.canMove){
				PlayerInstance.Position = new Vector2(globals.playerXlocation,globals.playerYlocation);
				CameraInstance.Position = new Vector2(globals.CameraXlocation,globals.CameraYlocation);
				resetMapPoints();
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		//so basically
			//get the camera position, to a variable
			//set it to a vector2
			//move it
			//round
			//set it
			//honestly don't know if this is smart code but i don't really care because it works
		float CamPosX = CameraInstance.Position.X;
		float CamPosY = CameraInstance.Position.Y;
		Vector2 CamPosition = new Vector2(CamPosX, CamPosY);

		if (TransitionRight == true){
			if (CamPosition.X != MapRightPoint){
				FreezePlayer();
				FreezeEnemy();
				CamPosition = CamPosition.MoveToward(new Vector2(MapRightPoint,CameraInstance.Position.Y), MapTransSpeed * Convert.ToSingle(delta));
				PlayerInstance.Position = PlayerInstance.Position.MoveToward(new Vector2(PlayerPointRight, PlayerInstance.Position.Y), MapTransSpeed * Convert.ToSingle(delta));
			}
			else{
				TransitionRight = false;
				resetMapPoints();
				UnFreezePlayer();
				UnfreezeEnemy();
			}
		}
		else if (TransitionLeft == true){
			if (CamPosition.X != MapLeftPoint){
				FreezePlayer();
				FreezeEnemy();
				CamPosition = CamPosition.MoveToward(new Vector2(MapLeftPoint,CameraInstance.Position.Y), MapTransSpeed * Convert.ToSingle(delta));
				PlayerInstance.Position = PlayerInstance.Position.MoveToward(new Vector2(PlayerPointLeft, PlayerInstance.Position.Y), MapTransSpeed * Convert.ToSingle(delta));
			}
			else{
				TransitionLeft = false;
				resetMapPoints();
				UnFreezePlayer();
				UnfreezeEnemy();
			}
		}
		else if (TransitionUp == true){
			if (CamPosition.Y != MapPointUp){
				CamPosition = CamPosition.MoveToward(new Vector2(CameraInstance.Position.X,MapPointUp), (MapTransSpeed * Convert.ToSingle(delta)) / 2);
				PlayerInstance.Position = PlayerInstance.Position.MoveToward(new Vector2(PlayerInstance.Position.X, PlayerPointUp), (MapTransSpeed * Convert.ToSingle(delta)) / 2);
				FreezePlayer();
				FreezeEnemy();
			}
			else{
				TransitionUp = false;
				resetMapPoints();
				UnFreezePlayer();
				UnfreezeEnemy();
			}
		}
		else if (TransitionDown == true){
			if (CamPosition.Y != MapPointDown){
				CamPosition = CamPosition.MoveToward(new Vector2(CameraInstance.Position.X,MapPointDown), (MapTransSpeed * Convert.ToSingle(delta)) / 2);
				PlayerInstance.Position = PlayerInstance.Position.MoveToward(new Vector2(PlayerInstance.Position.X, PlayerPointDown), (MapTransSpeed * Convert.ToSingle(delta)) / 2);
				FreezePlayer();
				FreezeEnemy();
			}
			else{
				TransitionDown = false;
				resetMapPoints();
				UnFreezePlayer();
				UnfreezeEnemy();
			}
		}


		float roundedCamPositionX = (float)Math.Round(CamPosition.X);
		float roundedCamPositionY = (float)Math.Round(CamPosition.Y);
		CameraInstance.Position = new Vector2(roundedCamPositionX, roundedCamPositionY);
		//Debug.Print(PlayerInstance.Position.X.ToString());
		//Debug.Print(CameraInstance.Position.ToString());
	}

	public void resetMapPoints(){
		MapPointDown = CameraInstance.Position.Y + 128;
		MapPointUp = CameraInstance.Position.Y - 128;
		MapRightPoint = CameraInstance.Position.X + 240;
		MapLeftPoint = CameraInstance.Position.X - 240;
		PlayerPointRight = CameraInstance.Position.X + 130;
		PlayerPointLeft = CameraInstance.Position.X - 130;
		PlayerPointUp = CameraInstance.Position.Y - 80;
		PlayerPointDown = CameraInstance.Position.Y + 80;
	}

	public void FreezePlayer(){
		PlayerInstance.canMove = false;
	}

	public void UnFreezePlayer(){
		PlayerInstance.canMove = true;
	}

	public void FreezeEnemy(){
		var NodeInstance = (Node2D)GetParent().GetNode("Point");
		Rect2 ViewportRect = CameraInstance.GetViewportRect();
		Vector2 ViewportRectPosition = NodeInstance.GlobalPosition;
		Vector2 ViewportRectSize = new Vector2(ViewportRect.Size.X, ViewportRect.Size.Y - 7);
		Rect2 CameraRect2 = new Rect2(ViewportRectPosition,ViewportRectSize);
		//Debug.Print(CameraRect2.ToString());
		//Debug.Print(CameraRect2.End.ToString());
		//var NodeInstance = (Area2D)GetParent().GetNode("ObjectFreezeZone");
		foreach (Node2D AllEnemies in GetTree().GetNodesInGroup("Enemy"))
		{
			//Debug.Print(AllEnemies.GetClass());
			//Debug.Print(CameraInstance.GetViewport().GetVisibleRect().ToString());
			if (CameraRect2.HasPoint(AllEnemies.GlobalPosition)){
				//Debug.Print("freeze bitch");
				if (AllEnemies.IsClass("CharacterBody2D") || AllEnemies.IsClass("Area2D") ){
					//Debug.Print("Found enemy at: " + AllEnemies.GlobalPosition.ToString());
					//Debug.Print(CameraInstance.GetViewport().GetVisibleRect().ToString());
					//Debug.Print(AllEnemies.Get("IsActive").ToString());
					AllEnemies.Set("IsActive", false);
					AllEnemies.GetParent().Set("IsActive", false);
					//AllEnemies.GlobalPosition = Position;
				}
			}
		}
	}

	public void UnfreezeEnemy(){
		var NodeInstance = (Node2D)GetParent().GetNode("Point");
		Rect2 ViewportRect = CameraInstance.GetViewportRect();
		Vector2 ViewportRectPosition = NodeInstance.GlobalPosition;
		Vector2 ViewportRectSize = new Vector2(ViewportRect.Size.X, ViewportRect.Size.Y - 7);
		Rect2 CameraRect2 = new Rect2(ViewportRectPosition,ViewportRectSize);
		//Debug.Print(CameraRect2.ToString());
		//Debug.Print(CameraRect2.End.ToString());
		//var NodeInstance = (Area2D)GetParent().GetNode("ObjectFreezeZone");
		foreach (Node2D AllEnemies in GetTree().GetNodesInGroup("Enemy"))
		{
			//Debug.Print(AllEnemies.GetClass());
			//Debug.Print(CameraInstance.GetViewport().GetVisibleRect().ToString());
			if (CameraRect2.HasPoint(AllEnemies.GlobalPosition)){
				//Debug.Print("freeze bitch");
				if (AllEnemies.IsClass("CharacterBody2D") || AllEnemies.IsClass("Area2D") ){	
					//Debug.Print("Found enemy at: " + AllEnemies.GlobalPosition.ToString());
					//Debug.Print(CameraInstance.GetViewport().GetVisibleRect().ToString());
					//Debug.Print(AllEnemies.Get("IsActive").ToString());
					AllEnemies.Set("IsActive", true);
					AllEnemies.GetParent().Set("IsActive", true);
					//AllEnemies.GlobalPosition = Position;
				}
			}
		}
	}

	private void _on_body_entered(Node2D body)
	{
		Debug.Print(body.Name);
		if (body.IsInGroup("Player")){
			
		}
	}

	private void _on_body_shape_entered(Rid body_rid, Node2D body, long body_shape_index, long local_shape_index)
	{
		// right = 0, 1 = left, 2 = up, 3 = down 
		if (body.IsInGroup("Player")){
			if (local_shape_index == 0){
				Debug.Print("heading right");
				TransitionRight = true;
				//CameraInstance.Position = new Vector2(CameraInstance.Position.X + 240,CameraInstance.Position.Y);
			}
			else if (local_shape_index == 1){
				Debug.Print("heading left");
				TransitionLeft = true;
				//CameraInstance.Position = new Vector2(CameraInstance.Position.X - 240,CameraInstance.Position.Y);
			}
			else if (local_shape_index == 2){
				Debug.Print("heading Up");
				TransitionUp = true;
			}
			else if (local_shape_index == 3){
				Debug.Print("heading Down");
				TransitionDown = true;
			}
		}
		
	}

}



