using System.Collections.Generic;
using UnityEngine;
using GameJam.Utils;

public class Element2D : MonoBehaviour
{
    [SerializeField]
    public Element2DType Type;
    [SerializeField]
    public Element2D Parent;
    [SerializeField]
    public Vector2 Min;
    [SerializeField]
    public Vector2 Max;
    [SerializeField]
    public List<Element2D> ChildElements;

    public Vector2 Size
    {
        get
        {
            return Max - Min;
        }
    }

    public Vector2 Center
    {
        get
        {
            return (Min + Max) / 2;
        }
    }

    public IElement2DDelegate Delegate { get; private set; }

    public void DestroyAndClearAllChildren()
    {
        foreach (Element2D child in ChildElements)
        {
            GameObject.Destroy(child.gameObject);
        }
        ChildElements.Clear();
    }

    public bool HasChildElement(Element2D childElement)
    {
        return ChildElements.Contains(childElement);
    }

    public void AddChildElement(Element2D childElement)
    {
        ChildElements.Add(childElement);
    }

    public void RemoveChildElement(Element2D childElement)
    {
        ChildElements.Remove(childElement);
    }

    public Element2D GetChildElement(int i)
    {
        if (i < ChildElements.Count)
        {
            return ChildElements[i];
        }
        return null;
    }

    public void Rebuild()
    {
        Rebuild(null);
    }

    public void Rebuild(Element2D parent)
    {
        Parent = parent;

        Delegate = null;
        Delegate = Element2DFactory.CreateDelegate(Type);
        Delegate.RebuildElement(this);
    }

    protected void OnDrawGizmos()
    {
        if (Parent == null) // only draw root components
        {
            Vector3 globalMin = transform.position + new Vector3(Min.x, Min.y);
            Vector3 globalMax = transform.position + new Vector3(Max.x, Max.y);

            Color originalColor = Gizmos.color;
            Gizmos.color = Color.red;

            // draw bounds
            Gizmos.DrawWireCube((globalMin + globalMax) / 2, (globalMax - globalMin));
            // draw origin
            //Gizmos.DrawWireSphere(transform.position, 0.2f);

            Gizmos.color = originalColor;
        }
    }
}