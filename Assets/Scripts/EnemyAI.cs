using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float lowHealthThreshhold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Cover[] availableCovers;

    private Material material;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;

    private Node topNode;

    public float _currentHealth = 100;

    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _currentHealth = startingHealth;
        material = GetComponent<MeshRenderer>().material;
        ConstructBehaviorTree();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _currentHealth > 10)
        {
            _currentHealth -= 10f;

        }
        topNode.Evaluate();


        if (topNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
            agent.isStopped = true;
        }

        if (currentHealth < startingHealth)
        {
            _currentHealth += Time.deltaTime * healthRestoreRate;
        }
        
        
    }


    //private void OnMouseDown()
    //{
    //    _currentHealth -= 10f;
    //}


    private void ConstructBehaviorTree()
    {
//--------------------------------ALLA CUSTOM NODES / DVS ALLA NODER SOM �R VARKEN SEQUENCE- ELLER SELECTOR NODES-----------------------------
        IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(availableCovers, playerTransform, this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealthThreshhold);
        IsCoveredNode isCoveredNode = new IsCoveredNode(playerTransform, transform);
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform);
        RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTransform, transform);
        ShootNode shootNode = new ShootNode(agent, this);


        //-------------------------------ALLA SEQUENCE OCH SELECTOR NODES I KORREKT ORDNING (FR�N H�GER GREN TILL V�NSTER GREN, FR�N BOT TILL TOP)---------------

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });

        Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });

        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvailableNode, goToCoverNode });

        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });

        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });

        Sequence coverSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });

        topNode= new Selector(new List<Node> { coverSequence, shootSequence, chaseSequence });

        


    }
    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    

    public void SetColor(Color green)
    {
        material.color = green;
    }

    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        this.bestCoverSpot = bestCoverSpot;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }
}
