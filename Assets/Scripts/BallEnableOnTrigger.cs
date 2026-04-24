using UnityEngine;
using UnityEngine.XR;

public class BallEnableOnTrigger : MonoBehaviour
{
    public GameObject ball;

    private InputDevice rightController;

    void Start()
    {
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        if (!rightController.isValid)
        {
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        bool pressed;
        if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out pressed) && pressed)
        {
            ball.SetActive(true);
        }
    }
}