using System;
using System.Collections.Generic;

[Serializable]
public class ArmData
{
    public float angle = -10000000;
    public string dateTime;
}

[Serializable]
public class ArmDataObj
{
    public List<ArmData> master = new List<ArmData>();
    public List<ArmData> secondary = new List<ArmData>();
}
