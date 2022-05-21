using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStruct 
{
    public float trialFrequency;
    public float trialPeriod;
    public float trialAmplitude;
    public float participantFrequency;
    public float participantPeriod;
    public float participantAmplitude;
    public float errorFrequency;
    public float errorPeriod;
    public float errorAmplitude;
    public DataStruct(){}
    public DataStruct(float tf, float tp, float ta, float pf, float pp ,float pa, float ef, float ep, float ea)
    {
        trialFrequency = tf;
        trialPeriod = tp;
        trialAmplitude = ta;
    
        participantFrequency = pf;
        participantPeriod = pp;
        participantAmplitude = pa;

        errorFrequency = ef;
        errorPeriod = ep;
        errorAmplitude = ea;
    }
}
