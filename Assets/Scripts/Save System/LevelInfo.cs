using System;
using System.Collections.Generic;

[Serializable]
public class LevelInfo
{
    public List<ObjectInfo> buildingsInfo;
    public List<ObjectInfo> emittersInfo;
    public ObjectInfo receiverInfo;
    public string name;

    public LevelInfo(List<ObjectInfo> buildingsInfo, List<ObjectInfo> emittersInfo, ObjectInfo receiverInfo, string name)
    {
        this.buildingsInfo = buildingsInfo;
        this.emittersInfo = emittersInfo;
        this.receiverInfo = receiverInfo;
        this.name = name;
    }
}
