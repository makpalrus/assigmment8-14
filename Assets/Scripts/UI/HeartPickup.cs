using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] Sprite artwork;

    void Awake()
    {
        SpriteRenderer picture = gameObject.AddComponent<SpriteRenderer>();
        picture.sprite = artwork;
        picture.sortingOrder = 7;

        CircleCollider2D contact = gameObject.AddComponent<CircleCollider2D>();
        contact.radius = .3f;
        contact.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        MobileGameUI ui = MobileGameUI.Instance;
        if (!ui || !ui.IsPlaying)
            return;

        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health && health.Heal(1))
            gameObject.SetActive(false);
    }
}
