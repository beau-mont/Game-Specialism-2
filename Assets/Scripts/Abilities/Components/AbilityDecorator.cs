using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AbilityDecorator combines functionality from a list of AbstractPayload's and an AbstractAbilityBehaviour.
/// It also passes modifiers to them that they can interpret in their own way.
/// </summary>
/// <param name="projectileName">the name of this projectile, only used for debugging.</param>
/// <param name="payloads">an array of ScriptableObject's that inherit AbstractPayload, they describe what to do when hitting something.</param>
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
    public AbstractPayload[] payloads;
    public AbstractAbilityBehaviour abilityBehaviour;
    [SerializeField] private PooledVFX[] hitEffects;
    [Header("Modifiers")]
    public PayloadMultipliers payloadMultipliers;
    public AbilityBehaviourMultipliers abilityBehaviourMultipliers;
    [Header("Settings")]
    public GameObject owner;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float lifetime = 1f;
    private float spawnTime;

    void OnEnable()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        if (abilityBehaviour)
            abilityBehaviour.Process(gameObject, rb, abilityBehaviourMultipliers);
        if (lifetime != 0 && Time.time > spawnTime + lifetime) gameObject.SetActive(false);
    }

    void OnDisable()
    {
        owner = null;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Projectile"))
        {
            foreach (AbstractPayload payload in payloads)
            {
                payload.HitEffect(gameObject, other.gameObject, payloadMultipliers);
            }
            foreach (var effect in hitEffects)
            {
                GameObject tempEffect = effect.GetPooledObject();
                tempEffect.GetComponent<VFX_Component>().multipliers = payloadMultipliers;
                tempEffect.transform.SetPositionAndRotation(transform.position, transform.rotation);
                tempEffect.SetActive(true);
            }
            gameObject.SetActive(false); // deactivate projectile on hit
        }            
    }
}
