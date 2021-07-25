using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/SteerCohesion")]
public class SteerCohesionBehavior : FilterFlockBehavior
{
    Vector2 currentVelocity;
    public float agentSmoothTime = 0.5f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // If no neighbors; return no adjustment
        if (context.Count == 0) { return Vector2.up; }

        // Add all points together and average
        Vector2 steerCohesionMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        if (filteredContext.Count == 0) { return agent.transform.up; }
        foreach (Transform item in filteredContext)
        {
            steerCohesionMove += (Vector2)item.position;
        }

        steerCohesionMove /= filteredContext.Count;

        //create offset from agent position
        steerCohesionMove -= (Vector2)agent.transform.position;
        steerCohesionMove = Vector2.SmoothDamp(agent.transform.up, steerCohesionMove, ref currentVelocity, agentSmoothTime);
        return steerCohesionMove;
    }
}
