using System;
using System.Linq;
using DG.Tweening;
using Tools;
using UnityEngine;

namespace TTGame
{
    public enum UnitState
    {
        Finished,
        Catched,
        Idle
    }
    public class UnitModel
    {
        public UnitModel(WayPointView point)
        {
            currentPoint = point;
        }
        public Connections connections = new();
        public BaseUnitView view;
        public WayPointView currentPoint;
        public EventStream onEndMove = new();
        public UnitState state = UnitState.Idle;
        public const float speed = 2f;
        public bool CanMoveToPoint(WayPointView point) => currentPoint.possiblePoints.Any(p => p == point);
        public float MoveTo(WayPointView point, Action onEnd = null)
        {
            if (currentPoint == point) return 0;
            var yRotation = Quaternion.LookRotation(point.transform.position - view.transform.position).eulerAngles.y;
            var startRotate = view.transform.rotation.eulerAngles;
            var duration = Vector3.Distance(point.transform.position, view.transform.position) / speed;
            
            view.transform.DORotate(new Vector3(startRotate.x, yRotation, startRotate.z), duration * 0.15f).OnComplete(() =>
            {
                view.SetRunState();
                SoundsManager.PlayAudio("DM-CGS-46");
                view.transform.DOMove(point.transform.position, duration * 0.70f)
                    .OnComplete(() =>
                    {
                        view.SetIdleState();
                        currentPoint = point;
                        onEndMove.Invoke();
                        onEnd?.Invoke();
                        view.transform.DORotate(startRotate, duration * 0.15f);
                    });
            });
            return duration * 0.85f;
        }
    }
}
