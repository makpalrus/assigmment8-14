using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerStats stats;
    public int Hearts { get; private set; }
    bool invincible;
    Vector3 spawn;
    SpriteRenderer picture;
    Player player;
    Rigidbody2D body;

    void Awake()
    {
        Hearts = stats.maxHearts;
        spawn = transform.position;
        picture = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
        body = GetComponent<Rigidbody2D>();
    }

    public void SetSpawn(Vector3 position)
    {
        spawn = position;
    }

    void Update()
    {
        if (GameUI.Instance.IsPlaying && transform.position.y < stats.fallY)
            TakeDamage(stats.hazardDamage);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        TouchHazard(collision.collider);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        TouchHazard(other);
    }

    void TouchHazard(Collider2D other)
    {
        Transform part = other.transform;
        while (part != null)
        {
            if (part.CompareTag("Damage"))
            {
                TakeDamage(stats.hazardDamage);
                return;
            }
            part = part.parent;
        }
    }

    public bool Heal(int amount)
    {
        if (Hearts <= 0 || Hearts >= stats.maxHearts) return false;
        Hearts = Mathf.Min(stats.maxHearts, Hearts + amount);
        GameUI.Instance.RefreshHearts(Hearts);
        return true;
    }

    public void TakeDamage(int amount)
    {
        if (invincible || Hearts <= 0 || !GameUI.Instance.IsPlaying) return;
        Hearts = Mathf.Max(0, Hearts - amount);
        GameUI.Instance.RefreshHearts(Hearts);
        player.ClearInput();
        body.linearVelocity = Vector2.zero;
        transform.position = spawn;
        if (Hearts == 0)
        {
            GameAudio.Instance.PlayDeath();
            GameUI.Instance.ShowGameOver();
        }
        else
        {
            GameAudio.Instance.PlayHurt();
            StartCoroutine(BlinkAfterHit());
        }
    }

    IEnumerator BlinkAfterHit()
    {
        invincible = true;
        float timer = 0f;
        while (timer < stats.invincibilityTime)
        {
            picture.enabled = !picture.enabled;
            yield return new WaitForSeconds(stats.blinkInterval);
            timer += stats.blinkInterval;
        }
        picture.enabled = true;
        invincible = false;
    }

    void OnDisable()
    {
        StopAllCoroutines();
        invincible = false;
        if (picture) picture.enabled = true;
    }
}
