using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoidEntity : MonoBehaviour, IDamageable
{
    public Rigidbody2D rb;
    public BoidController boidSystem;
    [SerializeField] private float maxHealthMod;
    [SerializeField] private float maxHealth;
    [SerializeField] private PooledVFX[] deathVFX;
    public float health;
    public List<BoidEntity> neighbors;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        DamageableList.objects.Add(gameObject);
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth + maxHealthMod;
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

    public void ModifyMaxHealth(float amount)
    {
        maxHealthMod += amount;
    }

    public void ModifyHealth(float amount)
    {
        health -= amount;
        if (health > maxHealth + maxHealthMod) health = maxHealth + maxHealthMod;
        if (health <= 0f) Kill();
    }

    public void ResetMaxHealth()
    {
        
    }

    public void Kill()
    {
        foreach (var effect in deathVFX)
        {
            GameObject tempEffect = effect.GetPooledObject();
            tempEffect.transform.SetPositionAndRotation(transform.position, transform.rotation);
            tempEffect.SetActive(true);
        }
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        BoidController.allBoids.Remove(this);
        DamageableList.objects.Remove(gameObject);
        boidSystem.boids.Remove(gameObject);
    }
}
