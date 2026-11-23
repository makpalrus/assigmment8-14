using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-3000)]
public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance;
    public AudioMixer mixer;
    public PauseAudio pauseAudio;
    public AudioSource effects, interfaceSource;
    public SfxPlayer hits;
    public AudioClip jump, land, coin, heal, death, click, unlock, finish, lose;
    public AudioClip patrolWarning, shooterWarning, chargerWarning, strike, shoot, dash;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ApplyVolume();
    }

    public void ApplyVolume()
    {
        SaveData data = SaveManager.Instance.Data;
        mixer.SetFloat("MasterVol", Decibels(data.masterVolume));
        mixer.SetFloat("MusicVol", Decibels(data.musicVolume));
        mixer.SetFloat("SFXVol", Decibels(data.soundVolume));
        mixer.SetFloat("UIVol", Decibels(data.soundVolume));
    }

    float Decibels(float value)
    {
        return Mathf.Log10(Mathf.Max(value, .0001f)) * 20f;
    }

    void PlayEffect(AudioClip clip)
    {
        effects.pitch = Random.Range(.92f, 1.08f);
        effects.PlayOneShot(clip);
    }

    void PlayUI(AudioClip clip)
    {
        interfaceSource.pitch = Random.Range(.92f, 1.08f);
        interfaceSource.PlayOneShot(clip);
    }

    public void PlayJump() { PlayEffect(jump); }
    public void PlayLand() { effects.PlayOneShot(land, .6f); }
    public void PlayCoin() { PlayEffect(coin); }
    public void PlayHeal() { PlayEffect(heal); }
    public void PlayHurt() { hits.PlayHit(); }
    public void PlayDeath() { PlayUI(death); }
    public void Click() { PlayUI(click); }
    public void PlayUnlock() { PlayEffect(unlock); }
    public void PlayFinish() { PlayUI(finish); }
    public void PlayLose() { PlayUI(lose); }
    public void PlayPatrolWarning() { PlayEffect(patrolWarning); }
    public void PlayShooterWarning() { PlayEffect(shooterWarning); }
    public void PlayChargerWarning() { PlayEffect(chargerWarning); }
    public void PlayStrike() { PlayEffect(strike); }
    public void PlayShoot() { PlayEffect(shoot); }
    public void PlayDash() { PlayEffect(dash); }

    public static void SetPaused(bool paused)
    {
        AudioListener.pause = paused;
        Instance.pauseAudio.SetPaused(paused);
    }

    void OnDestroy()
    {
        if (Instance != this) return;
        Instance = null;
        AudioListener.pause = false;
    }
}
