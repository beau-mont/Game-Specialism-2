using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A basic visual effect, when called this will return a copy of the gameobject from its pool.
/// </summary>
[CreateAssetMenu(fileName = "BasicVFX", menuName = "VFX/BasicVFXPool")]
public class BasicVFXPool : PooledVFX
{
    [SerializeField] private GameObject _vfxPrefab;
    public override GameObject VFXPrefab => _vfxPrefab;
    private List<GameObject> _vfxPool;
    protected override List<GameObject> VFXPool { get => _vfxPool; set => _vfxPool = value; }
}