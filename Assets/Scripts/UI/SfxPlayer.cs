using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] hits;
    int last = -1;

    public void PlayHit()
    {
        if (hits == null || hits.Length == 0) return;
        int i = Random.Range(0, hits.Length);
        if (i == last && hits.Length > 1) i = (i + 1) % hits.Length;
        last = i;
        source.pitch = Random.Range(.92f, 1.08f);
        source.PlayOneShot(hits[i]);
    }
}
