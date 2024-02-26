using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class LevelView : BaseLevelView
{
    [SerializeField] private List<StarView> stars;
    [SerializeField] private EndPointView endPoint;
    [SerializeField] private Candy player;
    [SerializeField] private Transform ropes;
    [SerializeField] private ParticleSystem confeti;
    private bool isTryingCompleteLevel = false;

    public override void Init(LevelModel model)
    {
        this.model = model;
        stars.ForEach(s => s.onCatchCallback = () => model.starCountReactive.value++);
        endPoint.onCatchCallback = () =>
        {
            isTryingCompleteLevel = true;
            ropes.SetActive(false);
            this.Wait(1.3f, () =>
            {
                OnWonLevel();
                confeti.Play();
            });
        };
        player.onEnter = enteredObj =>
        {
            if (enteredObj.GetComponent<Floor>())
                this.Wait(1, () => OnLoseLevel());
        };
        WindowManager.Instance.Show<LevelScreen>().Show(model);
    }
    private void FixedUpdate()
    {
        if (!OnScreen(player.transform.position))
        {
            if (!isTryingCompleteLevel)
            {
                isTryingCompleteLevel = true;
                this.Wait(1, () => OnLoseLevel());
            }
        }
    }
    protected override void OnLoseLevel()
    {
        GameSession.Instance.ReloadLevel();
        isTryingCompleteLevel = false;
    }
    private bool OnScreen(Vector3 position)
    {
        var screenPos = Camera.main.WorldToScreenPoint(position);
        var cameraPos = Camera.main.WorldToScreenPoint(Camera.main.transform.position);

        return screenPos.y > 0;
    }
}
