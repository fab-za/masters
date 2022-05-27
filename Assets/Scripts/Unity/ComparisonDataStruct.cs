using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComparisonDataStruct 
{
    public int experimentIndex;
    public int participantIndex;
    public float trialLeftFrequency;
    public float trialLeftRoughness;
    public float trialLeftAmplitude;
    public float trialRightFrequency;
    public float trialRightRoughness;
    public float trialRightAmplitude;
    public float leftFrequency;
    public float leftRoughness;
    public float leftAmplitude;
    public float rightFrequency;
    public float rightRoughness;
    public float rightAmplitude;
    public float differenceBetweenSidesFrequency;
    public float differenceBetweenSidesRoughness;
    public float differenceBetweenSidesAmplitude;
    public ComparisonDataStruct(){}
    public ComparisonDataStruct(int e, int p, float tlf, float tlp, float tla, float trf, float trp, float tra, float lf, float lp ,float la, float rf, float rp, float ra)
    {
        experimentIndex = e;
        participantIndex = p;

        trialLeftFrequency = tlf;
        trialLeftRoughness = tlp;
        trialLeftAmplitude = tla;

        trialRightFrequency = trf;
        trialRightRoughness = trp;
        trialRightAmplitude = tra;
    
        leftFrequency = lf;
        leftRoughness = lp;
        leftAmplitude = la;

        rightFrequency = rf;
        rightRoughness = rp;
        rightAmplitude = ra;

        differenceBetweenSidesFrequency = Mathf.Abs(rf - lf);
        differenceBetweenSidesRoughness = Mathf.Abs(rp- lp);
        differenceBetweenSidesAmplitude = Mathf.Abs(ra - la);
    }
}