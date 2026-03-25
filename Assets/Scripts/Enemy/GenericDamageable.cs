using UnityEngine;

public class GenericDamageable : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxHealthModifier;
    [Header("VFX/SFX")]
    public PooledVFX[] damageVFX;
    public PooledSFX[] damageSFX;
    public PooledVFX[] deathVFX;
    public PooledSFX[] deathSFX;
    public PooledVFX[] healVFX;
    public PooledSFX[] healSFX;

    public void Kill()
    {
        SpawnFX(deathVFX, deathSFX);
        gameObject.SetActive(false); 
    }

    public void ModifyHealth(float value)
    {
        health -= value;
        if (value > 0) SpawnFX(damageVFX, damageSFX);
        else if (value < 0) SpawnFX(healVFX, healSFX);
        if (health <= 0) Kill();
        if (health > maxHealth + maxHealthModifier) health = maxHealth + maxHealthModifier;
    }

    public void ModifyMaxHealth(float value)
    {
        maxHealthModifier += value;
    }

    public void ResetMaxHealth()
    {
        maxHealthModifier = 0;
    }

    private void SpawnFX(PooledVFX[] vfxArray, PooledSFX[] sfxArray)
    {
        foreach (var vfx in vfxArray)
        {
            if (vfx == null) continue;
            GameObject temp = vfx.GetPooledObject();
            temp.transform.SetPositionAndRotation(transform.position, transform.rotation);
            temp.SetActive(true);
        }
        foreach (var sfx in sfxArray)
        {
            if (sfx == null) continue;
            GameObject temp = sfx.GetPooledObject();
            temp.transform.SetPositionAndRotation(transform.position, transform.rotation);
            temp.SetActive(true);
        }
    }
}
