using UnityEngine;

public class GroupElement2D : IElement2DDelegate
{
    public void RebuildElement(Element2D element)
    {
        RebuildAllChildElements(element);
        UpdateBounds(element);
    }

    public void RebuildAllChildElements(Element2D element)
    {
        foreach (Element2D childElement in element.ChildElements)
        {
            if (childElement != null)
                childElement.Rebuild(element);
        }
    }

    public void UpdateBounds(Element2D element)
    {
        if (element.ChildElements.Count == 0)
        {
            element.Min = Vector2.zero;
            element.Max = Vector2.zero;
            return;
        }

        element.Min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        element.Max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

        foreach (Element2D childElement in element.ChildElements)
        {
            if (childElement == null) continue;

            Vector2 childElementMin, childElementMax;
            childElementMin = childElement.Min +
                new Vector2(childElement.transform.localPosition.x, childElement.transform.localPosition.y);
            childElementMax = childElement.Max +
                new Vector2(childElement.transform.localPosition.x, childElement.transform.localPosition.y);

            if (childElementMin.x < element.Min.x) element.Min.x = childElementMin.x;
            if (childElementMin.y < element.Min.y) element.Min.y = childElementMin.y;
            if (childElementMax.x > element.Max.x) element.Max.x = childElementMax.x;
            if (childElementMax.y > element.Max.y) element.Max.y = childElementMax.y;
        }
    }
}
