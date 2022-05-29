using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultimodalDataStruct 
{
    public int phase;
    public int experimentIndex;
    public int participantIndex;
    public float trialVisualFrequency;
    public float trialHapticFrequency;
    public float trialAmplitude;
    public int participantClass;
    public MultimodalDataStruct(){}
    public MultimodalDataStruct(int ph, int e, int p, float tvf, float thf, float ta, int c)
    {
        phase = ph;
        experimentIndex = e;
        participantIndex = p;

        trialVisualFrequency = tvf;
        trialHapticFrequency = thf;
        trialAmplitude = ta;

        participantClass = c;
    }
}