using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class CsvWriter : MonoBehaviour
{
    public void initCSV(int index){
        var result  = new StringBuilder("Trial Frequency, Trial Roughness, Trial Amplitude, Participant Frequency, Participant Roughness, Participant Amplitude, Frequency Error, Roughness Error, Amplitude Error");
        result.Append('\n');

        string path = Application.dataPath + "/" + index + "/result.csv";
        var writer = new StreamWriter(path, false); // true for append, false for overwrite
        writer.Write(result);
        writer.Close();
        Debug.Log($"result CSV file written to \"{path}\"");
    }

    public void addToCSV(DataStruct frame){
        var result = new StringBuilder("");
        result.Append(frame.trialFrequency.ToString()).Append(',').Append(frame.trialRoughness.ToString()).Append(',').Append(frame.trialAmplitude.ToString()).Append(',').Append(frame.participantFrequency.ToString()).Append(',').Append(frame.participantRoughness.ToString()).Append(',').Append(frame.participantRoughness.ToString()).Append(',').Append(frame.errorFrequency.ToString()).Append(',').Append(frame.errorRoughness.ToString());
        //Debug.Log(result);

        int index = (int)frame.trialRoughness;
        result.ToString();

        string path = Application.dataPath + "/" + index + "_result.csv";
        var writer = new StreamWriter(path, true); // true for append, false for overwrite
        writer.Write(result);
        writer.Close();
        Debug.Log($"result CSV file written to \"{path}\"");
    }
    public void storeParticipantCSV(List<DataStruct> data, int index){
        initCSV(index);
        var result = new StringBuilder("");

        foreach (DataStruct frame in data)
        {
            result.Append(frame.trialFrequency.ToString()).Append(',').Append(frame.trialRoughness.ToString()).Append(',').Append(frame.trialAmplitude.ToString()).Append(',').Append(frame.participantFrequency.ToString()).Append(',').Append(frame.participantRoughness.ToString()).Append(',').Append(frame.participantRoughness.ToString()).Append(',').Append(frame.errorFrequency.ToString()).Append(',').Append(frame.errorRoughness.ToString()).Append('\n');
            //Debug.Log(result);
        }

        result.ToString();
        string path = Application.dataPath + "/" + index + "/result.csv";
        var writer = new StreamWriter(path, true); // true for append, false for overwrite
        writer.Write(result);
        writer.Close();
        Debug.Log($"result CSV file written to \"{path}\"");
    }
    public void initRoughnessCSVs(float[] roughness){
        foreach(float r in roughness){
            string path = Application.dataPath + "/" + (int)r + "_result.csv";

            if(!System.IO.File.Exists(path)){
                var result  = new StringBuilder("Trial Frequency, Trial Roughness, Trial Amplitude, Participant Frequency, Participant Roughness, Participant Amplitude, Frequency Error, Roughness Error, Amplitude Error");
                result.Append('\n');

                
                var writer = new StreamWriter(path, false); // true for append, false for overwrite
                writer.Write(result);
                writer.Close();
                Debug.Log($"new CSV file written to \"{path}\"");
            }
        }
        
    }

}
