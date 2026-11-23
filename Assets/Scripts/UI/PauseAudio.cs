using UnityEngine;
using UnityEngine.Audio;

public class PauseAudio : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioMixerSnapshot normalSnapshot, pausedSnapshot;
    public float transitionTime = .3f;

    void Start()
    {
        mixer.updateMode = AudioMixerUpdateMode.UnscaledTime;
        normalSnapshot.TransitionTo(0f);
    }

    public void SetPaused(bool paused)
    {
        if (paused) pausedSnapshot.TransitionTo(transitionTime);
        else normalSnapshot.TransitionTo(transitionTime);
    }
}
