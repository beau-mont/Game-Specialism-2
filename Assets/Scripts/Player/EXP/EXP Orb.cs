using Unity.Mathematics;
using UnityEngine;

public class EXPOrb : MonoBehaviour
{
    public PlayerData playerData;
    private GameObject player;
    private EXPController controller;
    public int value;
    private float velocity;
    public float startVelocity;
    public float acceleration;
    private float startTime;
    public float startingMagicNumber;
    public float rotationMagicNumber;
    public float reduceMagicNumberBy;
    public PooledSFX pickupSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        rotationMagicNumber = startingMagicNumber;
        startTime = Time.time;
        player = playerData.Player;
        controller = player.GetComponent<EXPController>();
        Vector3 dir = UnityEngine.Random.insideUnitCircle;
        transform.up = dir;
        velocity = UnityEngine.Random.Range(startVelocity / 2, startVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        velocity += acceleration * Time.deltaTime;
        transform.position += Time.deltaTime * velocity * transform.up;

        if (rotationMagicNumber > 1) rotationMagicNumber -= reduceMagicNumberBy * Time.deltaTime;
        else
        {
            transform.up = playerData.Player.transform.position - transform.position;
            return;
        }
        transform.up += (playerData.Player.transform.position - transform.position) / rotationMagicNumber;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            controller.AddEXP(value);
            var sfx = pickupSFX.GetPooledObject();
            sfx.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
