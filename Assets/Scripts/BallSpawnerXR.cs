using UnityEngine;
using UnityEngine.XR;

public class BallSpawnerXR : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;

    public Shot[] shotSequence;
    private int currentShotIndex = 0;

    private GameObject currentBall;
    public ShotManager shotManager;

    private InputDevice rightController;
    private bool lastTriggerState = false;

    [Header("UI Images")]
    public GameObject[] shotImages;

    void Start()
    {
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // Hide all images at start
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

            // ✅ FIXED: Proper direction-based velocity
            dir.y += shot.upForce;
            dir = dir.normalized;

            rb.linearVelocity = dir * shot.hitForce;

            // Optional: smoother physics feel
            rb.useGravity = true;

            if (data != null)
            {
                data.initialSpeed = shot.hitForce;
            }
        }

        // UI update
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
        // Alternate between flat and topspin
        if (currentShotIndex % 2 == 0)
        {
            currentShotIndex++;
            return shotManager.flat;
        }
        else
        {
            currentShotIndex++;
            return shotManager.topSpin;
        }
    }
}