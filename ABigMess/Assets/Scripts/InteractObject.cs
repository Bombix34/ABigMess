using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    [SerializeField]
    Material highlightMaterial;
    Material defaultMat;

    [SerializeField]
    Texture indicator;

    float holdMaterial = 1f;
    // Start is called before the first frame update
    void Start()
    {
        defaultMat = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        holdMaterial -= Time.deltaTime;
        if(holdMaterial <= 0)
        {
            holdMaterial = 1f;
            ResetMaterial();
        }
    }

    public void Highlight()
    {
        gameObject.GetComponent<Renderer>().material = highlightMaterial;
    }

    public void ResetMaterial()
    {
      
        gameObject.GetComponent<Renderer>().material = defaultMat;

    }
}
