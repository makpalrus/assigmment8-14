using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public AudioClip track;

    void Start()
    {
        GameAudio.SetPaused(false);
        MusicManager.Instance.Play(track);
    }
}
