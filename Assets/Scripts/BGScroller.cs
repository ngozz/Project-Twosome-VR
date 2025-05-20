using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGScroller : MonoBehaviour
{
    [SerializeField] RawImage background;
    [SerializeField] float scrollSpeedX = 0.1f;
    [SerializeField] float scrollSpeedY = 0.1f;

    void Update()
    {
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;
        background.uvRect = new Rect(offsetX, offsetY, 1, 1);
    }
}
