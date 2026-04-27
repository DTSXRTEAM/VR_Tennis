using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotScript : MonoBehaviour
{
    float speed = 40; // moveSpeed
    Animator animator;
    public Transform ball;
    public Transform aimTarget;

    public Transform[] targets;

    public Transform batHitPoint; // 👈 NEW (assign in Inspector)

    float force = 13;
    Vector3 targetPosition;

    ShotManager shotManager;

    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        shotManager = GetComponent<ShotManager>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        targetPosition.x = ball.position.x;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    Vector3 PickTarget()
    {
        int randomValue = Random.Range(0, targets.Length);
        return targets[randomValue].position;
    }

    Shot PickShot()
    {
        int randomValue = Random.Range(0, 2);

        if (randomValue == 0)
            return shotManager.topSpin;
        else
            return shotManager.flat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Shot currentShot = PickShot();

            // 👇 move ball to bat position
            other.transform.position = batHitPoint.position;

            // 👇 direction from bat
            Vector3 dir = PickTarget() - batHitPoint.position;

            other.GetComponent<Rigidbody>().linearVelocity =
                dir.normalized * currentShot.hitForce +
                new Vector3(0, currentShot.upForce, 0);

            Vector3 ballDir = ball.position - transform.position;
        }
    }
}