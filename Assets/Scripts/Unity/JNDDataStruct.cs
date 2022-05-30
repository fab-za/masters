using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JNDDataStruct 
{
    public int phase;
    public int experimentIndex;
    public int participantIndex;
    public float trialVisualFrequency;
    public float trialHapticFrequency;
    public float accuracyRougher;
    public JNDDataStruct(){}
    public JNDDataStruct(int ph, int e, int p, float tvf, float thf, float a)
    {
        phase = ph;
        experimentIndex = e;
        participantIndex = p;

        trialVisualFrequency = tvf;
        trialHapticFrequency = thf;
        accuracyRougher = a;
    }
}