using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class BallSpawnerXR : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;

    public Shot[] shotSequence;
    private int currentShotIndex = 0;

    private GameObject currentBall;

    private InputDevice rightController;
    private bool lastTriggerState = false;

    [Header("UI")]
    public TextMeshPro infoText;   // Only ONE text
    public string[] shotTexts;         // Values per element

    void Start()
    { 
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // ✅ hide text initially
        if (infoText != null)
            infoText.gameObject.SetActive(false);
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

        int index = currentShotIndex; // store before increment

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

        // ✅ ONLY CHANGE TEXT VALUE
        if (infoText != null && index < shotTexts.Length)
        {
            infoText.gameObject.SetActive(true);   // ✅ enable
            infoText.text = shotTexts[index];      // update value
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