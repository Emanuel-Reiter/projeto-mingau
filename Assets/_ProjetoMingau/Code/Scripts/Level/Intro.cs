using UnityEngine;
using UnityEngine.Video;

public class Intro : MonoBehaviour
{
    private VideoPlayer _video;
    private int timerIndex;

    private bool _canSkipIntro = false;

    [Header("Params")]
    [SerializeField] private float _startTime = 2.5f;
    [SerializeField] private string _targetLevel;

    private void Start()
    {
        _video = GetComponent<VideoPlayer>();
        float clipDuration = float.Parse(_video.clip.length.ToString());
        clipDuration += 2.0f;

        GlobalTimer.Instance.StartTimer(_startTime, () => {
            _video.Play();
            _canSkipIntro = true;
        });

        timerIndex = GlobalTimer.Instance.StartTimer(clipDuration, () => {
            LevelLoader.Instance.LoadLevel(_targetLevel, () => { }, GameContext.Playing);
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _canSkipIntro) SkipIntro();
    }

    private void SkipIntro()
    {
        GlobalTimer.Instance.CancelTimer(timerIndex);
        LevelLoader.Instance.LoadLevel(_targetLevel, () => { }, GameContext.Playing);
    }
}
