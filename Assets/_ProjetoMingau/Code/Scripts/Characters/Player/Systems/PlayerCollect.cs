using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    private PlayerDependencies _deps;

    [Header("Params")]
    [SerializeField] private float _collectRadius = 0.667f;
    [SerializeField] private LayerMask _colletablesLayer;

    [Header("SFX params")]
    [SerializeField] private AudioSfxDef _collectSFX;

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();
    }

    private void Update()
    {
        SerchItems();
    }

    private void SerchItems()
    {
        Vector3 origin = transform.position + (Vector3.up * _collectRadius);
        Vector3 direction = Vector3.zero;

        Collider[] objectsInRange = Physics.OverlapSphere(origin, _collectRadius, _colletablesLayer);

        foreach (Collider obj in objectsInRange)
        {
            BaseCollectable collectable = obj.gameObject.GetComponent<BaseCollectable>();
            collectable.Collect();
            _deps.Inventory.Collectables += collectable.Value;

            
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;

        Vector3 origin = transform.position + (Vector3.up * _collectRadius);
        Gizmos.DrawWireSphere(origin, _collectRadius);
    }
#endif
}
