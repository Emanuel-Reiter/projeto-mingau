using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float _interactionRadius = 1.0f;
    [SerializeField] private LayerMask _interactableLayer;

    private PlayerDependencies _dependencies;

    private InterationPopup _popup;

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _popup = GameManager.Instance.InterationPopupRef;
    }

    private void Update()
    {
        SerchInteractables();
    }

    private void SerchInteractables()
    {
        Vector3 origin = transform.position + (Vector3.up * _interactionRadius);
        Vector3 direction = Vector3.zero;

        Collider[] objectsInRange = Physics.OverlapSphere(origin, _interactionRadius, _interactableLayer);

        if(objectsInRange.Length > 0 )
        {
            BaseInteractable interactable = objectsInRange[0].GetComponent<BaseInteractable>();

            if (interactable == null) return;
            if (interactable.HasBeenInteracted) return;

            _popup.ShowPopup();

            if (_dependencies.Input.InteractPressed)
            {
                interactable.Interact();
                _popup.Interact();
            }
        }
        else
        {
            _popup.HidePopup();
        }
    }
}
