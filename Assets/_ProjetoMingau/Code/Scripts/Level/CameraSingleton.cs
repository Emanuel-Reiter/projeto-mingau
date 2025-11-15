using UnityEngine;

public class CameraSingleton : MonoBehaviour
{
    public static CameraSingleton Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
