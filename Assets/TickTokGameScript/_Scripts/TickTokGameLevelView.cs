using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Tools;
using Tools.Reactive;
using TTGame;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using CAS.AdObject;

namespace TTGame
{
    public class TickTokGameLevelView : BaseLevelView
    {
        private List<WayPointView> points = new();
        private List<UnitModel> playerUnits = new();
        private List<EnemyAI> enemies = new();
        [SerializeField] private WayPointView endPoint;
        [SerializeField] private Material inTargeting, simple;
        [SerializeField] private Transform endView;
        [SerializeField] private BaseUnitView unitViewPrefab, enemyViewPrefab, kingViewPrefab;
        [SerializeField] private WayPointView playerKingStartPoint;
        [SerializeField] private List<WayPointView> playerHelpStartPoints;
        [SerializeField] private List<WayPointView> enemyStartPoints;
        [SerializeField] private ParticleSystem finishEffect;
        private UnitModel unitInTargeting;
        private Reactive<bool> isPlayerTurn = new Reactive<bool>(true);
        private List<UnitModel> freeUnits = new List<UnitModel>();
        private UnitModel playerKing;
        private bool gameFinished = false;//=> playerKing.state == UnitState.Finished || playerKing.state == UnitState.Catched;
        FieldModel gameModel;

        [SerializeField] private GameObject interstitialObject;
        public override void Init(LevelModel model)
        {
            this.model = model;
            points = GetComponentsInChildren<WayPointView>().ToList();
            points.ForEach(p =>
            {
                p.Init();
                p.DrawWays(simple);
                p.clickStream.Subscribe(() => ClickOnPlayer(p));
            });

            InitEnemies();
            InitUnits();

            isPlayerTurn.SubscribeAndInvoke(value =>
            {
                CheckResult();
                if (gameFinished) return;
                if (!value)
                {
                    freeUnits.Clear();

                    var e = enemies.GetRandom(e => e.CanCatchPlayer());
                    e ??= enemies.GetRandom();
                    this.Wait(e.Step() * 0.3f, () => isPlayerTurn.value = true);
                    var catched = playerUnits.Find(u => u.currentPoint == e.unit.currentPoint);
                    if (catched != null)
                    {
                        CatchPlayerUnit(catched);
                        return;
                    }

                    CheckResult();
                }
                else
                {
                    freeUnits.Clear();
                    freeUnits.AddRange(playerUnits.FindAll(u => u.state == UnitState.Idle));
                }
            });
            gameModel = new FieldModel(points.Select(w => w.model).ToList());
        }
        private void CatchPlayerUnit(UnitModel unit)
        {
            unit.state = UnitState.Catched;
            unit.currentPoint = null;
            Destroy(unit.view.gameObject);
        }
        private void InitEnemies()
        {
            for (int i = 0; i < enemyStartPoints.Count; i++)
            {
                var unit = new UnitModel(enemyStartPoints[i]);
                SpawnUnitView(unit, enemyViewPrefab);
                var enemy = new EnemyAI(unit, playerUnits, endPoint, points);

                enemies.Add(enemy);
            }
            enemies.ForEach(e => e.SetOtherEnemis(enemies.Select(e => e.unit).ToList()));
        }
        private bool playerIsMoving = false;
        private void ClickOnPlayer(WayPointView p)
        {
            if (!isPlayerTurn.value || playerIsMoving) return;
            var currUnit = playerUnits.Find(u => u.currentPoint == p);
            unitInTargeting?.currentPoint?.HighlightAllPatchs(simple, 0);
            if (currUnit != null)
            {
                unitInTargeting = currUnit;
                playerKing.view.GetComponent<KingView>().tutorial.SetActive(false);
            }
            if (unitInTargeting != null && unitInTargeting.CanMoveToPoint(p))
            {
                playerIsMoving = true;
                unitInTargeting.MoveTo(p, () =>
                {
                    playerIsMoving = false;
                    isPlayerTurn.value = false;
                    if (p == endPoint && p == playerKing.currentPoint) PlayerUnitOnFinish(unitInTargeting);
                    CheckResult();
                    unitInTargeting = null;
                });
                if (p == endPoint) OnStartMoveToFinish();
                freeUnits.Remove(unitInTargeting);
            }
            else
            {
                unitInTargeting = playerUnits.Find(u => u.currentPoint == p);
                unitInTargeting?.currentPoint?.HighlightAllPatchs(inTargeting, 10, true);
            }
            bool isInTarget = unitInTargeting != null && unitInTargeting.currentPoint == p;
            if (isInTarget)
                p.HighlightAllPatchs(inTargeting, 10, true);
            else
                p.HighlightAllPatchs(simple, 0);
        }
        private void PlayerUnitOnFinish(UnitModel unit)
        {
            unit.currentPoint = null;
            unit.connections.DisconnectAll();
            unitInTargeting = null;
            unit.state = UnitState.Finished;
            unit.view.GetComponent<KingView>().SetFinishState();
            CheckResult();
        }
        private void InitUnits()
        {
            playerHelpStartPoints.ForEach(p =>
                {
                    playerUnits.Add(new UnitModel(p));
                });
            playerUnits.ForEach(u =>
            {
                SpawnUnitView(u, unitViewPrefab);
            });
            playerKing = new UnitModel(playerKingStartPoint);
            playerUnits.Add(playerKing);
            SpawnUnitView(playerKing, kingViewPrefab);
        }
        private void OnStartMoveToFinish()
        {
            endView.DOMoveY(endView.position.y - 5, 1f).OnComplete(() => finishEffect?.Play());
        }
        private void SpawnUnitView(UnitModel unit, BaseUnitView viewPrefab)
        {
            var view = Instantiate(viewPrefab, unit.currentPoint.transform);
            view.transform.SetParent(null);
            unit.view = view;
            unit.onEndMove.Subscribe(CheckResult);
        }
        private void CheckResult()
        {
            if (gameFinished) return;
            playerUnits.ForEach(pu =>
            {
                if (enemies.Any(e => e.unit.currentPoint == pu.currentPoint)) CatchPlayerUnit(pu);
            });
            if (playerKing.state == UnitState.Catched)
            {
                OnLoseLevel();
                SoundsManager.PlayAudio("Lose_Game_02");
            }
            if (playerKing.state == UnitState.Finished)
            {
                OnWonLevel();
                SoundsManager.PlayAudio("Complete_Level_01");
            }
            if (playerKing.state == UnitState.Catched || playerKing.state == UnitState.Finished)
            {
                if (RandomTools.GetChance(75))
                {
                    InterstitialAdObject interstitial = interstitialObject.GetComponent<InterstitialAdObject>();
                    interstitial.Present();
                }
                gameFinished = true;
            }
        }
        private void OnDrawGizmos()
        {
            var points = GetComponentsInChildren<WayPointView>().ToList();
            points.ForEach(p =>
            {
                p.ways.RemoveAll(w => w == null);
                foreach (var w in p.ways)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(p.transform.position, w.transform.position);
                }
            });
        }
    }
}
public class WayPointModel
{
    public WayPointModel()
    {

    }
    public WayPointModel(UnitModel unit, List<WayPointModel> wayPointModels)
    {
        possiblePoints = wayPointModels;
        unitModel = unit;
    }
    public List<WayPointModel> possiblePoints;
    public UnitModel unitModel;
}
public class FieldModel
{
    List<WayPointModel> wayPoints;
    public FieldModel(List<WayPointModel> wayPoints)
    {
        this.wayPoints = wayPoints;
    }
    public bool CanMove(UnitModel unit, WayPointModel target) => !(unit == null || target == null || target.unitModel != null);
    public void MoveTo(UnitModel unit, WayPointModel target)
    {
        if (!CanMove(unit, target)) return;
        wayPoints.Find(w => w.unitModel == null).unitModel = null;
        target.unitModel = unit;
    }
}
public class EnemyAI
{
    public UnitModel unit { private set; get; }
    private List<WayPointView> points = new();
    private List<UnitModel> playerUnits = new();
    private List<UnitModel> enemyUnits = new();
    private WayPointView endPoint;
    public EnemyAI(UnitModel enemyUnit, List<UnitModel> targets, WayPointView end, List<WayPointView> allPoints)
    {
        unit = enemyUnit;
        playerUnits = targets;
        points = allPoints;
        endPoint = end;
    }
    public void SetOtherEnemis(List<UnitModel> otherEnemies)
    {
        enemyUnits = otherEnemies;
    }
    public bool CanCatchPlayer() => unit.currentPoint.possiblePoints.Find(p => playerUnits.Any(u => u.currentPoint == p));
    public float Step()
    {
        WayPointView targetPoint = unit.currentPoint.possiblePoints.Find(p => playerUnits.Any(u => u.currentPoint == p));

        if (targetPoint == null) targetPoint = unit.currentPoint.possiblePoints.GetRandom(p =>
            p != endPoint && !enemyUnits.Any(e => e.currentPoint == p));
        if (targetPoint == null) unit.onEndMove.Invoke();

        return unit.MoveTo(targetPoint);
    }
}
