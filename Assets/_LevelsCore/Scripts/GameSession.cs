using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    MenuScene,
    GameScene,
    Main
}

namespace GameType
{
    public enum GameViewType
    {
        Default,
        Cars,
        BottleGame
    }
}

public class GameSession : Singleton<GameSession>
{
    private int currentLevel = -1;
    private LevelConfigs currentConfigs;

    public void StartGame(LevelConfigs config) => StartGame(config, GameSaves.Instance.LoadCurrentLevel(config.configName));

    public void StartGame(LevelConfigs config, int level)
    {
        if (config == null) return;
        List<LevelConfig> levelsList = config.levels;

        var levelsCount = levelsList.Count;
        if (level > levelsCount - 1)
            level = Random.Range(0, levelsCount);

        GameStartsSetting(level);

        LoadScene(config.sceneName, onComplete: () =>
        {
            Object.FindObjectOfType<GameView>().CreateLevel(config, level);
        });
        currentConfigs = config;
    }

    private void GameStartsSetting(int level)
    {
        Time.timeScale = 1.5f;
        currentLevel = level;
    }

    public void CompleteLevel(LevelModel model)
    {
        Time.timeScale = 1;
        var saves = GameSaves.Instance;
        var currentLastLevel = saves.LoadCurrentLevel(model.configs.configName);
        if (currentLevel >= currentLastLevel) saves.SaveLevel(model.configs.configName, currentLastLevel + 1);//model.starCountReactive.value >= 1 && 

        if (!saves.levelModels.HasIndex(model.levelIdx)) saves.levelModels.Add(model);
        else saves.levelModels[model.levelIdx] = model;
        
        // currentLevel = -1;

        // LoadScene(Scenes.MenuScene);
    }
    public void ReloadLevel() => StartGame(currentConfigs);

    public void LoadScene(string scene, System.Action onComplete = null)
    {
        WindowManager.Instance.CloseAll();
        var operation = SceneManager.LoadSceneAsync(scene);
        if (operation == null)
        {
            Debug.LogError("Scene not exist in build");
            return;
        }
        operation.completed += o => onComplete?.Invoke();
    }
}
