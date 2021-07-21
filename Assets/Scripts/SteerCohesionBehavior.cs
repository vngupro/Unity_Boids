using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/SteerCohesion")]
public class SteerCohesionBehavior : FlockBehavior
{
    Vector2 currentVelocity;
    public float agentSmoothTime = 0.5f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // If no neighbors; return no adjustment
        if (context.Count == 0) { return Vector2.zero; }

        // Add all points together and average
        Vector2 steerCohesionMove = Vector2.zero;
        foreach (Transform item in context)
        {
            steerCohesionMove += (Vector2)item.position;
        }

        steerCohesionMove /= context.Count;

        //create offset from agent position
        steerCohesionMove -= (Vector2)agent.transform.position;
        steerCohesionMove = Vector2.SmoothDamp(agent.transform.up, steerCohesionMove, ref currentVelocity, agentSmoothTime);
        return steerCohesionMove;
    }
}
