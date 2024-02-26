using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : WindowBase
{
    [SerializeField] private Button  labyrinthBtn;//carBtn, bottleBtn, candyBtn,
    [SerializeField] private LevelConfigs  labyrinthLevelConfigs;//carsLevelConfigs, bottleLevelConfigs, candyLevelConfigs,
    public void Show()
    {
        //SubscribeBtn(carBtn, carsLevelConfigs);
        //SubscribeBtn(bottleBtn, bottleLevelConfigs);
        //SubscribeBtn(candyBtn, candyLevelConfigs);
        SubscribeBtn(labyrinthBtn, labyrinthLevelConfigs);

        void SubscribeBtn(Button btn, LevelConfigs configs)
        {
            btn.OnClick(() => GameSession.Instance.StartGame(configs));
        }
    }
}
