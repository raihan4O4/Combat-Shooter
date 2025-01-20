using UnityEngine;

public class GunShootController : MonoBehaviour
{

    [Header("Gun data")]
    public Camera Maincamera;
    public float shootRange = 100f;
    public float enemyDamage = 10f;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (Physics.Raycast(Maincamera.transform.position, Maincamera.transform.forward, out RaycastHit hit, shootRange))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
