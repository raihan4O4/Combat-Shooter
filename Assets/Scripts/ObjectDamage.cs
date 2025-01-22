using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    public float objectHealth = 30f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ObjectHitDamage(float damage)
    {
        objectHealth -= damage;

        if(objectHealth <= 0)
        {
            ObjectDestroy();
        }
    }

    void ObjectDestroy()
    {
        Destroy(gameObject);
    }
}
