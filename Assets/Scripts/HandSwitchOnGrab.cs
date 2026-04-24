using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HandSwitchOnGrab : MonoBehaviour
{
    public GameObject normalRightHand;
    public GameObject batHand;

    private XRGrabInteractable grab;

    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);

        // Initial state
        batHand.SetActive(false);
        normalRightHand.SetActive(true);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        normalRightHand.SetActive(false);
        batHand.SetActive(true);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        normalRightHand.SetActive(true);
        batHand.SetActive(false);
    }
}