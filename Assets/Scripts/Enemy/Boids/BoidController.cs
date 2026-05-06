using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BoidController : MonoBehaviour
{
    // [Header("Global Player Data")]
    // [SerializeField] private PlayerData playerData;
    [Header("Boid Settings")]
    public int suicideThreshold = 5;
    public PlayerData playerData;
    public GameObject followTarget;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float noiseMultiplier = 1f;
    [SerializeField] private float alignmentWeight = 1f;
    [SerializeField] private float cohesionWeight = 1f;
    [SerializeField] private float separationWeight = 1f;
    [SerializeField] private float separationDistance = 1f;
    [Header("Boid Spawning")]
    [SerializeField] private int boidCount = 50;
    [SerializeField] private PooledBoids boidPool;
    [SerializeField] private float spawnRadius;
    [Header("Temporary Variables")]
    private Vector2 targetPosition;
    public List<GameObject> boids;
    public static List<BoidEntity> allBoids;
    public BoidForcefield[] forcefields;
    
    void OnEnable()
    {
        while (boids.Count < boidCount)
        {
            Vector2 spawnPosition = (Vector2)this.transform.position + (Random.insideUnitCircle * spawnRadius);
            GameObject newBoid = boidPool.GetPooledObject();
            newBoid.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            newBoid.GetComponent<BoidEntity>().rb.linearVelocity = Random.insideUnitCircle * maxSpeed;
            newBoid.GetComponent<BoidEntity>().boidSystem = this;
            newBoid.SetActive(true);
            boids.Add(newBoid);
        }
        RefreshBoidList();
        forcefields = FindObjectsByType<BoidForcefield>(FindObjectsSortMode.None);
    }

    void OnDisable()
    {
        var boidsCopy = new List<GameObject>(boids);
        foreach (GameObject boid in boidsCopy)
        {
            boid.SetActive(false);
        }
        var eventController = FindFirstObjectByType<PlayerEventController>();
        if (eventController != null) eventController.OnKill.Invoke();
    }

    void FixedUpdate()
    {
        if (followTarget != null)
            targetPosition = followTarget.transform.position;
        else
            targetPosition = Vector3.zero;

        if (boids.Count <= suicideThreshold)
        {
            targetPosition = playerData.Player.transform.position;
        }
        foreach (GameObject boid in boids)
        {
            EvaluateBoid(boid);
        }

        

        if (boids.Count <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public static void RefreshBoidList()
    {
        allBoids = FindObjectsByType<BoidEntity>(FindObjectsSortMode.None).ToList();
    }

    private void EvaluateBoid(GameObject boid)
    {

        List<BoidEntity> neighbors = boid.GetComponent<BoidEntity>().neighbors;

        Vector2 separationForce = Separation(boid, neighbors);
        Debug.DrawRay(boid.transform.position, separationForce * 50f, Color.red);
        Vector2 alignmentForce = Alignment(boid, neighbors);
        Debug.DrawRay(boid.transform.position, alignmentForce * 50f, Color.green);
        Vector2 cohesionForce = Cohesion(boid, neighbors);
        Debug.DrawRay(boid.transform.position, cohesionForce * 50f, Color.blue);

        BoidEntity boidEntity = boid.GetComponent<BoidEntity>();
        boidEntity.rb.linearVelocity += separationForce + alignmentForce + cohesionForce;
        boidEntity.rb.linearVelocity += ((Mathf.PerlinNoise(boid.transform.position.x, boid.transform.position.y) - 0.5f) * noiseMultiplier * (Vector2)boid.transform.right );
        boidEntity.rb.linearVelocity = Vector2.ClampMagnitude(boidEntity.rb.linearVelocity, maxSpeed);
    }

    private Vector2 Separation(GameObject boid, List<BoidEntity> neighbors)
    {
        Vector2 separationForce = Vector2.zero;
        if (neighbors.Count != 0)
        {
            foreach (BoidEntity neighbor in neighbors)
            {
                if (Vector2.Distance(neighbor.transform.position, boid.transform.position) < separationDistance)
                {
                    float dist = Mathf.Clamp(Vector2.Distance(neighbor.transform.position, boid.transform.position), 0.01f, separationDistance);
                    Vector2 separationDir = boid.transform.position - neighbor.transform.position;
                    separationForce += separationDir.normalized * (separationDistance / dist);
                }
            }
            if (Vector2 .Distance(targetPosition, boid.transform.position) < separationDistance)
            {
                separationForce += (Vector2)boid.transform.position - targetPosition;
                separationForce /= neighbors.Count + 1;
            }
            else
                separationForce /= neighbors.Count;
        }
        return separationWeight * Time.fixedDeltaTime * separationForce;
    }

    private Vector2 Alignment(GameObject boid, List<BoidEntity> neighbors)
    {
        Vector2 avgVelocity = Vector2.zero;
        if (neighbors.Count != 0)
        {
            foreach (BoidEntity neighbor in neighbors)
            {
                avgVelocity += neighbor.rb.linearVelocity; 
            }
            avgVelocity /= neighbors.Count;
        }
        Vector2 alignmentForce = avgVelocity - boid.GetComponent<BoidEntity>().rb.linearVelocity;
        return alignmentWeight * Time.fixedDeltaTime * alignmentForce;
    }

    private Vector2 Cohesion(GameObject boid, List<BoidEntity> neighbors)
    {
        Vector2 COM = Vector2.zero;
        Vector2 boidPos = boid.transform.position;
        if (neighbors.Count != 0)
        {
            foreach (BoidEntity neighbor in neighbors)
            {
                Vector2 localPos = (Vector2)boid.transform.InverseTransformPoint((Vector2)neighbor.transform.position);
                COM += localPos;
            }
            COM /= neighbors.Count;
        }
        Vector2 targetLocalPos = (Vector2)boid.transform.InverseTransformPoint(targetPosition);
        COM += targetLocalPos;
        
        Vector2 worldCOM = (Vector2)boid.transform.TransformPoint(COM);
        COM = worldCOM;

        Vector2 cohesionForce = COM - boidPos;
        return cohesionWeight * Time.fixedDeltaTime * cohesionForce;
    }
}