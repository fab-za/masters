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
        public float frequency;
        public float amplitude;
        public LineRenderer lineRenderer;
        public Vector3[] positions;

        public GridParameters(int r, int s, float p, float a, LineRenderer l, Vector3[] pos){
            roughness = r;
            side = s;
            frequency = p;
            amplitude = 0.01f*a;
            lineRenderer = l;
            positions = pos;

        }
    }
    public GridParameters leftGrid;
    public GridParameters rightGrid;
    public int samplingRate;
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        leftGrid.positions = parametersToPositions(leftGrid);
        rightGrid.positions = parametersToPositions(rightGrid);
        
        updateLine();
    }

    public void updateParameters(float left_amplitude, float left_frequency, int left_roughness, float right_amplitude, float right_frequency, int right_roughness){
        leftGrid.amplitude = left_amplitude;
        leftGrid.frequency = left_frequency;
        leftGrid.roughness = left_roughness;

        rightGrid.amplitude = right_amplitude;
        rightGrid.frequency = right_frequency;
        rightGrid.roughness = right_roughness;

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
            float y = (panel.amplitude * Mathf.Sin(panel.frequency * (float)x[i]));
            Vector3 coord = new Vector3((float)x[i], y, 0);

            positions[i] = coord;
        }

        return positions;
    }
}
