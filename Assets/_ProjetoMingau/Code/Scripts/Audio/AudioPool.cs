using System.Collections.Generic;
using UnityEngine;

public sealed class AudioPool : Singleton<AudioPool>
{
    [Header("Pool")]
    [SerializeField] private int _initialSize = 16;
    [SerializeField] private int _maxVoices = 64;

    [Header("Defaults")]
    [SerializeField] private AudioMixerGroupRef _mixerGroup;
    [SerializeField] private float _defaultMinDistance = 1f;
    [SerializeField] private float _defaultMaxDistance = 25f;

    private readonly List<PooledVoice> _voices = new();
    private Transform _poolRoot;

    protected override void Awake()
    {
        base.Awake();

        _poolRoot = new GameObject("AudioPool_Voices").transform;
        DontDestroyOnLoad(_poolRoot.gameObject);

        for (int i = 0; i < _initialSize; i++)
            _voices.Add(CreateVoice());
    }

    private PooledVoice CreateVoice()
    {
        var go = new GameObject("Voice");
        go.transform.SetParent(_poolRoot);

        var src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.spatialBlend = 1f;
        src.minDistance = _defaultMinDistance;
        src.maxDistance = _defaultMaxDistance;

        return new PooledVoice(src);
    }

    public static AudioHandle Play(
        AudioClip clip,
        Vector3 position = default,
        bool is3D = true,
        int priority = 128,
        float volume = 1f,
        float pitch = 1f,
        float minDistance = -1f,
        float maxDistance = -1f,
        bool loop = false)
    {
        if (clip == null) return default;
        EnsureInstanceExists();
        return I.PlayInternal(new AudioPlayRequest
        {
            clip = clip,
            position = position,
            is3D = is3D,
            priority = priority,
            volume = volume,
            pitch = pitch,
            minDistance = minDistance,
            maxDistance = maxDistance,
            loop = loop
        });
    }

    public static AudioHandle Play(AudioSfxDef def, Vector3 position = default)
    {
        if (def == null || def.Clip == null) return default;
        return Play(def.Clip, position, def.is3D, def.Priority, def.Volume, def.Pitch, def.MinDistance, def.MaxDistance, def.Loop);
    }

    private static void EnsureInstanceExists()
    {
        if (I != null) return;
        var go = new GameObject("[AudioPool]");
        go.AddComponent<AudioPool>();
    }


    private AudioHandle PlayInternal(AudioPlayRequest req)
    {
        var voice = FindBestVoice(req.priority);
        if (voice == null) return default;

        if (voice.IsPlaying)
            voice.Stop();

        var src = voice.Source;

        src.spatialBlend = req.is3D ? 1f : 0f;
        if (req.is3D)
            src.transform.position = req.position;

        if (req.minDistance >= 0f) src.minDistance = req.minDistance;
        if (req.maxDistance >= 0f) src.maxDistance = req.maxDistance;

        src.clip = req.clip;
        src.volume = Mathf.Clamp01(req.volume);
        src.pitch = Mathf.Clamp(req.pitch, -3f, 3f);
        src.loop = req.loop;
        src.priority = Mathf.Clamp(req.priority, 0, 256);

        voice.Priority = req.priority;
        voice.StartTime = Time.unscaledTime;

        src.Play();

        return new AudioHandle(this, voice);
    }

    private PooledVoice FindBestVoice(int newPriority)
    {
        for (int i = 0; i < _voices.Count; i++)
            if (!_voices[i].IsPlaying)
                return _voices[i];

        if (_voices.Count < _maxVoices)
        {
            var v = CreateVoice();
            _voices.Add(v);
            return v;
        }

        PooledVoice best = null;
        for (int i = 0; i < _voices.Count; i++)
        {
            var v = _voices[i];

            if (best == null)
            {
                best = v;
                continue;
            }

            if (v.Priority < best.Priority) best = v;
            else if (v.Priority == best.Priority && v.StartTime < best.StartTime) best = v;
        }

        if (best != null && newPriority > best.Priority)
            return best;

        return null;
    }

    internal void StopVoice(PooledVoice v)
    {
        if (v == null) return;
        v.Stop();
    }

    private struct AudioPlayRequest
    {
        public AudioClip clip;
        public Vector3 position;
        public bool is3D;
        public int priority;
        public float volume;
        public float pitch;
        public float minDistance;
        public float maxDistance;
        public bool loop;
    }

    internal sealed class PooledVoice
    {
        public AudioSource Source { get; }
        public int Priority { get; set; }
        public float StartTime { get; set; }

        public bool IsPlaying => Source != null && Source.isPlaying;

        public PooledVoice(AudioSource src)
        {
            Source = src;
            Priority = 0;
            StartTime = 0f;
        }

        public void Stop()
        {
            if (Source == null) return;
            Source.Stop();
            Source.clip = null;
            Source.loop = false;
            Priority = 0;
            StartTime = 0f;
        }
    }
}
