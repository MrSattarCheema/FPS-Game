using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public float keepChasingTime = 5f, minDistanceForChasing = 10f, minDistanceForLose = 15f, distanceToStop = 2f;
    private bool isChasing;
    private Vector3 targetPointPosition, startPosition;
    private float chaseTimeCounter;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        targetPointPosition = PlayerController.instance.transform.position;
        targetPointPosition.y = transform.position.y;
        if (!isChasing)
        {
            if (Vector3.Distance(transform.position, targetPointPosition) < minDistanceForChasing)
            {
                isChasing = true;
            }
            if (chaseTimeCounter > 0)
            {
                chaseTimeCounter -= Time.deltaTime;
                if (chaseTimeCounter <= 0)
                {
                    navAgent.destination = startPosition;
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, targetPointPosition) > distanceToStop)
            {
                navAgent.destination = targetPointPosition;
            }
            else
            {
                navAgent.destination = transform.position;
            }
            if (Vector3.Distance(transform.position, targetPointPosition) > minDistanceForLose)
            {
                isChasing = false;
                chaseTimeCounter = keepChasingTime;
            }
        }
    }
}
