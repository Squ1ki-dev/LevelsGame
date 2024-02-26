using System;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public Action<Transform> onEnter;
    private void OnCollisionEnter2D(Collision2D other) => onEnter?.Invoke(other.transform);
}
