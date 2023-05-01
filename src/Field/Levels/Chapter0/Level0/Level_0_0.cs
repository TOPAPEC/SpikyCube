using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

public class Level_0_0 : FixedField
{
    private DummyPlayer _player;
    private bool _isTutorial;
    private Godot.Object _playerStats;
    private AnimationPlayer _touchAnimation;
    private Control _tutorial;
    private Area2D[] _tutorialTriggers = new Area2D[3];
    private DestinationArrowSprite[] _destinationArrows = new DestinationArrowSprite[3];

    private void _nextTrigger(PhysicsBody2D body, int currentTrigger)
    {
        GD.Print(body.GetType());
        if (body is DummyPlayer player)
        {
            if (currentTrigger + 1 < _tutorialTriggers.Length)
            {
                _tutorialTriggers[currentTrigger + 1].Visible = true;
                _destinationArrows[currentTrigger + 1].Visible = true;
            }
            _tutorialTriggers[currentTrigger].Visible = false;
            _destinationArrows[currentTrigger].Visible = false;
        }
    }

    private void _hideTutorial(String animationName)
    {
        _tutorial.Hide();
    }
    
    public override void _Ready()
    {
        GD.Print("TutorialStarted");
        _tutorial = GetNode<Control>("Control");
        _player = GetNode<DummyPlayer>("DummyPlayer");
        _playerStats = GetNode<Godot.Object>("/root/PlayerStatsExtended");
        _touchAnimation = GetNode<AnimationPlayer>("Control/AnimationPlayer");
        for (int triggerNumber = 0; triggerNumber < _tutorialTriggers.Length; ++triggerNumber)
        {
            _tutorialTriggers[triggerNumber] = GetNode<Area2D>($"Tutorial/Trigger{triggerNumber}");
            _destinationArrows[triggerNumber] = GetNode<DestinationArrowSprite>($"Tutorial/DestinationArrow{triggerNumber}");
        }
        _tutorialTriggers[0].Connect("body_entered", this, "_nextTrigger", new Array() {0});
        _tutorialTriggers[1].Connect("body_entered", this, "_nextTrigger", new Array() {1});
        _tutorialTriggers[2].Connect("body_entered", this, "_nextTrigger", new Array() {2});
        
        _player.Connect("NextLevel", _playerStats, "Call", new Array() {"finish_tutorial"});
        _touchAnimation.CurrentAnimation = "TouchAnimation";
        _touchAnimation.Play("TouchAnimation");
        _touchAnimation.Connect("animation_finished", this, "_hideTutorial");
        GD.Print("Tutorial is set");
    }
}
