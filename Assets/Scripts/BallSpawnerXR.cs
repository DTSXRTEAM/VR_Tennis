using UnityEngine;
using UnityEngine.XR;

public class BallSpawnerXR : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;

    public Shot[] shotSequence;
    private int currentShotIndex = 0;

    private GameObject currentBall;

    private InputDevice rightController;
    private bool lastTriggerState = false;

    [Header("UI Images")]
    public GameObject[] shotImages;   // Assign images in order

    void Start()
    {
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // ✅ Hide all images initially
        for (int i = 0; i < shotImages.Length; i++)
        {
            shotImages[i].SetActive(false);
        }
    }

    void Update()
    {
        if (!rightController.isValid)
        {
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        bool pressed;
        if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out pressed))
        {
            if (pressed && !lastTriggerState)
            {
                SpawnBall();
            }

            lastTriggerState = pressed;
        }
    }

    void SpawnBall()
    {
        if (currentBall != null)
        {
            Destroy(currentBall);
        }

        int index = currentShotIndex;

        currentBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);

        Shot shot = GetNextShot();

        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        BallData data = currentBall.GetComponent<BallData>();

        if (rb != null)
        {
            Vector3 dir = spawnPoint.forward;
            float speed = shot.hitForce;

            rb.linearVelocity = dir.normalized * speed + new Vector3(0, shot.upForce, 0);

            if (data != null)
            {
                data.initialSpeed = speed;
            }
        }

        // ✅ IMAGE SWITCH LOGIC
        ShowImage(index);
    }

    void ShowImage(int index)
    {
        for (int i = 0; i < shotImages.Length; i++)
        {
            shotImages[i].SetActive(i == index);
        }
    }

    Shot GetNextShot()
    {
        if (shotSequence.Length == 0)
            return new Shot();

        Shot shot = shotSequence[currentShotIndex];

        currentShotIndex++;

        if (currentShotIndex >= shotSequence.Length)
            currentShotIndex = 0;

        return shot;
    }
}