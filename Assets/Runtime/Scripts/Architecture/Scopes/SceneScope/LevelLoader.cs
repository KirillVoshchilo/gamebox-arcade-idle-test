using UnityEngine.SceneManagement;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using VContainer;
using VContainer.Unity;

public class LevelLoader
{
    public const string MAIN_SCENE = "StartScene";
    public const string FIRST_LEVEL = "FIrstScene";

    private LifetimeScope _container;
    private LevelStorage _currentLoadedLevel;

    [Inject]
    public void Construct(LifetimeScope lifetimeScope)
    {
        _container = lifetimeScope;
    }

    public async UniTask LoadScene(string scene, Action<LevelStorage> onComplete)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        await UniTask.Yield();
        LevelStorage levelStorage = GameObject.FindAnyObjectByType<LevelStorage>();
        levelStorage.Construct(_container);
        onComplete?.Invoke(levelStorage);
    }
    public void UnloadScene(string scene)
    {
        if (_currentLoadedLevel != null)
            _currentLoadedLevel.Destruct();
        _currentLoadedLevel = null;
        SceneManager.UnloadSceneAsync(scene);
    }
}
