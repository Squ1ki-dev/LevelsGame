using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleBase : MonoBehaviour
{
    public UnityEvent onHit;

    private void Awake() {
        onHit.AddListener(Ping);
    }

    public void Ping()
    {
        Debug.Log("Ping");
        GameSession.Instance.ReloadLevel();
    }
}
