using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{
    public string projectileName;
    public float speed;
    public float damage;
    public LayerMask hitLayers;
    public LayerMask excludeLayers;
    public GameObject owner;
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
            Debug.Log($"{projectileName} hit {other.gameObject.name}, dealing {damage} damage.");
            // Here you would typically apply damage to the hit object if it has a health component
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
