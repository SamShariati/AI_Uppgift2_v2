using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : Node
{
    private EnemyAI ai;
    private float threshhold;



    public HealthNode(EnemyAI ai, float threshhold)
    {
        this.ai = ai;
        this.threshhold = threshhold;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Entered Health Node");
        if (ai.currentHealth < threshhold)
        {

            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
