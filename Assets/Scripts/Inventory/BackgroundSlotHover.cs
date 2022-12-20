using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackgroundSlotHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite backgroundSpriteOnHover;
    private Sprite backgroundSpriteOnStopHover;

    private void Start()
    {
        backgroundSpriteOnStopHover = GetComponent<Image>().sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = backgroundSpriteOnHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = backgroundSpriteOnStopHover;
    }
}
