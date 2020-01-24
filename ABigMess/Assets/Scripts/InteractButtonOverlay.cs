using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractButtonOverlay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    Image image;
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
        this.text.color = Color.white;
    }

    public void SetImage(Sprite image)
    {
        this.image.sprite = image;
    }

    public void SetErrorText(string text)
    {
        this.text.text = text;
        this.text.color = Color.red;
    }
}
