using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultimodalDataStruct 
{
    public int experimentIndex;
    public int participantIndex;
    public float trialVisualFrequency;
    public float trialHapticFrequency;
    public int participantClass;
    public MultimodalDataStruct(){}
    public MultimodalDataStruct(int e, int p, float tvf, float thf, int c)
    {
        experimentIndex = e;
        participantIndex = p;

        trialVisualFrequency = tvf;
        trialHapticFrequency = thf;
        participantClass = c;
    }
}