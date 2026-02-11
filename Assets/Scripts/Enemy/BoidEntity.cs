using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoidEntity : MonoBehaviour, IDamageable
{
    public Rigidbody2D rb;
    public BoidController boidSystem;
    [SerializeField] private float maxHealthMod;
    [SerializeField] private float maxHealth;
    public float health;
    public List<BoidEntity> neighbors;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth + maxHealthMod;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (BoidEntity ent in BoidController.allBoids)
        {
            if (ent != this && Vector2.Distance(transform.position, ent.transform.position) < boidSystem.perceptionRadius)
            {
                neighbors.Add(ent);
            }
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
        BoidController.allBoids.Remove(this);
        gameObject.SetActive(false);
    }
}
