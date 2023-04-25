using Godot;
using System;

public class LoadingScreen : TextureRect
{
    private AnimatedSprite _crabLoading;
    private AnimatedSprite _loadingDots;

    public override void _Ready()
    {
        _crabLoading = GetNode<AnimatedSprite>("CrabLoading");
        _loadingDots = GetNode<AnimatedSprite>("LoadingDots");
    }

    public void StartLoading()
    {
        Visible = true;
        _crabLoading.Frame = 0;
        _loadingDots.Frame = 0;
        _crabLoading.Play();
        _loadingDots.Play();
    }

    public void StopLoading()
    {
        Visible = false;
        _crabLoading.Stop();
        _loadingDots.Stop();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
