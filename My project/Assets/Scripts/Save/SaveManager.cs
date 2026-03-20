using System;
using System.Collections.Generic;
using UnityEngine;

// JsonUtility는 Dictionary를 지원하지 않으므로 직렬화 가능한 구조로 변환
[Serializable]
public class SerializableResource
{
    public ResourceType type;
    public double amount;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private const string SAVE_KEY = "IdleGameSave";
    private const string QUIT_TIME_KEY = "QuitTime";

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Load();
        ProcessOfflineEarnings();
    }

    void OnApplicationPause(bool paused)
    {
        if (paused) Save();
    }

    void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        var rawResources = ResourceManager.Instance.GetAllResources();
        var resourceList = new List<SerializableResource>();
        foreach (var pair in rawResources)
            resourceList.Add(new SerializableResource { type = pair.Key, amount = pair.Value });

        SaveData data = new SaveData
        {
            resources = resourceList.ToArray(),
            partLevels = RobotManager.Instance.GetPartLevels(),
            quitTime = DateTime.UtcNow.ToString("o")
        };

        PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(data));
        PlayerPrefs.SetString(QUIT_TIME_KEY, data.quitTime);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY)) return;

        string json = PlayerPrefs.GetString(SAVE_KEY);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        var resourceDict = new Dictionary<ResourceType, double>();
        foreach (var r in data.resources)
            resourceDict[r.type] = r.amount;

        ResourceManager.Instance.LoadResources(resourceDict);
        RobotManager.Instance.LoadPartLevels(data.partLevels);
    }

    private void ProcessOfflineEarnings()
    {
        if (!PlayerPrefs.HasKey(QUIT_TIME_KEY)) return;

        string quitTimeStr = PlayerPrefs.GetString(QUIT_TIME_KEY);
        DateTime quitTime = DateTime.Parse(quitTimeStr, null, System.Globalization.DateTimeStyles.RoundtripKind);
        float offlineSeconds = (float)(DateTime.UtcNow - quitTime).TotalSeconds;

        // 최대 8시간까지만 오프라인 보상 (광고 보면 2배)
        offlineSeconds = Mathf.Min(offlineSeconds, 8f * 3600f);

        if (offlineSeconds > 10f)
            WorkerManager.Instance.ApplyOfflineEarnings(offlineSeconds);
    }
}

[Serializable]
public class SaveData
{
    public SerializableResource[] resources;
    public int[] partLevels;
    public string quitTime;
}
