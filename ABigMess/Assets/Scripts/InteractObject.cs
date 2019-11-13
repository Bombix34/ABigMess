using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractObject : MonoBehaviour
{
    [SerializeField]
    Material highlightMaterial; // The material used to highlight 

    Material[] materialsToChange;
    Material[] defaultMats;

    [SerializeField]
    Canvas canvas; // The canvas where I display my button overlay 
    [SerializeField]
    GameObject interactButtonOverlayPrefab;
    GameObject interactButtonOverlayInstance;

    static float HOLD_TIME = 0.125f;
    float holdMaterial = HOLD_TIME;

    bool interacted;
    bool childrenHaveMaterials;

    void Start()
    {
        if (materialsToChange == null || materialsToChange.Length == 0)
        {
            materialsToChange = new Material[1];

            if (GetComponent<Renderer>() != null)
            {
                // If material is on parent
                materialsToChange[0] = GetComponent<Renderer>().material;
            }
            else
            {
                // Search for material on children
                if (transform.childCount > 0)
                {
                    materialsToChange = new Material[transform.childCount];

                    for (int i = 0; i < transform.childCount; i++)
                    {
                        materialsToChange[i] = transform.GetChild(i).GetComponent<Renderer>().material;
                    }
                    childrenHaveMaterials = true;
                }

            }

        }
        defaultMats = new Material[materialsToChange.Length];
        for (int i = 0; i < materialsToChange.Length; i++)
        {
            defaultMats[i] = materialsToChange[i];
        }
    }

    void Update()
    {

        holdMaterial -= Time.deltaTime;
        if (holdMaterial <= 0)
        {
            holdMaterial = HOLD_TIME;
            ResetMaterial();
        }

        UpdateOverlayPosition();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Interact();
        }
    }

    public void Interact()
    {
        interacted = true;
        ResetMaterial();
    }

    public void Highlight()
    {

        holdMaterial = HOLD_TIME;
        if (interactButtonOverlayInstance == null)
        {
            interactButtonOverlayInstance = Instantiate(interactButtonOverlayPrefab, canvas.transform);
            interactButtonOverlayInstance.GetComponent<InteractButtonOverlay>().SetText(gameObject.name);
        } else
        {
            interactButtonOverlayInstance.SetActive(true);
        }
        for (int i = 0; i < materialsToChange.Length; i++)
        {
            materialsToChange[i] = highlightMaterial;
        }
        ApplyMaterials();

    }

    public void UpdateOverlayPosition()
    {
        if (interactButtonOverlayInstance != null)
        {
            Vector3 overlayPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, transform.localScale.y, 0));

            interactButtonOverlayInstance.transform.position = overlayPos;
        }
    }

    public void ResetMaterial()
    {
        if (interactButtonOverlayInstance != null)
        {
            interactButtonOverlayInstance.SetActive(false);
        }
        for (int i = 0; i < materialsToChange.Length; i++)
        {
            materialsToChange[i] = defaultMats[i];
            ApplyMaterials();
        }

    }

    // Apply the same highlight material to all the children of an object
    public void ApplyMaterials()
    {
        if (childrenHaveMaterials)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Renderer r = transform.GetChild(i).GetComponent<Renderer>();
                if (r != null)
                {
                    r.material = materialsToChange[i];
                }
            }
        }
        else
        {
            GetComponent<Renderer>().material = materialsToChange[0];
        }
    }
}
