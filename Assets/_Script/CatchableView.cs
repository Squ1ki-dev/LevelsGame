using System;
using UnityEngine;

public class CatchableView : MonoBehaviour
{
    public Action onCatchCallback;
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<Candy>();
        if (onCatchCallback != null && player)
        {
            onCatchCallback?.Invoke();
            
            OnCatch(player);
        }
    }
    protected virtual void OnCatch(Candy player)
    {
        Destroy(gameObject);
    }
}