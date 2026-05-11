using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ParryAbility : MonoBehaviour
{
    public float parryWindow = 0.5f; // Time window for a successful parry
    public float parryCooldown = 1f; // Cooldown time after a parry attempt
    [SerializeField] private GameObject parryBoxInstance;
    private bool isParrying = false;
    private float parryStartTime;
    private float lastParryTime;
    InputAction parryAction;
    [SerializeField] private GameObject parryIndicator;
    private float targetAlpha;
    public float lerpSpeed = 5f;
    private Image image;

    public PooledVFX[] parryVFX;

    void Start()
    {
        image = parryIndicator.GetComponent<Image>();
        parryAction = InputSystem.actions.FindAction("Parry");
    }

    void Update()
    {
        if (parryAction.ReadValue<float>() > 0.5f && Time.time - lastParryTime > parryCooldown)
        {
            StartParry();
        }

        if (isParrying && Time.time - parryStartTime > parryWindow)
        {
            EndParry();
        }

        if (Time.time - lastParryTime > parryCooldown)
        {
            targetAlpha = 1f;
        }
        else
        {
            targetAlpha = 0.5f;
        }

        image.color = new Color(0f, 1f, 1f, Mathf.Lerp(image.color.a, targetAlpha, Time.deltaTime * lerpSpeed));

        parryBoxInstance.transform.position = transform.position; // Keep parry box aligned with player
    }

    void StartParry()
    {
        isParrying = true;
        parryStartTime = Time.time;
        lastParryTime = Time.time;

        parryBoxInstance.SetActive(true); // Enable parry box for collision detection

        foreach (var vfx in parryVFX) // spawn parry vfx
        {
            GameObject temp = vfx.GetPooledObject();
            temp.transform.SetPositionAndRotation(transform.position, transform.rotation);
            temp.SetActive(true);
        }
    }

    void EndParry()
    {
        isParrying = false;
        
        parryBoxInstance.SetActive(false); // Disable parry box after parry window ends
    }
}
