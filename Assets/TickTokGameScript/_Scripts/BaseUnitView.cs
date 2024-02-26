using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnitView : MonoBehaviour
{
    public Transform tutorial;
    public virtual void SetFinishState()
    {
    }
    public virtual void SetRunState()
    {
    }
    public virtual void SetIdleState()
    {
    }
}
