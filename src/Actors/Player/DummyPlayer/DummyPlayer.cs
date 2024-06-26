using Godot;
using System;
using System.Threading.Tasks;
using SpikyCube.SceneController;

public class DummyPlayer : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    [Export] public int Speed = 1000;
    [Export] public int Gravity = 500;

    [Signal]
    public delegate void RestartLevel();
    
    [Signal]
    public delegate void NextLevel();

    private const float SwipeSensitivity = 15f;
    // public GridTranslator GridTranslator { get; set; }
    private bool _isMoving;
    private int _dir; // 0=up 1=right 2=down 3=left
    private bool _isMovingForward;
    private bool _died;
    private bool _end;
    private Vector2 _currentVelocity;
    private AnimatedSprite _playerSprite;
    private Area2D _hitbox;
    private Area2D _rotatebox;
    private Area2D _hitboxforenemy;
    private Area2D _attackbox;
    private Area2D _endbox;
    private AudioStreamPlayer _audio_attack;
    private AudioStreamPlayer _audio_death;
    private AudioStreamPlayer _audio_finish;
    private Godot.Object _playerStats;
    
    public override void _Ready()
    {
        _playerSprite = GetNode<AnimatedSprite>("PlayerSprite");
        _rotatebox = GetNode<Area2D>("RotateBox");
        _rotatebox.Connect("area_entered", this, "Rotate");
        _hitboxforenemy = GetNode<Area2D>("HitBoxForEnemy");
        _hitboxforenemy.Connect("area_entered", this, "Die");
        _attackbox = GetNode<Area2D>("AttackBox");
        _attackbox.Connect("area_entered", this, "Attack");
        _playerSprite.Connect("animation_finished", this, "Idle");
        _endbox = GetNode<Area2D>("EndBox");
        _endbox.Connect("area_entered", this, "End");
        _audio_attack = GetNode<AudioStreamPlayer>("AttackAudio");
        _audio_death = GetNode<AudioStreamPlayer>("DeathAudio");
        _audio_finish = GetNode<AudioStreamPlayer>("FinishAudio");
        _playerStats = GetNode<Godot.Object>("/root/PlayerStatsExtended");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void Freee()
    {
        EmitSignal("RestartLevel");
    }
    
    public void Freeee()
    {
        EmitSignal("NextLevel");
    }

    public void End(Area2D other)
    {
        _end = true;
        CollisionMask = 0;
        _playerStats.Call("end_current_level");
        _audio_finish.Play();
        _playerSprite.Animation = "running";
        _playerSprite.Connect("animation_finished", this, "Freeee");;
    }

    public void Die(Area2D other)
    {
        _attackbox.SetCollisionLayerBit(1, false);
        _died = true;
        _playerSprite.Animation = "death";
        _playerSprite.Connect("animation_finished", this, "Freee");
        _audio_death.Play();
    }

    public void Rotate(Area2D other)
    {
        if (other.Name.StartsWith("RotateClockwise"))
        {
            RotationDegrees += 90;
        }
        else
        {
            RotationDegrees -= 90;
        }
        SetMovingForward();
    }

    public void Idle()
    {
        if (_playerSprite.Animation != "idle")
        {
            _playerSprite.Animation = "idle";
        }
    }

    public void Attack(Area2D other)
    {
        if ((_isMovingForward || !_isMoving) && !_died)
        {
            _playerSprite.Animation = "attack";
            _audio_attack.Play();
        }
    }

    public bool MovingForward
    {
        get => _isMovingForward;
    }

    public bool GetAttackPossibility()
    {
        return _isMovingForward || !_isMoving;
    }

    private void SetMovingForward()
    {
        int posRotate = RotationDegrees < 0 ? (int)(RotationDegrees + 360) : (int)(RotationDegrees);
        if ((posRotate == 0 && _dir == 0) ||
            (posRotate == 90 && _dir == 1) ||
            (posRotate == 180 && _dir == 2) ||
            (posRotate == 270 && _dir == 3))
        {
            _isMovingForward = true;
        }
        else
        {
            _isMovingForward = false;
        }

    }

    private void _move(String actionName)
    {
        _isMoving = true;
        switch (actionName)
        {
            case "move_right":
                _dir = 1;
                _currentVelocity = Vector2.Right * Speed;
                break;
            case "move_left":
                _dir = 3;
                _currentVelocity = Vector2.Left * Speed;
                break;
            case "move_down":
                _dir = 2;
                _currentVelocity = Vector2.Down * Speed;
                break;
            case "move_up":
                _dir = 0;
                _currentVelocity = Vector2.Up * Speed;
                break;
        }
        SetMovingForward();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if (!_isMoving & !_died)
        {
            if (@event is InputEventScreenDrag screenDrag)
            {
                var swipe = screenDrag.Relative;
                if (Math.Abs(swipe.y) > SwipeSensitivity && Math.Abs(swipe.y) > Math.Abs(swipe.x))
                {
                    if (swipe.y > 0)
                    {
                        _move("move_down");
                    }
                    else
                    {
                        _move("move_up");
                    }
                }
                else if (Math.Abs(swipe.x) > SwipeSensitivity)
                {
                    if (swipe.x < 0)
                    {
                        _move("move_left");
                    }
                    else
                    {
                        _move("move_right");
                    }
                }
                
            }
            else
            {
                if (@event.IsActionPressed("move_right"))
                {
                    _move("move_right");
                }    
                else if (@event.IsActionPressed("move_left"))
                {
                    _move("move_left");
                }
                else if (@event.IsActionPressed("move_down"))
                {
                    _move("move_down");
                }
                else if (@event.IsActionPressed("move_up"))
                {
                    _move("move_up");
                }
            }
        }       
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (_died || _end) {
            _currentVelocity = new Vector2();
        } else if (!(MoveAndSlide(_currentVelocity, Vector2.Up) == _currentVelocity))
        {
            _isMoving = false;
            _isMovingForward = false;
        }

    }
}
