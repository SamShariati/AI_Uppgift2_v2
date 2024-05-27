using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[System.Serializable]
public abstract class Node
{
    public NodeState nodeState;

    public abstract NodeState Evaluate();
    
}

public enum NodeState
{
    RUNNING, SUCCESS, FAILURE
}
