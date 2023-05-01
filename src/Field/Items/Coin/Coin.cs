using Godot;
using SpikyCube.SceneController;

public class Coin : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Object _playerStats;
    private AudioStreamPlayer _audio_coin;
    private Sprite _sprite;
    private float _bounceInterval = 5.0f;
    private float timePassed = 0.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Connect("body_entered", this, "Collect");
        _playerStats = GetNode<Object>("/root/PlayerStatsExtended");
        _audio_coin = GetNode<AudioStreamPlayer>("CoinAudio");
        _sprite = GetNode<Sprite>("CoinGold48Px");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        timePassed += delta * 5f;
        _sprite.Position = new Vector2(0, Mathf.Lerp(0, Mathf.Sin(timePassed) * _bounceInterval, 0.5f));
    }

    public void Collect(Area2D other)
    {
        CollisionMask = 0;
        _audio_coin.Connect("finished", this, "Clear");
        _audio_coin.Play();
        _playerStats.Call("set_current_coins", (int)_playerStats.Get("current_coins") + 1);
        Hide();
    }

    public void Clear()
    {
        QueueFree();
    }
}
