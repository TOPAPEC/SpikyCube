using Godot;
using System;
using SpikyCube.SceneController;

public class Coin : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private PlayerStats _ps;
    private AudioStreamPlayer _audio_coin;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Connect("body_entered", this, "Collect");
        _ps = GetNode<PlayerStats>("/root/PlayerStats");
        _audio_coin = GetNode<AudioStreamPlayer>("CoinAudio");
        
    }

    public void Collect(Area2D other)
    {
        CollisionMask = 0;
        _audio_coin.Connect("finished", this, "Freee");
        _audio_coin.Play();
        _ps.CoinsCollected += 1;
        Hide();
    }

    public void Freee()
    {
        QueueFree();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
