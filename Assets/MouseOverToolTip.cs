using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite TooltipImage;
    public tooltipController TTC;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TTC.init(TooltipImage);
        Debug.Log("Blarg");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TTC.unload();
    }
}
