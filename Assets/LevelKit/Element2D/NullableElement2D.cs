using UnityEngine;

public class NullableElement2D : IElement2DDelegate
{
    public void RebuildElement(Element2D element)
    {
        element.Min = Vector2.zero;
        element.Max = Vector2.zero;
    }
}