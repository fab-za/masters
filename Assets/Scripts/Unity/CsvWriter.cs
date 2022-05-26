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

        string path = Application.dataPath + "/Data/" + index + "/result.csv";
        var writer = new StreamWriter(path, false); // true for append, false for overwrite
        writer.Write(result);
        writer.Close();
        Debug.Log($"result CSV file written to \"{path}\"");
    }

    public void addToCSV(DataStruct frame){
        var result = new StringBuilder("");
        result.Append(frame.trialFrequency.ToString()).Append(',').Append(frame.trialRoughness.ToString()).Append(',').Append(frame.trialAmplitude.ToString()).Append(',').Append(frame.participantFrequency.ToString()).Append(',').Append(frame.participantRoughness.ToString()).Append(',').Append(frame.participantRoughness.ToString()).Append(',').Append(frame.errorFrequency.ToString()).Append(',').Append(frame.errorRoughness.ToString()).Append('\n');
        //Debug.Log(result);

        int index = (int)frame.trialFrequency;
        result.ToString();

        string path = Application.dataPath + "/Data/frequencies/" + index + "_result.csv";
        var writer = new StreamWriter(path, true); // true for append, false for overwrite
        writer.Write(result);
        writer.Close();
        Debug.Log($"result CSV file written to \"{path}\"");
    }
    public void storeParticipantCSV(List<DataStruct> data, int index){
        string folderpath = Application.dataPath + "/Data/" + index + "/";

        if(!System.IO.File.Exists(folderpath)){
            Directory.CreateDirectory(folderpath);
        }

        string path = folderpath + "result.csv";
        var result = new StringBuilder("");
        var writer = new StreamWriter(path, false); // true for append, false for overwrite
        result.Append("Trial Frequency, Trial Roughness, Trial Amplitude, Participant Frequency, Participant Roughness, Participant Amplitude, Frequency Error, Roughness Error, Amplitude Error");
        result.Append('\n');
        // Debug.Log(path);

        foreach (DataStruct frame in data)
        {
            result.Append(frame.trialFrequency.ToString()).Append(',').Append(frame.trialRoughness.ToString()).Append(',').Append(frame.trialAmplitude.ToString()).Append(',').Append(frame.participantFrequency.ToString()).Append(',').Append(frame.participantRoughness.ToString()).Append(',').Append(frame.participantRoughness.ToString()).Append(',').Append(frame.errorFrequency.ToString()).Append(',').Append(frame.errorRoughness.ToString()).Append('\n');
            //Debug.Log(result);
        }

        result.ToString();
        // writer = new StreamWriter(path, true); // true for append, false for overwrite
        writer.Write(result);
        writer.Close();
        Debug.Log($"result CSV file written to \"{path}\"");
    }
    public void initIndvCSVs(List<float> list, string type){
        // Debug.Log("hello");
        foreach(float l in list){
            string folderpath = Application.dataPath + "/Data/" + type + "/";

            if(!System.IO.File.Exists(folderpath)){
                Directory.CreateDirectory(folderpath);
            }

            string path = folderpath + (int)l + "_result.csv";
            // Debug.Log(path);

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
