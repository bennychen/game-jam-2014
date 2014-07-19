using System.Collections.Generic;

public class LevelComponentHorizontalSorter : IComparer<LevelComponent>
{
    public int Compare(LevelComponent srcComponent, LevelComponent destComponent)
    {
        if (srcComponent.transform.position.x == destComponent.transform.position.x)
        {
            if (srcComponent.transform.position.y == destComponent.transform.position.y)
            {
                return 0;
            }
            else
            {
                return srcComponent.transform.position.y > destComponent.transform.position.y ? 1 : -1;
            }
        }
        else
        {
            return srcComponent.transform.position.x > destComponent.transform.position.x ? 1 : -1;
        }
    }
}

public class LevelElementHorizontalSorter : IComparer<LevelElement>
{
    public int Compare(LevelElement srcElement, LevelElement destElement)
    {
        float srcX = srcElement.Position.x;
        float destX = destElement.Position.x;

        if (srcX == destX)
        {
            if (srcElement.Position.y == destElement.Position.y)
            {
                return 0;
            }
            else
            {
                return srcElement.Position.y > destElement.Position.y ? 1 : -1;
            }
        }
        else
        {
            return srcX > destX ? 1 : -1;
        }
    }
}