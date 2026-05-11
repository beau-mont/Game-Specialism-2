using System.Linq;
using UnityEngine;

public class ParryBox : MonoBehaviour
{
    [SerializeField] private PooledVFX[] parryVFX;
    [SerializeField] private PooledSFX[] parrySFX;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            ParryProjectile(collision.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            ParryProjectile(collision.gameObject);
        }
    }

    public void ParryProjectile(GameObject projectile)
    {
        LayerMask includeLayers = GetComponentInParent<PlayerAbilityUser>().TargetLayers;
        LayerMask excludeLayers = GetComponentInParent<PlayerAbilityUser>().IgnoreLayers;

        projectile.transform.rotation = projectile.transform.rotation * Quaternion.Euler(0, 0, 180);
        projectile.GetComponent<Rigidbody2D>().linearVelocity *= -1;
        // projectile.tag = "Projectile";
        // projectile.layer = LayerMask.NameToLayer("Projectile");
        if (!projectile.GetComponent<BoidEntity>())
        {
            AbilityDecorator decorator = projectile.GetComponent<AbilityDecorator>();
            decorator.owner = transform.parent.gameObject; // set owner to player for damage attribution
            decorator.OnParry(); // reset lifetime and VFX on the parried projectile
            if (includeLayers != 0 || excludeLayers != 0)
            {
                decorator.abilityCollider.includeLayers = includeLayers;
                decorator.abilityCollider.excludeLayers = excludeLayers;
            }

            foreach(var vfx in parryVFX)
            {
                if (vfx == null) continue;
                GameObject temp = vfx.GetPooledObject();
                temp.transform.SetPositionAndRotation(projectile.transform.position, projectile.transform.rotation);
                temp.SetActive(true);
            }

            if (parrySFX.Count() != 0)
            {
                GameObject tempSFX = parrySFX[Random.Range(0, parrySFX.Count())].GetPooledObject();
                tempSFX.transform.SetPositionAndRotation(transform.position, transform.rotation);
                tempSFX.SetActive(true);
            }

            Debug.Log("Parried projectile: " + projectile.name);
        }
        else
        {
            Debug.Log($"Parried boid {projectile.gameObject.name}");
        }

        GetComponentInParent<PlayerEventController>().OnParry?.Invoke(true); // invoke parry event with success = true
    }
}
