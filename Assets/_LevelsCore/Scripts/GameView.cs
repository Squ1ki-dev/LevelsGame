using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class GameView : MonoBehaviour
{
    public void CreateLevel(LevelConfigs configs, int levelIdx)
    {
        var levels = configs.levels;
        if (!levels.HasIndex(levelIdx))
        {
            levelIdx = levels.Count - 1;
            if(levelIdx == -1) 
            {
                Debug.LogError("Levels not exist");
                return;
            }
        }
        WindowManager.Instance.GetComponent<Canvas>().worldCamera = Camera.main;
        LevelModel model = new LevelModel(configs, levelIdx);
        Instantiate(levels[levelIdx].levelView).Init(model);
    }
}