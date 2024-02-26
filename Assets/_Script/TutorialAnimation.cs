using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tools;
using UnityEngine;

public class TutorialAnimation : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private float lenght, duration;
    private Vector3 startPos;
    private void Start()
    {
        startPos = hand.position;
        StartAnimation();
    }
    private void StartAnimation()
    {
        hand.DOMove(startPos.WithX(startPos.x + lenght), duration / 2).OnComplete(() =>
        hand.DOMove(startPos, duration / 2).OnComplete(() => StartAnimation()));
    }
    public void Stop() => hand.DOKill();
    public void Resume() => StartAnimation();
}
