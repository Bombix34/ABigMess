using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    [SerializeField]
    Material highlightMaterial;
    Material defaultMat;

    [SerializeField]
    GameObject indicator;
    GameObject indicatorInstance;

    static float HOLD_TIME = 0.125f;
    float holdMaterial = HOLD_TIME;

    bool interacted;

    void Start()
    {
        defaultMat = gameObject.GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (!interacted)
        {
            holdMaterial -= Time.deltaTime;
            if (holdMaterial <= 0)
            {
                holdMaterial = HOLD_TIME;
                ResetMaterial();
            }
        }
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
        if (!interacted)
        {
            holdMaterial = HOLD_TIME;
            if (indicatorInstance == null)
            {
                indicatorInstance = Instantiate(indicator, transform);
                indicatorInstance.transform.Translate(0, transform.localScale.y, 0);
            }
            gameObject.GetComponent<Renderer>().material = highlightMaterial;
        }
    }

    public void ResetMaterial()
    {
        Destroy(indicatorInstance);
        gameObject.GetComponent<Renderer>().material = defaultMat;

    }
}
