using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinLauncher : MonoBehaviour
{
    public GameObject colliderPumpkin01;

    public GameObject colliderPumpkin02;

    public GameObject colliderPumpkin03;


    private int nbrInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            nbrInput += 1;
        }

        if(nbrInput == 1)
        {
            colliderPumpkin01.SetActive(false);
        }

        else if(nbrInput == 2)
        {
            colliderPumpkin02.SetActive(false);
        }

        else if (nbrInput == 3)
        {
            colliderPumpkin03.SetActive(false);
        }
    }
}
