using UnityEngine;

public class Gun : MonoBehaviour
{
    private Touch touch;
    private Vector2 startPos;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
                startPos = touch.position;  
            if(touch.phase == TouchPhase.Ended)
                Shoot();
        }
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
