using Events;
using UnityEngine;
using UnityEngine.Events;
using Utils.Scenes;
using Utils.Singleton;

namespace Levels
{
    public class LevelsManager : DontDestroyMonoBehaviourSingleton<LevelsManager>
    {
        public event UnityAction<LevelSettings> LevelSettingsChanged;

        [SerializeField] private LevelSettings[] levelSettings;

        private const string LevelNamePattern = "Level{0}";

        private int _currentLevelIndex;

        private void OnEnable()
        {
            ScenesChanger.SceneLoadedEvent += OnSceneLoaded;
        }

        private void OnDisable()
        {
            ScenesChanger.SceneLoadedEvent -= OnSceneLoaded;
        }

        private void Start()
        {
            ScenesChanger.GotoScene(string.Format(LevelNamePattern, _currentLevelIndex));

            EventsController.Subscribe<EventModels.Game.TargetColorNodesFilled>(this, OnTargetColorNodesFilled);
        }

        private void OnTargetColorNodesFilled(EventModels.Game.TargetColorNodesFilled e)
        {
            _currentLevelIndex += 1;
            ScenesChanger.GotoScene(string.Format(LevelNamePattern, _currentLevelIndex));
        }

        private void OnSceneLoaded()
        {
            if (_currentLevelIndex >= 0 && _currentLevelIndex < levelSettings.Length)
            {
                LevelSettingsChanged?.Invoke(levelSettings[_currentLevelIndex]);
            }
        }
    }
}