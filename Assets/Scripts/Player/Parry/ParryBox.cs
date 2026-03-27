using UnityEngine;

public class ParryBox : MonoBehaviour
{
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
        AbilityDecorator decorator = projectile.GetComponent<AbilityDecorator>();
        decorator.owner = transform.parent.gameObject; // set owner to player for damage attribution
        decorator.OnParry(); // reset lifetime and VFX on the parried projectile
        if (includeLayers != 0 || excludeLayers != 0)
        {
            decorator.abilityCollider.includeLayers = includeLayers;
            decorator.abilityCollider.excludeLayers = excludeLayers;
        }

        Debug.Log("Parried projectile: " + projectile.name);
        GetComponentInParent<PlayerEventController>().OnParry?.Invoke(true); // invoke parry event with success = true
    }
}
