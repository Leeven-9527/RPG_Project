using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    private const float doThreshold = 0.5f;
    public static bool isFacingTarget(this Transform transform, Transform target) {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize(); //得到向量的方向
        // Vector3.Dot两个向量的点积
        bool isFacing = Vector3.Dot(transform.forward, vectorToTarget) >= doThreshold;
        return isFacing;
    }
}
