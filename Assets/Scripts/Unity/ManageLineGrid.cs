using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;
// using static System.Math;

public class ManageLineGrid : MonoBehaviour
{
    
    [System.Serializable]
    public struct GridParameters{
        public int roughness;
        public int side;    // left = -1, right = +1
        public float period;
        public float amplitude;
        public LineRenderer lineRenderer;
        public Vector3[] positions;

        public GridParameters(int r, int s, float p, float a, LineRenderer l, Vector3[] pos){
            roughness = r;
            side = s;
            period = p;
            amplitude = a;
            lineRenderer = l;
            positions = pos;

        }
    }
    [System.Serializable]
    public struct SliderValues{
        public float amplitude_left;
        public float amplitude_right;
        public float period_left;
        public float period_right;

        public SliderValues(float sil, float sir, float spl, float spr){
            amplitude_left = sil;
            amplitude_right = sir;
            period_left = spl;
            period_right = spr;
        }
    }
    public GridParameters leftGrid;
    public GridParameters rightGrid;
    public SliderValues slider;
    // public int trial = 0;
    // private GameObject newGrid;
    public int tempLeftRoughness;
    public int tempRightRoughness;
    public int samplingRate;
    public float weightAmplitude;
    public float weightPeriod;
    
    void Start()
    {
        // lr = lineManager.GetComponent<LineRenderer>();

        // leftGrid = new GridParameters(0,-1,1,0);
        // rightGrid = new GridParameters(0,1,1,0);
        // slider = new SliderValues(1,1,0,0);

        // leftGrid.positions = new Vector3[samplingRate];
        // rightGrid.positions = new Vector3[samplingRate];
    }

    // Update is called once per frame
    void Update()
    {
        // lr.numCornerVertices = numCornerVertices;

        leftGrid.roughness = tempLeftRoughness;
        rightGrid.roughness = tempRightRoughness;
        updateParameters();
        updateLine();
    }

    public void AdjustAmplitudeLeft(float newAmplitude){
        slider.amplitude_left = newAmplitude;
    }

    public void AdjustPeriodLeft(float newPeriod){
        slider.period_left = newPeriod;
    }

    public void AdjustAmplitudeRight(float newAmplitude){
        slider.amplitude_right = newAmplitude;
    }

    public void AdjustPeriodRight(float newPeriod){
        slider.period_right = newPeriod;
    }

    private void updateParameters(){
        leftGrid.amplitude = slider.amplitude_left;
        leftGrid.period = slider.period_left;

        rightGrid.amplitude = slider.amplitude_right;
        rightGrid.period = slider.period_right;

        leftGrid.positions = parametersToPositions(leftGrid);
        rightGrid.positions = parametersToPositions(rightGrid);
    }

    public void convertToRoughness(){
        // somehow based on the current grid parameters, categorise into roughness
          
        // roughness = (weightAmplitude * amplitude) /( weightPeriod * period);
        
        // return higher number = rougher

    }

    public void updateLine(){
        leftGrid.lineRenderer.positionCount = leftGrid.positions.Length;
        leftGrid.lineRenderer.SetPositions(leftGrid.positions);

        rightGrid.lineRenderer.positionCount = rightGrid.positions.Length;
        rightGrid.lineRenderer.SetPositions(rightGrid.positions);
    }
    public Vector3[] parametersToPositions(GridParameters panel){
        Vector3[] positions = new Vector3[samplingRate];

        double[] x = Generate.LinearSpaced(samplingRate, 0, (10*panel.side));

        for(int i = 0; i < x.Length; i++){
            float y = (panel.amplitude * Mathf.Sin(panel.period * (float)x[i]));
            Vector3 coord = new Vector3((float)x[i], y, 0);

            positions[i] = coord;
        }

        return positions;
    }
}
