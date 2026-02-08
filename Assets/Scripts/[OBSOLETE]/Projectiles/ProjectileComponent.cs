// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;

// /// <summary>
// /// A monoBehaviour to provide the behaviour for a generic projectile.
// /// </summary>
// public class ProjectileComponent : MonoBehaviour
// {
//     public string projectileName;
//     public float speed;
//     public float Damage { get; set; }
//     public LayerMask hitLayers;
//     public LayerMask excludeLayers;
//     public GameObject owner;
//     public List<BasicVFXPool> hitEffects;
//     private Collider2D projectileCollider;
//     private Rigidbody2D rb;

//     void OnEnable() // ALWAYS CONFIGURE PROJECTILE BEFORE ENABLING IT
//     {
//         if (!owner || string.IsNullOrEmpty(projectileName) || hitLayers == 0) // check if projectile has been configured properly
//         {
//             Debug.LogError("Projectile is misconfigured.");
//             gameObject.SetActive(false); // set inactive
//         }
//         projectileCollider = GetComponent<Collider2D>();
//         if (!projectileCollider)
//         {
//             Debug.LogError("Projectile has no Collider2D component.");
//             gameObject.SetActive(false); // set inactive
//         }
//         rb = GetComponent<Rigidbody2D>();
//         if (!rb)
//         {
//             Debug.LogError("Projectile has no Rigidbody2D component.");
//             gameObject.SetActive(false); // set inactive
//         }
//         gameObject.name = projectileName;
//         projectileCollider.excludeLayers = excludeLayers;
//         projectileCollider.includeLayers = hitLayers;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         rb.linearVelocity = transform.up * speed; // move projectile forward
//     }

//     void OnCollisionEnter2D(Collision2D other)
//     {
//         if (((1 << other.gameObject.layer) & hitLayers) != 0) // check if collided layer is in hitLayers
//         {
//             // Here you would typically apply damage to the hit object if it has a health component
//             if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
//             {
//                 HitIDamageable(damageable, Damage);
//             }
//             else
//             {
//                 Debug.Log($"no IDamageable found on {other.gameObject.name}");
//             }

//             foreach (var effect in hitEffects)
//             {
//                 GameObject tempEffect = effect.GetPooledObject();
//                 //tempEffect.GetComponent<VFX_Component>().modifier = (speed + Damage) / 10f;
//                 tempEffect.transform.SetPositionAndRotation(transform.position, transform.rotation);
//                 tempEffect.SetActive(true);
//             }
//             gameObject.SetActive(false); // deactivate projectile on hit
//         }
//     }

//     public void HitIDamageable(IDamageable hit, float damage)
//     {
//         hit.ModifyHealth(damage);
//     }

//     void OnDisable() // reset projectile state when deactivated
//     {
//         owner = null; // clear owner reference
//         hitLayers = 0; // reset hit layers
//         speed = 0; // reset speed
//         Damage = 0; // reset damage
//         projectileName = null; // reset name
//         gameObject.name = "Pooled Projectile";
//     }
// }