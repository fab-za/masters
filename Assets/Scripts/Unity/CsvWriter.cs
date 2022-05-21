using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class CsvWriter : MonoBehaviour
{
    public static string ToCSV(List<DataStruct> data)
    {
        var sb = new StringBuilder("Time,X1,Y1,Z1,Rx1,Ry1,Rz1,X2,Y2,Z2,Rx2,Ry2,Rz2,wrist");
        foreach (DataStruct frame in data)
        {
            
            sb.Append('\n').Append(frame.Time.ToString()).Append(',').Append(frame.positionX1.ToString()).Append(',').Append(frame.positionY1.ToString()).Append(',').Append(frame.positionZ1.ToString()).Append(',').Append(frame.rotationX1.ToString()).Append(',').Append(frame.rotationY1.ToString()).Append(',').Append(frame.rotationZ1.ToString()).Append(',').Append(frame.positionX2.ToString()).Append(',').Append(frame.positionY2.ToString()).Append(',').Append(frame.positionZ2.ToString()).Append(',').Append(frame.rotationX2.ToString()).Append(',').Append(frame.rotationY2.ToString()).Append(',').Append(frame.rotationZ2.ToString()).Append(',').Append(frame.wristRotation.ToString());
            //Debug.Log(sb);
        }

        return sb.ToString();
    }


    public static void saveAsCsv(List<DataStruct> data, string name)
    {
        var content = ToCSV(data);  
        string path = Application.persistentDataPath + "/"+name+".csv";
        var writer = new StreamWriter(path, false);
        writer.Write(content);
        writer.Close();
        Debug.Log($"CSV file written to \"{path}\"");
    }

    public static void recordResult(List<List<float>> data)
    {
        var result  = new StringBuilder("Trial Frequency, Trial Period, Trial Amplitude, Participant Frequency, Participant Period, Participant Amplitude, Frequency Error, Period Error, Amplitude Error");
        result.Append('\n');
        foreach (List<float> list in data)
        {
            for(int i = 0; i<list.Count; i++)
            {
                result.Append(list[i].ToString()).Append(',');
            }

            result.Append('\n');
            
        }
        string path = Application.persistentDataPath + "/result.csv";
        var writer = new StreamWriter(path, true); // true for append, false for overwrite
        writer.Write(result);
        writer.Close();
        Debug.Log($"result CSV file written to \"{path}\"");
    }

}
