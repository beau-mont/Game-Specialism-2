using System.Linq;
using UnityEditor;
using UnityEngine;

public class AbiilityUser : MonoBehaviour
{
    [SerializeField]
    private IAbility currentAbility;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string[] abilityGuids = AssetDatabase.FindAssets("t:IAbility");
        if (abilityGuids.Length > 0)
        {
            string abilityPath = AssetDatabase.GUIDToAssetPath(abilityGuids[0]);
            Debug.Log("Loading ability from path: " + abilityPath);
            currentAbility = AssetDatabase.LoadAssetAtPath<IAbility>(abilityPath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentAbility)
            return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentAbility.ActivateAbility(gameObject);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            currentAbility.HoldAbility(gameObject);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            currentAbility.DeactivateAbility(gameObject);
        }
        
    }
}
