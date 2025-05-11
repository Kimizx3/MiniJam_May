using System;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    public RawImage img;
    public float x = 0.01f, y = 0.01f;

    private void Update()
    {
        img.uvRect = new Rect(img.uvRect.position + new Vector2(x, y) * Time.deltaTime, img.uvRect.size);
    }
}
