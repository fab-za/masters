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
    public float trialHapticIndex;
    public float trialHapticFrequency;
    public float accuracySmoother;
    public JNDDataStruct(){}
    public JNDDataStruct(int ph, int e, int p, float tvf, float thi, float thf, float a)
    {
        phase = ph;
        experimentIndex = e;
        participantIndex = p;

        trialVisualFrequency = tvf;
        trialHapticIndex = thi;
        trialHapticFrequency = thf;
        accuracySmoother = a;
    }
}