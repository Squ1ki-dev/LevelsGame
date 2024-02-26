using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScaleAnimation : MonoBehaviour
{
    private Vector3 startScale;
    [SerializeField] private float speed = 1;
    private void Start()
    {
        startScale = transform.localScale;
        StartAnim();
    }
    private void StartAnim()
    {
        transform.DOScale(startScale * 0.7f, 0.3f * speed)
        .OnComplete(() => transform.DOScale(startScale, 0.3f / speed)
        .OnComplete(() => StartAnim()));
    }
}
