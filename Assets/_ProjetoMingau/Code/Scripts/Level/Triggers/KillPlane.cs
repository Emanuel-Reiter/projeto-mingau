using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        AttributesManager attributes = other.GetComponent<AttributesManager>();
        
        if (attributes == null) return;
        
        if(attributes.IsDead) return;

        attributes.TakeDamage(999999);
    }
}
