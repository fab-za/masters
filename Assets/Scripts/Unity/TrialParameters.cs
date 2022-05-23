using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TrialParameters
{
    public int roughness_left;
    public float amplitude_left;
    public float frequency_left;
    public int roughness_right;
    public float amplitude_right;
    public float frequency_right;
    public TrialParameters(){}
    public TrialParameters(int rl, float al, float pl, int rr, float ar, float pr){
            roughness_left = rl;
            amplitude_left = al;
            frequency_left = pl;
            roughness_right = rr;
            amplitude_right = ar;
            frequency_right = pr;
        }
}
