using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerHealth : MonoBehaviour
{
    public int maxHearts { get { return LevelLoader.ActiveData.playerStats.maxHearts; } }
    public float fallY { get { return LevelLoader.ActiveData.playerStats.fallY; } }
    public int Hearts { get; private set; }
    Vector3 spawn;
    float nextDamage;
    SpriteRenderer picture;
    Coroutine blink;
    void Awake()
    {
        Hearts = maxHearts;
        spawn = transform.position;
        picture = GetComponent<SpriteRenderer>();
    }
    public void SetSpawn(Vector3 position)
    {
        spawn = position;
    }

    void Update()
    {
        if (MobileGameUI.Instance && MobileGameUI.Instance.IsPlaying && transform.position.y < fallY) TakeDamage(LevelLoader.ActiveData.playerStats.hazardDamage);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        TouchHazard(collision.collider);
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        TouchHazard(collision.collider);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        TouchHazard(other);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        TouchHazard(other);
    }
    void TouchHazard(Collider2D other)
    {
        for (Transform part = other.transform; part; part = part.parent)
            if (part.CompareTag("Damage"))
            {
                TakeDamage(LevelLoader.ActiveData.playerStats.hazardDamage);
                return;
            }
    }
    public bool Heal(int amount)
    {
        if (amount <= 0 || Hearts <= 0 || Hearts >= maxHearts)
            return false;

        Hearts = Mathf.Min(maxHearts, Hearts + amount);
        MobileGameUI.Instance.RefreshHearts(Hearts);
        return true;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || Time.time < nextDamage || !MobileGameUI.Instance || !MobileGameUI.Instance.IsPlaying) return;
        if (blink != null) StopCoroutine(blink);
        blink = null;
        if (picture) picture.enabled = true;
        Hearts = Mathf.Max(0, Hearts - amount);
        if (Hearts > 0) GameAudio.Play(GameSound.Hurt);
        nextDamage = Time.time + LevelLoader.ActiveData.playerStats.invincibilityTime;
        MobileGameUI.Instance.RefreshHearts(Hearts);
        GetComponent<Player>().ClearInput();
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        transform.position = spawn;
        if (Hearts == 0) MobileGameUI.Instance.ShowGameOver();
        else if (picture) blink = StartCoroutine(BlinkAfterHit());
    }
    IEnumerator BlinkAfterHit()
    {
        while (Time.time < nextDamage)
        {
            picture.enabled = !picture.enabled;
            yield return new WaitForSeconds(LevelLoader.ActiveData.playerStats.blinkInterval);
        }
        picture.enabled = true;
        blink = null;
    }
    void OnDisable()
    {
        if (blink != null) StopCoroutine(blink);
        blink = null;
        if (picture) picture.enabled = true;
    }
}

