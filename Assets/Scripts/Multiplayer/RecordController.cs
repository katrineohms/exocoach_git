using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SerializableList<T>
{
    public List<T> list = new List<T>();
}

public class RecordController : MonoBehaviour
{
    public static event Action<string> OnResendMsg;
    public static event Action OnSaveRecord;

    [SerializeField] private PlayerNetworkController pnc;
    private bool isRecording = false;
    private bool isReplaying = true;
    private int replayCounter = 0;
    private SerializableList<string> recording = new SerializableList<string>();
    private List<string> replayRecording = new List<string>();

    private List<string> recordings = new List<string>();

    public void ToggleRecording()
    {
        isRecording = !isRecording;
    }

    public void Start()
    {
        string path = Application.persistentDataPath + "/Recordings/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        DirectoryInfo d = new DirectoryInfo(path);
        Debug.Log(d);
        foreach (var file in d.GetFiles("*"))
        {
            recordings.Add(file.Name);
        }
        OnSaveRecord?.Invoke();
    }

    public void Update()
    {
        if (isRecording)
        {
            recording.list.Add(pnc.Message);
            Debug.Log(pnc.Message);
        }
        if (isReplaying && replayCounter < replayRecording.Count)
        {
            Debug.Log(isReplaying + " " + replayCounter + " " + replayRecording.Count);
            OnResendMsg?.Invoke(replayRecording[replayCounter]);
            replayCounter++;
        }
        else if (isReplaying && replayCounter >= replayRecording.Count)
        {
            isReplaying = false;
        }
    }

    public List<string> SaveRecording(string name)
    {
        recordings.Add(name);
        SaveToFile(name, recording);
        OnSaveRecord?.Invoke();
        return recordings;
    }

    public void DeleteRecording(string name)
    {
        recordings.Remove(name);
        DeleteFile(name);
        OnSaveRecord?.Invoke();
    }

    public static void SaveToFile(string name, SerializableList<string> recording)
    {
        Debug.Log(recording.list[0]);
        string path = Application.persistentDataPath + "/Recordings/" + name;
        string json = JsonUtility.ToJson(recording, true);
        Debug.Log(json);
        File.WriteAllText(Application.persistentDataPath + "/Recordings/" + name, json);
    }


    public static void DeleteFile(string name)
    {
        string path = Application.persistentDataPath + "/Recordings/" + name;
        File.Delete(path);
    }

    public void ReplayRecording(string name)
    {
        string filePath = Application.persistentDataPath + "/Recordings/" + name;

        string json = File.ReadAllText(filePath);
        Debug.Log(json);

        SerializableList<string> selectedRecording = JsonUtility.FromJson<SerializableList<string>>(json);

        replayRecording = selectedRecording.list;
        replayCounter = 0;
        isReplaying = true;
    }

    public List<string> GetRecordings()
    {
        return recordings;
    }

    public bool IsRecording()
    {
        return isRecording;
    }
}
