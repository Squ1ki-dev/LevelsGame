using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    [SerializeField] private float speed;
    void FixedUpdate()
    {
        transform.Rotate(0, 0, Time.fixedDeltaTime * speed);
    }
}
