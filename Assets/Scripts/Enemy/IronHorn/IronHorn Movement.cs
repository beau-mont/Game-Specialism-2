using UnityEngine;

public class IronHornMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerData playerData;
    private GameObject player;
    public float speedMult = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!playerData)
        {
            Debug.LogError($"playerdata not provided on object {this.gameObject.name}");
            return;
        }
        player = playerData.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) return;

        float playerX = player.transform.position.x;
        float ironHornX = transform.position.x;
        rb.linearVelocityX = (playerX - ironHornX) * speedMult;
    }
}
