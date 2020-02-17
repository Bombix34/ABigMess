using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFakeUI : MonoBehaviour
{
    private Material levelMat;

    // Start is called before the first frame update
    void Start()
    {
        levelMat = this.gameObject.GetComponent<Renderer>().material;
        levelMat.SetColor("_EmissionColor", Color.black);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            levelMat.SetColor("_EmissionColor", Color.white);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            levelMat.SetColor("_EmissionColor", Color.black);
        }
    }
}
