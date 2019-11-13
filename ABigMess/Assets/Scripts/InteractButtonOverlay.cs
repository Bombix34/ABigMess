using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractButtonOverlay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Awake()
    {
        text.text = Random.Range(1, 99999).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
