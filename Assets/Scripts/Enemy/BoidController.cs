using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BoidController : MonoBehaviour
{
    [Header("Global Player Data")]
    [SerializeField] private PlayerData playerData;
    [Header("Boid Settings")]
    public GameObject followTarget;
    public float maxSpeed = 5f;
    public float noiseMultiplier = 1f;
    public float perceptionRadius = 5f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float separationWeight = 1f;
    public float separationDistance = 1f;
    [Header("Boid Spawning")]
    public int boidCount = 50;
    public PooledBoids boidPool;
    public Vector3 spawnPosition;
    public float spawnRadius;
    [Header("Temporary Variables")]
    public Vector3 targetPosition;
    public List<GameObject> boids;
    public static List<BoidEntity> allBoids;
    public BoidForcefield[] forcefields;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        while (boids.Count < boidCount)
        {
            Vector3 spawnPosition = this.spawnPosition + new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0f) * spawnRadius;
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

    void FixedUpdate()
    {
        
        if (followTarget != null)
            targetPosition = followTarget.transform.position;
        else
            targetPosition = Vector3.zero;
        foreach (GameObject boid in boids)
        {
            EvaluateBoid(boid);
        }

    }

    public static void RefreshBoidList()
    {
        allBoids = FindObjectsByType<BoidEntity>(FindObjectsSortMode.None).ToList();
    }

    private void EvaluateBoid(GameObject boid)
    {
        Vector2 separationForce = Vector2.zero;
        Vector2 alignmentForce = Vector2.zero;
        Vector2 cohesionForce = Vector2.zero;

        List<BoidEntity> neighbors = boid.GetComponent<BoidEntity>().neighbors;

        separationForce = Separation(boid, neighbors);
        Debug.DrawRay(boid.transform.position, new Vector3(separationForce.x, 0f, separationForce.y) * 50f, Color.red);
        alignmentForce = Alignment(boid, neighbors);
        Debug.DrawRay(boid.transform.position, new Vector3(alignmentForce.x, 0f, alignmentForce.y) * 50f, Color.green);
        cohesionForce = Cohesion(boid, neighbors);
        Debug.DrawRay(boid.transform.position, new Vector3(cohesionForce.x, 0f, cohesionForce.y) * 50f, Color.blue);


        BoidEntity boidEntity = boid.GetComponent<BoidEntity>();
        boidEntity.rb.linearVelocity += separationForce + alignmentForce + cohesionForce + ForceField(boid);
        boidEntity.rb.linearVelocity += Random.insideUnitCircle * noiseMultiplier * Time.fixedDeltaTime;
        boidEntity.rb.linearVelocity = Vector2.ClampMagnitude(boidEntity.rb.linearVelocity, maxSpeed);
    }

    private Vector2 ForceField(GameObject boid)
    {
        Vector2 forceFieldForce = Vector2.zero;
        foreach (BoidForcefield field in forcefields)
        {
            float dist = Vector3.Distance(field.transform.position, boid.transform.position);
            if (dist < field.radius)
            {
                Vector3 dir = boid.transform.position - field.transform.position;
                forceFieldForce += new Vector2(dir.x, dir.z).normalized * (field.strength * (field.radius / dist));
            }
        }
        return forceFieldForce * Time.fixedDeltaTime;
    }

    private Vector2 Separation(GameObject boid, List<BoidEntity> neighbors)
    {
        Vector2 separationForce = Vector2.zero;
        if (neighbors.Count != 0)
        {
            foreach (BoidEntity neighbor in neighbors)
            {
                if (Vector3.Distance(neighbor.transform.position, boid.transform.position) < separationDistance)
                {
                    float dist = Mathf.Clamp(Vector3.Distance(neighbor.transform.position, boid.transform.position), 0.01f, separationDistance);
                    Vector3 separationDir = boid.transform.position - neighbor.transform.position;
                    separationForce += new Vector2(separationDir.x, separationDir.z).normalized * (separationDistance / dist);
                }
            }
            if (Vector3 .Distance(targetPosition, boid.transform.position) < separationDistance)
            {
                separationForce += new Vector2(boid.transform.position.x, boid.transform.position.z) - new Vector2(targetPosition.x, targetPosition.z);
                separationForce /= neighbors.Count + 1;
            }
            else
                separationForce /= neighbors.Count;
        }
        return separationWeight * Time.fixedDeltaTime * separationForce;
    }

    private Vector2 Alignment(GameObject boid, List<BoidEntity> neighbors)
    {
        Vector2 avgVelocity = boid.GetComponent<BoidEntity>().rb.linearVelocity;
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
        Vector2 boidPos = new Vector2(boid.transform.position.x, boid.transform.position.z);
        if (neighbors.Count != 0)
        {
            foreach (BoidEntity neighbor in neighbors)
            {
                Vector3 localPos = boid.transform.InverseTransformPoint(neighbor.transform.position);
                COM += new Vector2(localPos.x, localPos.z);
            }
            COM /= neighbors.Count;
        }
        Vector3 targetLocalPos = boid.transform.InverseTransformPoint(targetPosition);
        COM += new Vector2(targetLocalPos.x, targetLocalPos.z);
        
        Vector3 worldCOM = boid.transform.TransformPoint(new Vector3(COM.x, 0f, COM.y));
        COM = new Vector2(worldCOM.x, worldCOM.z);

        Vector2 cohesionForce = COM - boidPos;
        return cohesionWeight * Time.fixedDeltaTime * cohesionForce;
    }
}