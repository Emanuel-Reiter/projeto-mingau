using UnityEngine;

public class VFXClouds : MonoBehaviour
{
    [SerializeField] float _cloudSpeed = 20f;

    private void Start()
    {
        _cloudSpeed += Random.Range(-(_cloudSpeed / 10f), (_cloudSpeed / 10f));
    }

    private void Update()
    {
        Vector3 rot = new Vector3(0f, _cloudSpeed, 0f);
        transform.localEulerAngles += rot * Time.deltaTime;
    }
}
