using System;
using UnityEngine;

[Serializable]
public class ObjectInfo
{
    private int posX;
    private int posY;
    private int posZ;
    
    private int scaleX;
    private int scaleY;
    private int scaleZ;

    private int rotationY;

    public ObjectInfo( Transform transform )
    {
        Vector3 position = transform.position;
        Vector3 localScale = transform.localScale;
        
        posX = (int)position.x;
        posY = (int)position.y;
        posZ = (int)position.z;
        
        scaleX = (int)localScale.x;
        scaleY = (int)localScale.y;
        scaleZ = (int)localScale.z;
        
        rotationY = (int)transform.eulerAngles.y;
    }

    public static implicit operator ObjectInfo(Transform transform)
    {
        return new ObjectInfo(transform);
    }

    public Vector3 GetPosition()
    {
        return new Vector3(posX, posY, posZ);
    }
    
    public Vector3 GetScale()
    {
        return new Vector3(scaleX, scaleY, scaleZ);
    }
    
    public Quaternion GetRotation()
    {
        return Quaternion.Euler(0.0f, rotationY, 0.0f);
    }
}
