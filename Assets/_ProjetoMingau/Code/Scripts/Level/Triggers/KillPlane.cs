using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        AttributesManager entity = other.GetComponent<AttributesManager>();
        
        if (entity == null) return;
        if(!entity.IsAlive) return;

        entity.TakeDamage(999999);
    }
}
