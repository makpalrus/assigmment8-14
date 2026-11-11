using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    int damage;
    LayerMask walls;
    Camera view;

    public void Launch(Vector2 velocity, float lifetime, int amount, LayerMask obstacles, Camera camera)
    {
        damage = amount;
        walls = obstacles;
        view = camera;
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!EnemyBrain.Visible(view, transform.position)) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health != null)
        {
            if (EnemyBrain.Visible(view, health.transform.position)) health.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if ((walls.value & (1 << other.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}
