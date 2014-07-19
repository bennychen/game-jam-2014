public static class Element2DFactory
{
    public static IElement2DDelegate CreateDelegate(Element2DType type)
    {
        switch (type)
        {
            //case Element2DType.Sprite:
            case Element2DType.Mesh:
                return new MeshElement2D();
            case Element2DType.Group:
                return new GroupElement2D();
            default:
                return new NullableElement2D();
        }
    }
}
