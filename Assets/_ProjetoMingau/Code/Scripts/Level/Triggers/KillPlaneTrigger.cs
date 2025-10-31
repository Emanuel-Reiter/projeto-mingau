using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlaneTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other != null) {
            if (other.gameObject.CompareTag("Player")) {
                try {
                    Scene scene = SceneManager.GetActiveScene();

                    if (scene != null) {
                        SceneManager.LoadScene(scene.name);
                    }
                }
                catch {
                    Debug.LogError("Error during scene loading!");
                }
            }
        }
    }
}
