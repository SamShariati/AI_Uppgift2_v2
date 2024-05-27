using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoveredNode : Node
{

    private Transform target;
    private Transform origin;
    private Color rayColor = Color.red;
    private EnemyAI ai;

    public IsCoveredNode(Transform target, Transform origin, EnemyAI ai)
    {
        this.target = target;
        this.origin = origin;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Entered IsCovered Node");
        RaycastHit hit;
        Vector3 direction= target.position - origin.position;
        float distance = direction.magnitude;

        Debug.DrawRay(origin.position, direction, rayColor);

        if (Physics.Raycast(origin.position, target.position - origin.position, out hit))
        {
            
            if (hit.collider.transform != target)
            {
                ai.SetColor(Color.cyan);
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }
}
