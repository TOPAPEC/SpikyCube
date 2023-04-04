using Godot;
using System;

public class DummyEnemy : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private float _sinceLastAttack;
    private String _state = "idle";
    private SceneTreeTimer _attackTimer;
    private AnimatedSprite _enemySprite;
    private Position2D _bulletPosition;
    private Area2D _attackRange;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _enemySprite = GetNode<AnimatedSprite>("EnemySprite");
        _bulletPosition = GetNode<Position2D>("BulletPosition");
        _attackRange = GetNode<Area2D>("AttackRange");
        _attackRange.SetPhysicsProcess(false);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (_enemySprite.Animation == "attack" && _enemySprite.Frame == 5)
        {
            _state = "idle";
            _attackRange.SetPhysicsProcess(false);
            _enemySprite.Animation = "idle";
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (_state != "attack")
        {
            _sinceLastAttack += delta;
        }
        else if (_enemySprite.Frame >= 2)
        {
            _attackRange.SetPhysicsProcess(true);
            GD.Print("Monitorable!");
        }
        if (_sinceLastAttack > 1f)
        {
            _sinceLastAttack = 0f;
            Attack();
        }

    }

    public void Attack()
    {
        _state = "attack";
        _enemySprite.Animation = "attack";
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
