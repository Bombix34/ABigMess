using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadScene : MonoBehaviour
{
    private GameManager manager;


    private void Awake()
    {
        manager = this.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
