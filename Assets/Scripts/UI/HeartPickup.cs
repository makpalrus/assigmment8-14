using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public ItemStats stats;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameUI.Instance.IsPlaying) return;
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health != null && health.Heal(stats.heartHealing))
        {
            GameAudio.Instance.PlayHeal();
            gameObject.SetActive(false);
        }
    }
}
