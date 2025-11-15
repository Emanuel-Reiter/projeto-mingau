using UnityEngine;

public class LoadingCanvasSingleton : MonoBehaviour
{
    public static LoadingCanvasSingleton Instance;

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
