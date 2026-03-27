using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AbilityDecorator combines functionality from a list of AbstractPayload's and an AbstractAbilityBehaviour.
/// It also passes modifiers to them that they can interpret in their own way.
/// </summary>
/// <param name="projectileName">the name of this projectile, only used for debugging.</param>
/// <param name="payloads">an array of ScriptableObject's that inherit AbstractPayload, they describe what to do when hitting something.</param>
/// <param name="damage">the base damage of this ability</param>
/// <param name="abilityBehaviour">a ScriptableObject's that inherits AbstractAbilityBehaviour, this is used to describe how the ability moves.</param>
/// <param name="hitEffects">an array of VFX to spawn when the ability hits something.</param>
/// <param name="payloadMultipliers">a data container that stores multipliers to modify AbstractPayload's behaviour.</param>
/// <param name="AbilityBehaviourMultipliers">a data container that stores multipliers to modify AbstractAbilityBehaviour's behaviour.</param>
/// <param name="owner">the GameObject that fired the ability.</param>
/// <param name="rb">the rigidbody attached to the ability</param>
/// <param name="lifetime">the lifetime of the ability object, the ability object will disable itself after this amount of time has passed. set to zero for unlimited lifetime.</param>
public class AbilityDecorator : MonoBehaviour // GRAAAAAAAH I FUCKING LOVE DOCUMENTATION
{
    [Header("Properties")]
    [SerializeField] private string projectileName;
    [SerializeField] private AbstractPayload[] payloads;
    [SerializeField] private AbstractAbilityBehaviour abilityBehaviour;
    [SerializeField] private PooledVFX[] hitVFX;
    [SerializeField] private bool spawnVFXAtTarget;
    [SerializeField] private bool destroyOnHit;
    [SerializeField] private bool triggerCollider;
    public Collider2D abilityCollider;
    [Header("Modifiers")]
    public PayloadMultipliers payloadMultipliers;
    public AbilityBehaviourMultipliers abilityBehaviourMultipliers;
    [Header("Settings")]
    public GameObject owner;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float lifetime = 1f;
    private float spawnTime;

    public void OnParry()
    {
        spawnTime = Time.time; // reset lifetime on parry
        if (TryGetComponent<VFXComponent>(out var vfx))
        {
            vfx.OnParry(); // reset VFX on the parried object
        }
    }

    void OnEnable()
    {
        spawnTime = Time.time;
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (TryGetComponent<VFXComponent>(out var vfx))
        {
            vfx.multipliers = payloadMultipliers;
        }
    }

    void Update()
    {
        if (abilityBehaviour)
            abilityBehaviour.Multipliers = abilityBehaviourMultipliers;
        if (lifetime != 0 && Time.time > spawnTime + lifetime) gameObject.SetActive(false);
    }

    void OnDisable()
    {
        owner = null;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (triggerCollider) return;
        ProcessHit(other.collider);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ParryBox")) 
        {
            other.GetComponent<ParryBox>().ParryProjectile(gameObject); // parry the projectile if it hits a parry box
            return; // don't trigger on parry boxes
        }
        if (!triggerCollider) return;
        ProcessHit(other);
    }

    public void ProcessHit(Collider2D other)
    {
        if (other.gameObject == owner) return; // don't trigger on the owner of the ability
        foreach (AbstractPayload payload in payloads)
        {
            payload.HitEffect(gameObject, other.gameObject, payloadMultipliers);
        }
        foreach (var effect in hitVFX)
        {
            GameObject tempEffect = effect.GetPooledObject();
            tempEffect.GetComponent<VFXComponent>().multipliers = payloadMultipliers;
            if (spawnVFXAtTarget) tempEffect.transform.SetPositionAndRotation(other.transform.position, transform.rotation);
            else tempEffect.transform.SetPositionAndRotation(transform.position, transform.rotation);
            tempEffect.SetActive(true);
        }
        if (TryGetComponent<IDamageable>(out var damageable))
            damageable.Kill(); // kill
        if (destroyOnHit)
            gameObject.SetActive(false); // deactivate projectile on hit
    }
}
