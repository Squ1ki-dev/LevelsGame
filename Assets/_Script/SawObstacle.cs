using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawObstacle : ObstacleBase
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Candy>())
            onHit.Invoke();
    }
}
