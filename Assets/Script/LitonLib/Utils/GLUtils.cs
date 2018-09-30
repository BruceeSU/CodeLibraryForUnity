using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GL绘图工具类
/// </summary>
public static class GLUtils
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    public static void Begin(int mode)
    {
        GL.Begin(mode);
    }
    /// <summary>
    /// 
    /// </summary>
    public static void End()
    {
        GL.End();
    }
    public static void DrawLine(Vector3 start, Vector3 end)
    {
        GL.Vertex(start);
        GL.Vertex(end);
    }
    public static void DrawLine(Vector3 start, Vector3 dir, float length)
    {
        DrawLine(start, start + dir.normalized * length);
    }

    public static void DrawCircle(Vector3 center, Vector3 normal, float radius)
    {
        Vector3[] points = GenerateCirclePoints(center, normal, radius);
        for (int i = 0; i < points.Length - 1; ++i)
        {
            GL.Vertex(points[i]);
            GL.Vertex(points[i + 1]);
        }
        DrawLine(points[points.Length - 1], points[0]);
    }

    /// <summary>
    /// 生成圆上的节点
    /// </summary>
    /// <param name="center"></param>
    /// <param name="normal"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    private static Vector3[] GenerateCirclePoints(Vector3 center, Vector3 normal, float radius)
    {
        return null;
    }

    /// <summary>
    /// 画网格线
    /// </summary>
    /// <param name="center"></param>
    /// <param name="dir"></param>
    /// <param name="crossDir"></param>
    /// <param name="step"></param>
    /// <param name="length"></param>
    /// <param name="num"></param>
    public static void DrawGridLine(Vector3 center, Vector3 dir, Vector3 crossDir, float step, float length, int num)
    {
        Vector3 start = center - dir.normalized * step*num*0.5f - crossDir * length * 0.5f;
        for (int i = 0; i < num; ++i)
        {
            GL.Vertex(start);
            GL.Vertex(start + crossDir.normalized * length);
            start += dir.normalized * step;
        }
    }

}
