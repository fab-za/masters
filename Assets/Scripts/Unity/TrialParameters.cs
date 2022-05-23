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
    public TrialParameters(float rl, float al, float pl, float rr, float ar, float pr){
            roughness_left = rl;
            amplitude_left = al;
            frequency_left = pl;
            roughness_right = rr;
            amplitude_right = ar;
            frequency_right = pr;
        }
}
