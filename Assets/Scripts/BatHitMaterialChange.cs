using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BatHitMaterialChange : MonoBehaviour
{
    public Transform[] otherBats;     // Assign all bats (B, C, D...)
    public float detectDistance = 0.5f;

    public Material hitMaterial;

    private bool[] changed;

    void Start()
    {
        changed = new bool[otherBats.Length];
    }

    void Update()
    {
        for (int i = 0; i < otherBats.Length; i++)
        {
            if (otherBats[i] == null) continue;

            float dist = Vector3.Distance(transform.position, otherBats[i].position);

            if (dist < detectDistance && !changed[i])
            {
                Renderer r = otherBats[i].GetComponentInChildren<Renderer>();

                if (r != null)
                {
                    r.material = hitMaterial;
                    changed[i] = true;
                }
            }
        }
    }
}