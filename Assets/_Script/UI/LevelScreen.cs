using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class LevelScreen : WindowBase
{
    [SerializeField] private TextMeshProUGUI levelNumber;
    [SerializeField] private Button replayBtn;
    [SerializeField] private RectTransform starsContainer, starPrefab;
    private SimplePresenter<RectTransform> presenter = new();
    public void Show(LevelModel model)
    {
        replayBtn.OnClick(() =>
        {
            GameSession.Instance.ReloadLevel();
        });
        levelNumber.text = "Level: " + (GameSaves.Instance.LoadCurrentLevel(model.configs.configName) + 1);//(model.levelIdx + 1);
        model.starCountReactive.SubscribeAndInvoke(value => presenter.Present(value, starPrefab, starsContainer));
    }
}
