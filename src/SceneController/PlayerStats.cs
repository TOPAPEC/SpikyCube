using System;
using System.Collections.Generic;
using Godot;

namespace SpikyCube.SceneController
{
    public class PlayerStats : Node
    {
        [Signal]
        delegate void CoinsAmountChanged(int newAmount);

        [Signal]
        delegate void KeysAmountChanged(int newAmount);
        
        // -1 Level was never tried
        public List<List<int>> LevelScores;

        private int _coinsCollected;
        private int _keysCollected;
        
        public int CoinsCollected
        {
            get => _coinsCollected;
            set {
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
    }
}