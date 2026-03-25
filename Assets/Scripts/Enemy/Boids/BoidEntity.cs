using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoidEntity : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoidController boidSystem;
    public List<BoidEntity> neighbors;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        DamageableList.objects.Add(gameObject);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 diff = rb.linearVelocity.normalized;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        neighbors.RemoveAll(a => a == null || !a.isActiveAndEnabled);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BoidEntity>(out var boid))
        {
            boid.neighbors.Add(this);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BoidEntity>(out var boid))
        {
            boid.neighbors.Remove(this);
        }
    }

    void OnDisable()
    {
        BoidController.allBoids.Remove(this);
        DamageableList.objects.Remove(gameObject);
        boidSystem.boids.Remove(gameObject);
    }
}
