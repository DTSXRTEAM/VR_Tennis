using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
float speed = 40;
    Animator animator;

    public Transform ball;
    public Transform[] targets;

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
        if (ball == null) return;

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
        if (!other.CompareTag("Ball")) return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        BallData data = rb.GetComponent<BallData>();

        Shot currentShot = PickShot();

        Vector3 dir = PickTarget() - transform.position;

        // ✅ Use original spawn speed
        float speedToUse = rb.linearVelocity.magnitude;

        if (data != null && data.initialSpeed > 0)
        {
            speedToUse = data.initialSpeed;
        }

        rb.linearVelocity =
            dir.normalized * speedToUse +
            new Vector3(0, currentShot.upForce, 0);

        // Animation
        Vector3 ballDir = ball.position - transform.position;

        if (ballDir.x >= 0)
            animator.Play("forehand");
        else
            animator.Play("backhand");
    }
}