using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tooltipController : MonoBehaviour
{
    public Image img;
    float timer;

    public void init(Sprite s)
    {
        timer = 0;
        img.sprite = s;
        img.preserveAspect = true;
        gameObject.SetActive(true);
    }

    public void unload()
    {
        timer = 0;
        img.sprite = null;
        img.enabled = false;
        gameObject.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.5f)
        {
            img.enabled = true;
            transform.position = Input.mousePosition;
        }

    }
}
