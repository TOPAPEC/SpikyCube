using Godot;
using SpikyCube.SceneController;

public class Flag : KinematicBody2D
{
    [Export] public int open = 1;
    private AnimatedSprite _sprite;
    private Area2D _endBox;
    private Object _playerStatsLink;
    
    public override void _Ready()
    {
        if (open == 0) {
            _sprite = GetNode<AnimatedSprite>("AnimatedSprite");
            _sprite.Animation = "close";
            _playerStatsLink = GetNode<Object>("/root/PlayerStatsExtended");
            _playerStatsLink.Connect("keys_amount_changed", this, "Open");
            _endBox = GetNode<Area2D>("EndBox");
            _endBox.SetCollisionLayerBit(5, false);
        }
    }

    public void Open(int cnt)
    {
        if (cnt > 0) {
            open = 1;
            _sprite.Animation = "idle";
            _endBox.SetCollisionLayerBit(5, true);
        }
    }
}
