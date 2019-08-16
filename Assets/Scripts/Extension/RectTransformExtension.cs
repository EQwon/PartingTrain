using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtension
{
    public static void RectTransformReset(this RectTransform _rect)
    {
        _rect.anchoredPosition = new Vector2(0, 0);
    }
}
