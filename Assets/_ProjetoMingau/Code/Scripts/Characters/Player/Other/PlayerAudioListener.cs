using UnityEngine;

public class PlayerAudioListener : MonoBehaviour
{
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        transform.rotation = _mainCamera.transform.rotation;
    }
}
