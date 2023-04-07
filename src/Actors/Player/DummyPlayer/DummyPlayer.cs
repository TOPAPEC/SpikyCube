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
    private bool _died;
    private Vector2 _currentVelocity;
    private AnimatedSprite _playerSprite;
    private Area2D _hitbox;
    private Area2D _rotatebox;
    
    public override void _Ready()
    {
        _playerSprite = GetNode<AnimatedSprite>("PlayerSprite");
        _hitbox = GetNode<Area2D>("Hitbox");
        _hitbox.Connect("area_entered", this, "Die");
        _rotatebox = GetNode<Area2D>("RotateBox");
        _rotatebox.Connect("area_entered", this, "Rotate");
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
        _playerSprite.Animation = "death";
        _died = true;
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
            }    
            else if (Input.IsActionPressed("move_left"))
            {
                _isMoving = true;
                _currentVelocity = Vector2.Left * Speed;
            }
            else if (Input.IsActionPressed("move_down"))
            {
                _isMoving = true;
                _currentVelocity = Vector2.Down * Speed;
            }
            else if (Input.IsActionPressed("move_up"))
            {
                _isMoving = true;
                _currentVelocity = Vector2.Up * Speed;
            }
        }

        if (!(MoveAndSlide(_currentVelocity, Vector2.Up) == _currentVelocity))
        {
            _isMoving = false;
            GD.Print($"{GlobalPosition} -> {GridTranslator.SnapPositionToGrid(GlobalPosition)}");
        }

    }
}
