using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{
    public string projectileName;
    public float speed;
    public float damage;
    public LayerMask hitLayers;
    public LayerMask excludeLayers;
    public GameObject owner;
    public List<BasicVFXPool> hitEffects;
    private Collider2D projectileCollider;
    private Rigidbody2D rb;

    void OnEnable() // ALWAYS CONFIGURE PROJECTILE BEFORE ENABLING IT
    {
        if (!owner || string.IsNullOrEmpty(projectileName) || hitLayers == 0) // check if projectile has been configured properly
        {
            Debug.LogError("Projectile is misconfigured.");
            gameObject.SetActive(false); // set inactive
        }
        projectileCollider = GetComponent<Collider2D>();
        if (!projectileCollider)
        {
            Debug.LogError("Projectile has no Collider2D component.");
            gameObject.SetActive(false); // set inactive
        }
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.LogError("Projectile has no Rigidbody2D component.");
            gameObject.SetActive(false); // set inactive
        }
        gameObject.name = projectileName;
        projectileCollider.excludeLayers = excludeLayers;
        projectileCollider.includeLayers = hitLayers;
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = transform.up * speed; // move projectile forward
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & hitLayers) != 0) // check if collided layer is in hitLayers
        {
            // Here you would typically apply damage to the hit object if it has a health component
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.ModifyHealth(damage);
            }
            else
            {
                Debug.Log($"no IDamageable found on {other.gameObject.name}");
            }

            foreach (var effect in hitEffects)
            {
                GameObject tempEffect = effect.GetPooledObject();
                tempEffect.GetComponent<VFX_Component>().modifier = (speed + damage) / 10f;
                tempEffect.transform.SetPositionAndRotation(transform.position, transform.rotation);
                tempEffect.SetActive(true);
            }
            gameObject.SetActive(false); // deactivate projectile on hit
        }
    }

    void OnDisable() // reset projectile state when deactivated
    {
        owner = null; // clear owner reference
        hitLayers = 0; // reset hit layers
        speed = 0; // reset speed
        damage = 0; // reset damage
        projectileName = null; // reset name
        gameObject.name = "Pooled Projectile";
    }
}

public abstract class IProjectile : MonoBehaviour
{
    [Header("Abstract Properties")]
    public abstract string ProjectileName { get; }
    public abstract float Speed { get; set; }
    public float Damage { get; set; }
    public float LifeTime { get; set; }
    public LayerMask HitLayers { get; set; }
    public LayerMask ExcludeLayers { get; set; }
    public GameObject Owner { get; set; }
    public List<IPooledVFX> HitVFX { get; set; }
}