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

        public List<List<int>> LevelScores
        {
            get => LevelScores;
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

        public String SaveVersion
        {
            get => _saveVersion;
        }

        public String LastProfileVersion
        {
            get => "1.0";
        }

        private String _saveVersion;
        private int _coinsCollected;
        private int _keysCollected;
        private int _lastSelectedLevel;
        private List<List<int>> _levelScores;
        private int _playerSkin;
        private List<int> _enemySkins;
        private List<int> _unlockedPlayerSkins;
        private List<int> _unlockedEnemySkins;
        private int _worldSkin;
        private List<int> _unlockedWorldSkins;

        public PlayerStats()
        {
            _levelScores = new List<List<int>>(1);
            _levelScores.Add(new List<int>(new int[20]));
        }

        public bool UpdateProfileVersion()
        {
            throw new NotImplementedException();
        }

        public void CreateBlankProfile()
        {
            throw new NotImplementedException();
        }

        public void LoadProfile()
        {
            
        }

        public void _fillProfile(JavaScriptObject[] returnArgs)
        {
            if (!(returnArgs[0] is null))
            {
                
            }
        }

        public void SaveLevelProgress(int chapterId, int levelId)
        {
            _levelScores[chapterId][levelId] = _coinsCollected;
            CoinsCollected = 0;
            KeysCollected = 0;
        }

        public void ResetCurrentState()
        {
            KeysCollected = 0;
            CoinsCollected = 0;
        }
    }
}
