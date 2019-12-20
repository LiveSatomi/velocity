namespace Utility
{
using UnityEngine;
 
public enum Orientation
{
    Horizontal,
    Vertical
}
public static class MeshCreator 
{

    public static Mesh CreatePlane( string name, int widthSegments, int lengthSegments, float width, float length, Orientation orientation, bool twoSided)
    {
        Mesh m = new Mesh {name = name};
        int hCount2 = widthSegments+1;
        int vCount2 = lengthSegments+1;
        int numTriangles = widthSegments * lengthSegments * 6;
        if (twoSided) {
            numTriangles *= 2;
        }
        int numVertices = hCount2 * vCount2;

        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];
        int[] triangles = new int[numTriangles];
        Vector4[] tangents = new Vector4[numVertices];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        int index = 0;
        float uvFactorX = 1.0f/widthSegments;
        float uvFactorY = 1.0f/lengthSegments;
        float scaleX = width/widthSegments;
        float scaleY = length/lengthSegments;
        for (float y = 0.0f; y < vCount2; y++)
        {
            for (float x = 0.0f; x < hCount2; x++)
            {
                if (orientation == Orientation.Horizontal)
                {
                    vertices[index] = new Vector3(x*scaleX - width/2f , 0.0f, y*scaleY - length/2f);
                }
                else if (orientation == Orientation.Vertical)
                {
                    vertices[index] = new Vector3(x*scaleX - width/2f , y*scaleY - length/2f , 0.0f);
                }                
                tangents[index] = tangent;
                uvs[index++] = new Vector2(x*uvFactorX, y*uvFactorY);
            }
        }

        index = 0;
        for (int y = 0; y < lengthSegments; y++)
        {
            for (int x = 0; x < widthSegments; x++)
            {
                triangles[index]   = (y     * hCount2) + x;
                triangles[index+1] = ((y+1) * hCount2) + x;
                triangles[index+2] = (y     * hCount2) + x + 1;

                triangles[index+3] = ((y+1) * hCount2) + x;
                triangles[index+4] = ((y+1) * hCount2) + x + 1;
                triangles[index+5] = (y     * hCount2) + x + 1;
                index += 6;
            }
            if (twoSided) {
                // Same tri vertices with order reversed, so normals point in the opposite direction
                for (int x = 0; x < widthSegments; x++)
                {
                    triangles[index]   = (y     * hCount2) + x;
                    triangles[index+1] = (y     * hCount2) + x + 1;
                    triangles[index+2] = ((y+1) * hCount2) + x;

                    triangles[index+3] = ((y+1) * hCount2) + x;
                    triangles[index+4] = (y     * hCount2) + x + 1;
                    triangles[index+5] = ((y+1) * hCount2) + x + 1;
                    index += 6;
                }
            }
        }

        m.vertices = vertices;
        m.uv = uvs;
        m.triangles = triangles;
        m.tangents = tangents;
        m.RecalculateNormals();
        m.RecalculateBounds();

        return m;
    }
}
}