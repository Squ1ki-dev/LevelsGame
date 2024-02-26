using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class ProjectRunner : MonoBehaviour
{
    private void Awake()
    {
        WindowManager.Instance.Show<MainMenuScreen>().Show();
        TinySauce.OnGameStarted();
        if(SoundsManager.sfxVolume == 0) SoundsManager.sfxVolume = 1;
        // GameSession.Instance.StartGame();
    }
}
