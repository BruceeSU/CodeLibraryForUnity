using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class HandlesUtils
{

    private class Consts
    {
        public const float circleStep = 20f;
    }

    /// <summary>
    /// 绘制四棱椎
    /// </summary>
    /// <param name="basePoint">底座中心点</param>
    /// <param name="topPoint">椎体顶端点</param>
    /// <param name="width">底座宽度</param>
    public static void DrawPyramid(Vector3 basePoint, Vector3 topPoint, float width)
    {
        Vector3 toTop = (topPoint - basePoint).normalized;
        float halfWidth = width * 0.5f;
        Vector3 right = GetIdentityRightAxis(toTop) * halfWidth;
        Vector3 up = GetIdentityUpAxis(toTop) * halfWidth;

        Vector3[] baseVertexs = new Vector3[4];
        baseVertexs[0] = basePoint + up;
        baseVertexs[1] = basePoint + right;
        baseVertexs[2] = basePoint - up;
        baseVertexs[3] = basePoint - right;
        for (int i = 0; i < 4; ++i)
        {
            UnityEditor.Handles.DrawLine(topPoint, baseVertexs[i]);
            if (i < 3) UnityEditor.Handles.DrawLine(baseVertexs[i], baseVertexs[i + 1]);
            else UnityEditor.Handles.DrawLine(baseVertexs[0], baseVertexs[3]);
        }
    }
    public static void DrawSphere(Vector3 center, Vector3 up, Vector3 right, float radius)
    {
        DrawCircle(center, up, radius);
      //  DrawCircle(center, right, radius);
    }
    public static void DrawSphere(Transform trans, float radius)
    {
        DrawSphere(trans.position, trans.up, trans.right, radius);
    }
    public static void DrawCircle(Vector3 center, Vector3 normal, float radius)
    {
        Vector3[] points = GetCirclePoint(center, normal, radius);
        for (int i = 0; i < points.Length - 1; ++i)
        {
            Handles.DrawLine(points[i], points[i + 1]);
        }
        Handles.DrawLine(points[0], points[points.Length - 1]);
    }

    public static Vector3[] GetCirclePoint(Vector3 center, Vector3 normal, float radius)
    {
        Vector3 fwd = Vector3.ProjectOnPlane((normal + Vector3.one), normal).normalized;
        List<Vector3> points = new List<Vector3>();
        for (float i = 0; i <= 360f; i += Consts.circleStep)
        {
            
            Vector3 p = center + Quaternion.Euler(0f,i,0f) * fwd * radius;
            points.Add(p);
        }
        //Camera.main.depthTextureMode = DepthTextureMode.Depth;
        return points.ToArray();
    }

    public static Vector3 GetIdentityRightAxis(Vector3 forward)
    {
        if (forward == Vector3.zero) return Vector3.zero;
        Vector3 projectToHorizontal = Vector3.ProjectOnPlane(forward, Vector3.up);
        if (projectToHorizontal == Vector3.zero)
            return Vector3.right;
        return Vector3.Cross(projectToHorizontal, Vector3.up).normalized;
    }

    public static Vector3 GetIdentityUpAxis(Vector3 forward)
    {
        Vector3 right = GetIdentityRightAxis(forward);
        return Vector3.Cross(right, forward);
    }
}
