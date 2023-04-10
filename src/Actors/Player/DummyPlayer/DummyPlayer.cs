using Godot;
using System;
using System.Threading.Tasks;

public class DummyPlayer : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    [Export] public int Speed = 1000;
    [Export] public int Gravity = 500;
    public GridTranslator GridTranslator { get; set; }
    private bool _isMoving;
    private bool _isMovingForward;
    private bool _died;
    private Vector2 _currentVelocity;
    private AnimatedSprite _playerSprite;
    private Area2D _hitbox;
    private Area2D _rotatebox;
    private Area2D _hitboxforenemy;
    private Area2D _attackbox;
    
    public override void _Ready()
    {
        _playerSprite = GetNode<AnimatedSprite>("PlayerSprite");
        _hitbox = GetNode<Area2D>("Hitbox");
        _hitbox.Connect("area_entered", this, "Die");
        _rotatebox = GetNode<Area2D>("RotateBox");
        _rotatebox.Connect("area_entered", this, "Rotate");
        _hitboxforenemy = GetNode<Area2D>("HitBoxForEnemy");
        _hitboxforenemy.Connect("area_entered", this, "Die");
        _attackbox = GetNode<Area2D>("AttackBox");
        _attackbox.Connect("area_entered", this, "Attack");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void Freee()
    {
        QueueFree();
    }

    public void Die(Area2D other)
    {
        _died = true;
        _playerSprite.Animation = "death";
        _playerSprite.Connect("animation_finished", this, "Freee");
    }

    public void Rotate(Area2D other)
    {
        if (other.Name == "RotateClockwise")
        {
            RotationDegrees += 90;
        }
        else
        {
            RotationDegrees -= 90;
        }
        //GD.Print(RotationDegrees);
    }

    public void Idle()
    {
        GD.Print("check idle");
        _playerSprite.Animation = "idle";
        //_playerSprite.Disconnect("animation_finished");
    }

    public void Attack(Area2D other)
    {
        if (_isMovingForward)
        {
            _playerSprite.Animation = "attack";
            _playerSprite.Connect("animation_finished", this, "Idle");
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (!_isMoving & !_died)
        {
            if (Input.IsActionPressed("move_right"))
            {
                _isMoving = true;
                _currentVelocity = Vector2.Right * Speed;
                if (RotationDegrees == 90)
                {
                    _isMovingForward = true;
                }
                else {
                    _isMovingForward = false;
                }
            }    
            else if (Input.IsActionPressed("move_left"))
            {
                _isMoving = true;
                _currentVelocity = Vector2.Left * Speed;
                if (RotationDegrees == -90)
                {
                    _isMovingForward = true;
                }
                else {
                    _isMovingForward = false;
                }
            }
            else if (Input.IsActionPressed("move_down"))
            {
                _isMoving = true;
                _currentVelocity = Vector2.Down * Speed;
                if (Math.Abs(RotationDegrees) == 180)
                {
                    _isMovingForward = true;
                }
                else {
                    _isMovingForward = false;
                }
            }
            else if (Input.IsActionPressed("move_up"))
            {
                _isMoving = true;
                _currentVelocity = Vector2.Up * Speed;
                if (RotationDegrees == 0)
                {
                    _isMovingForward = true;
                }
                else {
                    _isMovingForward = false;
                }
            }
        }
        if (_died) {
            _currentVelocity = new Vector2();
        } else if (!(MoveAndSlide(_currentVelocity, Vector2.Up) == _currentVelocity))
        {
            _isMoving = false;
        }

    }
}
