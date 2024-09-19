using Godot;
using System;
using System.Diagnostics;


public partial class Player : Godot.CharacterBody2D
{
	public const int Speed = 50;
	[Export]
	public PackedScene Sword;
	[Export] public AnimationPlayer PlayerAnimations;
	[Export] public Node2D SwordSpawnPoint;
	[Export] public Sprite2D SpriteInstance;
	[Export] public Texture2D[] textures;
	public const float constantswingtimer = 0.5f;
	public float swingtimer = constantswingtimer;
	public bool canSwing = true;
	public bool canMove = true; //this is useful for tons of things

	//Player Stats
	public int MaxHP = 1;
	public int Health = 1;
	public int PlayerClass = 0;
	//0 - default
	//1 - zombie
	//2 - ghost
	//3 - skeleton
	//4 - bat
	//5 - knight
	//6 - dragon
	//7 - demon
	//8 - chaos

	public int MaxHPwatcher;
	public int HealthWatcher;
	public int PlayerClassWatcher;

	//reminder to seperate these away from an export to the litteral thing
	[Export] Sprite2D HUD_text;
	[Export] TextureRect EmptyHeart;
	[Export] TextureRect FullHearts;


	//Zombie Variables
	public bool AvoidDeath = false;
	public float AvoidPrecent = 50f;
	[Export] public Sprite2D QuestionMark;

	//Ghost Variables
	public const float GhostInvinConst = 5;
	public float TimeBetweenGhostPower = GhostInvinConst;

	//Skeleton Variables
	[Export] public PackedScene Bow;

	//Bat Variables
	public const int BatSpeed = 75;

	//Knight Variables
	[Export] public PackedScene Axe;
	public const int KnightSpeed = 30;
	public const int KnightMaxHp = 2;

	//dragon Variables
	public int DragonMaxHP = 2;
	//??? Variables
	public bool EnemyDeletion = false;
	//Chaos Variables
	public bool WalkThroughWalls = false;

	//other things that i couldn't be bothered to organize
	public enum MoveDirection{
		Up,
		Down,
		left,
		right
	}
	public int currentMoveDirection = (int)MoveDirection.Down;

	// game feel related things

	//invulnerbility frames
	public bool Invincible = false;
	public const float InvinConst = 2f;
	public float InvincibilityTimer = InvinConst;
	public Area2D PlayerAreaInstance;
	public float Fdelta;
	public AnimationPlayer EffectAnimation;


	public override void _Ready(){
		EffectAnimation = (AnimationPlayer)GetNode("EffectAnimations");
		Health = MaxHP;
		HealthWatcher = Health;
		MaxHPwatcher = MaxHP;
		PlayerClassWatcher = PlayerClass;
		PlayerClass = globals.playerClass;
		CallClassStuff();
		//InitalizePlayer();
		PlayerAreaInstance = (Area2D)GetNode("PlayerGameCollision");
	}

	public override void _Process(double delta){
		Invincibility();
		ClassSpecficEffects();
		//watcher stuffs
		if(HealthWatcher != Health){
			if (Health < HealthWatcher && HealthWatcher > 1){
				TakeDamage();
			}
			HealthWatcher = Health;
			Debug.Print("Took Damage?");
			FullHearts.Size = new Vector2 (Health * 7, 6);
			PlayerDeath();
		}
		if(MaxHPwatcher != MaxHP){
			MaxHPwatcher = MaxHP;
			EmptyHeart.Size = new Vector2 (MaxHP * 7, 6);
			Debug.Print("Health Upgrade Recived");
		}
		if(PlayerClassWatcher != PlayerClass){
			PlayerClassWatcher = PlayerClass;
			Debug.Print("Switched Classes");
			CallClassStuff();
		}
		if (Input.IsActionJustPressed("ClassUp")){
			PlayerClass += 1;
		}
		else if (Input.IsActionJustPressed("ClassDown")){
			PlayerClass -= 1;
		}


		//shmovement
		//Debug.Print(Health.ToString());
		//Debug.Print(Position.ToString());
		Fdelta = Convert.ToSingle(delta);
		if (canMove){
			if (Input.IsActionJustPressed("SwordSwing")){
				if (canSwing){
					if (PlayerClass == 3){
						var bowinstance = Bow.Instantiate();
						SwordSpawnPoint.AddChild(bowinstance);
						//AddChild(shoot);
						canSwing = false;
						canMove = false;
					}
					else if (PlayerClass == 5){
						var axeinstance = Axe.Instantiate();
						SwordSpawnPoint.AddChild(axeinstance);
						//AddChild(shoot);
						canSwing = false;
						canMove = false;
					}
					else{
						var shoot = Sword.Instantiate();
						SwordSpawnPoint.AddChild(shoot);
						//AddChild(shoot);
						canSwing = false;
						canMove = false;
					}
				}
			}
		}

		if (canSwing != true){
			if (swingtimer > 0){
				swingtimer -= Fdelta;
			}
			else{
				canSwing = true;
				canMove = true;
				swingtimer = constantswingtimer;
			}
		}
		if (currentMoveDirection == (int)MoveDirection.right){
			PlayerAnimations.Play("PlayerRight");
			SwordSpawnPoint.RotationDegrees = 0;
			SwordSpawnPoint.Scale = new Vector2(1,1);
		}
		else if (currentMoveDirection == (int)MoveDirection.left){
			PlayerAnimations.Play("PlayerLeft");
			SwordSpawnPoint.RotationDegrees = 0;
			SwordSpawnPoint.Scale = new Vector2(-1,1);
		}
		else if (currentMoveDirection == (int)MoveDirection.Up){
			PlayerAnimations.Play("PlayerUp");
			SwordSpawnPoint.Scale = new Vector2(1,1);
			SwordSpawnPoint.RotationDegrees = -90;
		}
		else if (currentMoveDirection == (int)MoveDirection.Down){
			PlayerAnimations.Play("PlayerDown");
			SwordSpawnPoint.Scale = new Vector2(1,1);
			SwordSpawnPoint.RotationDegrees = 90;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		//this needs more work honestly
		//Position = new Vector2( (float)Math.Round(Position.X, 0) , (float)Math.Round(Position.Y, 0) );
		// Add the gravity.

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		if (canMove){
			Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
			if (direction != Vector2.Zero)
			{
				// velocity.X = direction.X * Speed;
				// velocity.Y = direction.Y * Speed;
				velocity = new Vector2(direction.X,direction.Y);
				// if (velocity.Length() < 1){
				// 	velocity = velocity.Normalized().Round();
				// }
				//Debug.Print(velocity.Length().ToString());
				//Debug.Print(velocity.Normalized().ToString());
				if (PlayerClass == 4){
					velocity *= BatSpeed;
				}
				else if (PlayerClass == 5){
					velocity *= KnightSpeed;
				}
				else{
					velocity *= Speed;
				}
				//Debug.Print(velocity.ToString());
				
				//Debug.Print(Position.ToString());
				//velocity = new Vector2 ((float)Math.Round(velocity.X * Speed), (float)Math.Round(velocity.Y * Speed));
			}
			else
			{
				velocity.X = 0;
				velocity.Y = 0;
			}

			//animation handler
			if (direction == new Vector2(0,0)){
			//Debug.Print("Idle");
			PlayerAnimations.Stop();
			}
			else if (direction == new Vector2(1,0)){
				currentMoveDirection = (int)MoveDirection.right;
				//Debug.Print("right");
			}
			else if (direction == new Vector2(-1,0)){
				currentMoveDirection = (int)MoveDirection.left;
				//Debug.Print("left");
			}
			else if (direction == new Vector2(0,-1)){
				currentMoveDirection = (int)MoveDirection.Up;
				//Debug.Print("Up");
			}
			else if (direction == new Vector2(0,1)){
				currentMoveDirection = (int)MoveDirection.Down;
				//Debug.Print("Down");
		}
		}
		else
		{
			velocity.X = 0;
			velocity.Y = 0;
			PlayerAnimations.Pause();
		}
		//Debug.Print(direction.ToString());
		Velocity = velocity;
		MoveAndSlide();
	}



	private void _on_player_game_collision_body_entered(Node2D body)
	{
		if (body.IsInGroup("Enemy")){
			Debug.Print(body.ToString());
			globals.playerClass = (int)body.GetParent().Get("EnemyClass");
	   		globals.playerClass = (int)body.Get("EnemyClass");
		}
	}

	private void _on_player_game_collision_area_entered(Area2D area)
	{
		Debug.Print("what i see: " + area.ToString());
		if (area.IsInGroup("Enemy")){
			Debug.Print("player Hit");
			Health -= 1;
			//globals.playerClass = (int)area.GetParent().Get("EnemyClass");
			//globals.playerClass = (int)area.Get("EnemyClass");
			//TakeDamage();
		}
	}

	public void CallClassStuff(){
		HUD_text.RegionRect = new Rect2(0,PlayerClass * 6,38,6);
		SpriteInstance.Texture = textures[PlayerClass];
		if (PlayerClass == 1){
			QuestionMark.Visible = true;
		}
		else{
			QuestionMark.Visible = false;
		}

		if (PlayerClass == 5 || PlayerClass == 6){
			MaxHP = KnightMaxHp;
			Health = KnightMaxHp;
		}
		else{
			MaxHP = 1;
		}
	}

	public void TakeDamage(){
			Debug.Print("ow");
			var oof = (AudioStreamPlayer) GetNode("Ow");
			oof.Play();
			Invincible = true;
			InvincibilityTimer = InvinConst;
	}

	[Export] public PackedScene GameOverScene;
	public void PlayerDeath(){
		if (Health <= 0){
			if (PlayerClass == 1){
				var RandomValue = new Random().Next(0,100);
				if (RandomValue > 50){
					Invincible = true;
					Health = MaxHP;
				}
				else{
					GetTree().ChangeSceneToPacked(GameOverScene);
					Debug.Print("Tough luck");
				}
			}
			else{
				GetTree().ChangeSceneToPacked(GameOverScene);
				Debug.Print("in my medical opinion the heavy is dead!");
			}
			
		}
	}

	public void ClassSpecficEffects(){
		if (PlayerClass == 2){
			if (TimeBetweenGhostPower > 0){
				TimeBetweenGhostPower -= Fdelta;
			}
			else{
				TimeBetweenGhostPower = GhostInvinConst;
				Invincible = true;
			}
		}
	}
	public void Invincibility(){
		if (Invincible){
			if (InvincibilityTimer > 0){
				//Debug.Print(InvincibilityTimer.ToString());
				InvincibilityTimer -= Fdelta;
				PlayerAreaInstance.Monitorable = false;
				PlayerAreaInstance.Monitoring = false;
				EffectAnimation.Play("Invincible");

			}
			else{
				//Debug.Print("Can be hit");
				Invincible = false;
				PlayerAreaInstance.Monitorable = true;
				PlayerAreaInstance.Monitoring = true;
				InvincibilityTimer = InvinConst;
				EffectAnimation.Stop();
			}
				
		}
	}
}
