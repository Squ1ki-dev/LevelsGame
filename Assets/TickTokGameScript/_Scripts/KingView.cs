using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KingView : BaseUnitView
{
    [SerializeField] private Animator animator;
    private void Start()
    {
        tutorial.SetActive(true);
    }
    public override void SetFinishState()
    {
        animator.SetTrigger("Finish");
    }
    public override void SetRunState()
    {
        animator.SetTrigger("Run");
    }
    public override void SetIdleState()
    {
        animator.SetTrigger("Idle");
    }
}
