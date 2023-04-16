using Godot;
using System;

public class CoinsCounter : GridContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private int _coinsCount;
    private TextureRect[] _coins = new TextureRect[3];

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _coins[0] = GetNode<TextureRect>("coin1");
        _coins[1] = GetNode<TextureRect>("coin2");
        _coins[2] = GetNode<TextureRect>("coin3");
    }

    public void ChangeCoinsCount(int newValue)
    {
        GD.Print("INSIDECOIN");
        GD.Print(newValue);
        if (newValue < 0 || newValue > 3)
        {
            newValue = 0;
        }

        for (int i = 0; i < 3; i++)
        {
            GD.Print("ChangedTexture");
            if (i < newValue)
            {
                _coins[i].Texture =
                    (Texture)ResourceLoader.Load(
                        "res://resources/LevelItems/coinGold48px.png");
            }
            else
            {
                _coins[i].Texture =
                    (Texture)ResourceLoader.Load(
                        "res://resources/LevelItems/coinSilver48px.png");
            }
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
