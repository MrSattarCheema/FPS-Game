using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public float keepChasingTime = 5f, minDistanceForChasing = 10f, minDistanceForLose = 15f, distanceToStop = 2f, fireRate;
    private bool isChasing, canFire = true;
    private Vector3 targetPointPosition, startPosition;
    private float chaseTimeCounter, fireCounter, burstLimiter = 0;
    public GameObject bullet;
    public Transform bulletPoint;
    public Animator enemyAnimator;

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
                fireCounter = 1; // wait 1s before starting firing every time when start chasing Player
            }

            if (chaseTimeCounter > 0)
            {
                chaseTimeCounter -= Time.deltaTime;
                if (chaseTimeCounter <= 0)
                {
                    navAgent.destination = startPosition;
                }
            }

            if (navAgent.remainingDistance < 0.25f)
            {
                enemyAnimator.SetBool("isRunning", false);
            }
            else
            {
                enemyAnimator.SetBool("isRunning", true);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, targetPointPosition) > distanceToStop)
            {
                navAgent.destination = targetPointPosition;
                enemyAnimator.SetBool("isRunning", true);
            }
            else
            {
                navAgent.destination = transform.position;
                enemyAnimator.SetBool("isRunning", false);
            }

            if (Vector3.Distance(transform.position, targetPointPosition) > minDistanceForLose)
            {
                isChasing = false;
                chaseTimeCounter = keepChasingTime;
            }

            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0 && canFire && PlayerController.instance.isActiveAndEnabled)
            {
                fireCounter = fireRate;

                bulletPoint.LookAt(targetPointPosition + new Vector3(0f, .5f, 0f));
                Vector3 targetDirection = targetPointPosition - transform.position;
                float angle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);

                if (Mathf.Abs(angle) < 30f)
                {
                    enemyAnimator.SetBool("isRunning", false);
                    navAgent.destination = transform.position;
                    enemyAnimator.SetTrigger("Shoot");
                    Instantiate(bullet, bulletPoint.position, bulletPoint.rotation);
                    burstLimiter++;

                    if (burstLimiter > 5)
                    {
                        canFire = false;
                        StartCoroutine(BurstCooldown());
                    }
                }
                else
                {
                    StartCoroutine(BurstCooldown());
                }
            }
        }
    }

    IEnumerator BurstCooldown()
    {
        yield return new WaitForSeconds(3f);
        enemyAnimator.SetBool("isRunning", true);
        burstLimiter = 0;
        canFire = true;
    }
}
