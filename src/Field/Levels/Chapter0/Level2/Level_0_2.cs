using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

public class Level_0_2 : FixedField
{
    private DummyPlayer _player;
    private bool _isTutorial;
    private Godot.Object _playerStats;
    private AnimationPlayer _touchAnimation;
    private Control _tutorial;
    private Area2D[] _tutorialTriggers = new Area2D[0];
    private DestinationArrowSprite[] _destinationArrows = new DestinationArrowSprite[0];
    private void _nextTrigger(PhysicsBody2D body, int currentTrigger)
    {
        GD.Print(body.GetType());
        if (body is DummyPlayer player)
        {
            if (currentTrigger + 1 < _tutorialTriggers.Length)
            {
                _tutorialTriggers[currentTrigger + 1].Visible = true;
                _tutorialTriggers[currentTrigger + 1].Monitoring = true;
                _destinationArrows[currentTrigger + 1].Visible = true;
            }
            _tutorialTriggers[currentTrigger].Visible = false;
            _tutorialTriggers[currentTrigger].SetDeferred("monitoring", false);
            _destinationArrows[currentTrigger].Visible = false;
        }
    }

    public override void _Ready()
    {
        _player = GetNode<DummyPlayer>("DummyPlayer");
        _playerStats = GetNode<Godot.Object>("/root/PlayerStatsExtended");
        for (int triggerNumber = 0; triggerNumber < _tutorialTriggers.Length; ++triggerNumber)
        {
            _tutorialTriggers[triggerNumber] = GetNode<Area2D>($"Tutorial/Trigger{triggerNumber}");
            _destinationArrows[triggerNumber] = GetNode<DestinationArrowSprite>($"Tutorial/DestinationArrow{triggerNumber}");
            _tutorialTriggers[triggerNumber].Connect("body_entered", this, "_nextTrigger", new Array() { triggerNumber });
        }
    }
}

