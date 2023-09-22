using System;
using System.Collections;
using UnityEngine;

public static class Helper
{
    #region Public Methods
    public static Vector3[] GetUniformPointsOnSphere(int numberOfPoints)
    {
        Vector3[] points = new Vector3[numberOfPoints];
        
        float i = Mathf.PI * (3.0f - Mathf.Sqrt(5.0f));
        float offset = 2.0f / numberOfPoints;
        float halfOffset = 0.5f * offset;
        
        for (int currPoint = 0; currPoint < numberOfPoints; currPoint++)
        {
            float y = currPoint * offset - 1 + halfOffset;
            float r = Mathf.Sqrt(1 - y * y);
            float phi = currPoint * i;
            
            points[currPoint] = new Vector3(Mathf.Cos(phi) * r, y, Mathf.Sin(phi) * r);
        }

        return points;
    }

    public static float GetSignalPower(float distance, int reflectionsCount = 0)
    {
        float attenuationСoefficient = 0.8f;
        float signalWithoutReflection = 1.0f / Mathf.Pow(distance, 2.0f);
        
        return reflectionsCount == 0
            ? signalWithoutReflection
            : signalWithoutReflection * Mathf.Pow(attenuationСoefficient, reflectionsCount);
    }
    #endregion

    #region Extensions Methods
    public static void SetXPosition(this Transform transform, float x)
    {
        Vector3 position = transform.position;
        position.x = x;
        transform.position = position;
    }
    
    public static void SetYPosition(this Transform transform, float y)
    {
        Vector3 position = transform.position;
        position.y = y;
        transform.position = position;
    }
    
    public static void SetZPosition(this Transform transform, float z)
    {
        Vector3 position = transform.position;
        position.z = z;
        transform.position = position;
    }
    
    public static void SetXScale(this Transform transform, float x)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = x;
        transform.localScale = localScale;
    }
    
    public static void SetYScale(this Transform transform, float y)
    {
        Vector3 localScale = transform.localScale;
        localScale.y = y;
        transform.localScale = localScale;
    }
    
    public static void SetZScale(this Transform transform, float z)
    {
        Vector3 localScale = transform.localScale;
        localScale.z = z;
        transform.localScale = localScale;
    }

    public static void CorrectEulerAngles(this Transform transform, float x = 0, float y = 0, float z = 0, bool xSetMode = false, bool ySetMode = false, bool zSetMode = false)
    {
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler
        (
            xSetMode ? x : eulerAngles.x + x,
            ySetMode ? y : eulerAngles.y + y,
            zSetMode ? z : eulerAngles.z + z
        );
    }

    public static void ChangeTransform(this Transform transform, ObjectInfo objectInfo)
    {
        transform.position   = objectInfo.GetPosition();
        transform.localScale = objectInfo.GetScale();
        transform.rotation   = objectInfo.GetRotation();
    }
    
    public static IEnumerator SmoothlySetAlpha(this CanvasGroup canvasGroup, float targetAlpha, float seconds = 0.5f)
    {
        float startAlpha = canvasGroup.alpha;
        float lerpValue = 0.0f;

        while (Math.Abs(canvasGroup.alpha - targetAlpha) > 0.001f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, lerpValue);
            lerpValue += Time.unscaledDeltaTime / seconds;
            
            yield return null;
        }
    }

    public static int ToInt(this string valueStr)
    {
        int valueInt = 0;

        try
        {
            valueInt = Convert.ToInt32(valueStr);
        }
        catch { }

        return valueInt;
    }

    public static bool IsReceiverLayer(this int layer)
    {
        return layer == ReceiverLayer;
    }

    public static bool IsEmitterLayer(this int layer)
    {
        return layer == EmitterLayer;
    }

    public static bool IsGroundLayer(this int layer)
    {
        return layer == GroundLayer;
    }
    
    public static bool IsBuildingLayer(this int layer)
    {
        return layer == BuildingLayer;
    }
    #endregion

    #region LayerMask Helper
    public static readonly int ReceiverLayer = LayerMask.NameToLayer("Receiver");
    public static readonly int EmitterLayer  = LayerMask.NameToLayer("Emitter");
    public static readonly int GroundLayer   = LayerMask.NameToLayer("Ground");
    public static readonly int BuildingLayer = LayerMask.NameToLayer("Building");
    #endregion
}
