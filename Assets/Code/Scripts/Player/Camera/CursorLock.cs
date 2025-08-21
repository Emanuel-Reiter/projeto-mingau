using UnityEngine;

public class CursorLock : MonoBehaviour {
    private bool _isCursorLocked = false;
    public bool IsCursorLocked => _isCursorLocked;

    private void Start () {
        ToggleCursorLock();
    }

    private void ToggleCursorLock() {
        if (!_isCursorLocked) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        _isCursorLocked = !_isCursorLocked;
    }
}
