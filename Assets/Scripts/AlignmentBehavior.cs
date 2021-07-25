using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
public class AlignmentBehavior : FilterFlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // If no neighbors; maintain current alignment
        if (context.Count == 0) { return agent.transform.up; }

        // Add all points together and average
        Vector2 alignmentMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        if(filteredContext.Count == 0) { return agent.transform.up; }
        foreach (Transform item in filteredContext)
        {
            alignmentMove += (Vector2)item.transform.up;
        }

        alignmentMove /= filteredContext.Count;

        return alignmentMove;
    }
}
