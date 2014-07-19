using UnityEngine;

public class MeshElement2D : IElement2DDelegate
{
    public void RebuildElement(Element2D element)
    {
        Renderer renderer = element.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Bounds bounds = renderer.bounds;
            element.Min = bounds.center - element.transform.position - bounds.size / 2;
            element.Max = bounds.center - element.transform.position + bounds.size / 2;
        }
        else
        {
            Debug.LogError("Couldn't find a MeshRenderer component from Element2D [" + element.name + "]");
        }
    }
}