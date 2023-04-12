using System;
using System.Collections.Generic;
using System.Linq;
using Godot;


namespace SpikyCube.SceneController
{
    public class PlayerStats : Node
    {
        [Signal]
        public delegate void CoinsAmountChanged(int newAmount);

        [Signal]
        public delegate void KeysAmountChanged(int newAmount);

        // -1 Level was never tried
        public List<List<int>> LevelScores;

        private int _coinsCollected;
        private int _keysCollected;

        public PlayerStats()
        {
            LevelScores = new List<List<int>>(1);
            LevelScores.Add(new List<int>(new int[20]));
        }

        public int CoinsCollected
        {
            get => _coinsCollected;
            set
            {
                EmitSignal("CoinsAmountChanged", value);
                _coinsCollected = value;
            }
        }

        public int KeysCollected
        {
            get => _keysCollected;
            set
            {
                EmitSignal("KeysAmountChanged", value);
                _keysCollected = value;
            }
        }

        public void SaveLevelProgress(int chapterId, int levelId)
        {
            LevelScores[chapterId][levelId] = _coinsCollected;
            CoinsCollected = 0;
            KeysCollected = 0;
            var win = JavaScript.GetInterface("window");
            // win.Call()
            var saveArray = JavaScript.Eval("[" + String.Join(",", LevelScores.Select(g =>
                "[" + String.Join(",", g.Select(i => i.ToString())) + "]"
            )) + "]");
        }

        public void ResetCurrentState()
        {
            KeysCollected = 0;
            CoinsCollected = 0;
        }
    }
}