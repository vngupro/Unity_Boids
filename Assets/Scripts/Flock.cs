using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;
    [Range(10, 9999)]
    public int startingFlockAgent = 250;                                            // Number of agents at start
    [Range(0.1f, 100f)]
    public float agentDensity = 10f;                                                // Decide the radius of the circle where the agents are going to be created
                                                                                    // Higher Density = Smaller Radius & Less Density = Higher Radius
    [Range(1f, 100f)]
    public float driveFactor = 10f;                                                 // Speed Multiplier
    [Range(1f, 100f)]
    public float maxSpeed = 5f;                                                     // Max Speed of the agent
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;                                             // Radius around the agent considers as neighbors
    [Range(0f, 2f)]
    public float avoidanceRadiusMultiplier = 0.5f;                                  // Multipler of the neighbor radius to know which to avoid

    float squareMaxSpeed;                                                           // Math square of max speed
    float squareNeighborRadius;                                                     // Math square of neighborRadius
    float squareAvoidanceRadius;                                                    // Math square of the avoidance Radius
    public float SquareAvoidanceRadius { get => squareAvoidanceRadius; }
    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for(int i = 0; i < startingFlockAgent; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingFlockAgent / agentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform);
            newAgent.name = "Agent " + i;
            agents.Add(newAgent);

        }
    }

    private void Update()
    {
        foreach(FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            // For Demo
            //agent.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.blue, context.Count / 6f);

            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }

            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
