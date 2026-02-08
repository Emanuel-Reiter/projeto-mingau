using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public delegate void OnCollectablesChangedDelegate();
    public event OnCollectablesChangedDelegate OnCollectablesChanged;

    private int _collectables;
    public int Collectables
    {
        get => _collectables;
        set
        {
            _collectables = value;
            OnCollectablesChanged?.Invoke();
        }
    }

    [Header("Params")]
    [SerializeField] private float _collectRadius = 0.667f;
    [SerializeField] private LayerMask _colletablesLayer;

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
            Collectables += collectable.Value;
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
