using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-2900)]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource a, b;
    public float volume = .28f, fadeTime = 1f;
    public AudioSource Current { get; private set; }

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Current = a;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip)
    {
        if (!clip || Current.clip == clip && Current.isPlaying) return;
        StopAllCoroutines();
        AudioSource next = Current == a ? b : a;
        next.Stop();
        next.clip = clip;
        next.volume = 0f;
        next.Play();
        StartCoroutine(Crossfade(Current, next));
        Current = next;
    }

    IEnumerator Crossfade(AudioSource from, AudioSource to)
    {
        float start = from.volume;
        for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / fadeTime)
        {
            from.volume = Mathf.Lerp(start, 0f, t);
            to.volume = Mathf.Lerp(0f, volume, t);
            yield return null;
        }
        from.Stop();
        from.volume = 0f;
        to.volume = volume;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
