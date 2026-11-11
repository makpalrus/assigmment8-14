using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    Vector2 velocity;
    float expires;
    int damage;
    LayerMask walls;
    PlayerHealth target;
    Collider2D targetBody;
    Camera view;

    public void Launch(Vector2 motion, float lifetime, int hitDamage, LayerMask obstacles,
        PlayerHealth player, Collider2D playerBody, Camera camera)
    {
        velocity = motion;
        expires = Time.time + lifetime;
        damage = hitDamage;
        walls = obstacles;
        target = player;
        targetBody = playerBody;
        view = camera;
    }

    void FixedUpdate()
    {
        Vector2 origin = transform.position;
        if (!target || !targetBody || !view || Time.time >= expires || !EnemyBrain.Visible(view, origin))
        {
            Destroy(gameObject);
            return;
        }

        Vector2 step = velocity * Time.fixedDeltaTime;
        RaycastHit2D wall = Physics2D.Linecast(origin, origin + step, walls);
        Bounds body = targetBody.bounds;
        float reach = wall.collider ? wall.distance : step.magnitude;
        var path = new Ray(transform.position, step.normalized);
        if (EnemyBrain.Visible(view, body.center) &&
            (body.Contains(transform.position) || (body.IntersectRay(path, out float distance) && distance < reach)))
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (wall.collider) Destroy(gameObject);
        else transform.position = origin + step;
    }
}
