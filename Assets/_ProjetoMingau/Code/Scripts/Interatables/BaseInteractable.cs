using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour
{
    public bool HasBeenInteracted { get; private set; } = false; 

    public abstract void Interact();

    public void SetHasBeenInteracted(bool hasBeenInteracted) { HasBeenInteracted = hasBeenInteracted; }
}
