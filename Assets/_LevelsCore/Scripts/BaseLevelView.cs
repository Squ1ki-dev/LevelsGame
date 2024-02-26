using Tools;
using UnityEngine;

public abstract class BaseLevelView : MonoBehaviour
{
    protected LevelModel model;
    public abstract void Init(LevelModel model);
    protected virtual void OnLoseLevel()
    {
        model.isWon = false;
        WindowManager.Instance.Show<EndScreen>().Show(model);
    }
    protected virtual void OnWonLevel()
    {
        model.isWon = true;
        WindowManager.Instance.Show<EndScreen>().Show(model);
    }
}