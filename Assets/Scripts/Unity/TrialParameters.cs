using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TrialParameters
{
    public float roughness_left;
    public float amplitude_left;
    public float frequency_left;
    public float roughness_right;
    public float amplitude_right;
    public float frequency_right;
    public TrialParameters(){}
    public TrialParameters(TrialParameters other){
        roughness_left = other.roughness_left;
        amplitude_left = other.amplitude_right;
        frequency_left = other.frequency_left;
        roughness_right = other.roughness_right;
        amplitude_right = other.amplitude_right;
        frequency_right = other.frequency_right;
    }
    public TrialParameters(float rl, float al, float pl, float rr, float ar, float pr){
        roughness_left = rl;
        amplitude_left = al;
        frequency_left = pl;
        roughness_right = rr;
        amplitude_right = ar;
        frequency_right = pr;
    }
}
