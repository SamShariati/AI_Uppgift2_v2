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

    private float currentHealth
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
        material = GetComponent<MeshRenderer>().material;
        ConstructBehaviorTree();
    }

    private void ConstructBehaviorTree()
    {
        IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(availableCovers, playerTransform, this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private void Update()
    {
        currentHealth += Time.deltaTime * healthRestoreRate;
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
        throw new NotImplementedException();
    }
}
