using UnityEngine;
using UnityEngine.XR;

public class XRTriggerAnimation : MonoBehaviour
{
    public Animator animator;

    private InputDevice leftController;
    private bool triggerPressedLastFrame = false;

    void Start()
    {
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    }

    void Update()
    {
        if (!leftController.isValid)
        {
            leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }

        bool triggerValue;
        if (leftController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue))
        {
            // Detect press (not hold)
            if (triggerValue && !triggerPressedLastFrame)
            {
                animator.ResetTrigger("PlayTrigger"); // important
                animator.SetTrigger("PlayTrigger");
            }

            triggerPressedLastFrame = triggerValue;
        }
    }
}