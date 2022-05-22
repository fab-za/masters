using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataStruct 
{
    public float trialFrequency;
    public float trialRoughness;
    public float trialAmplitude;
    public float participantFrequency;
    public float participantRoughness;
    public float participantAmplitude;
    public float errorFrequency;
    public float errorRoughness;
    public float errorAmplitude;
    public DataStruct(){}
    public DataStruct(float tf, float tp, float ta, float pf, float pp ,float pa, float ef, float ep, float ea)
    {
        trialFrequency = tf;
        trialRoughness = tp;
        trialAmplitude = ta;
    
        participantFrequency = pf;
        participantRoughness = pp;
        participantAmplitude = pa;

        errorFrequency = ef;
        errorRoughness = ep;
        errorAmplitude = ea;
    }
}
