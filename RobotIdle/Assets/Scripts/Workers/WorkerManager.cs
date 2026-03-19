using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public static WorkerManager Instance { get; private set; }

    [SerializeField] private WorkerData[] availableWorkers;
    [SerializeField] private Transform workerParent;     // 일꾼들이 붙을 로봇 오브젝트

    private List<Worker> activeWorkers = new();
    private float speedMultiplier = 1f;
    private float boostTimer = 0f;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (boostTimer > 0f)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
            {
                speedMultiplier = 1f;
                boostTimer = 0f;
            }
        }
    }

    public bool HireWorker(WorkerData data)
    {
        if (activeWorkers.Count >= RobotManager.Instance.TotalWorkerSlots) return false;
        if (!ResourceManager.Instance.Spend(data.hireCost)) return false;

        GameObject go = new GameObject($"Worker_{data.workerName}");
        go.transform.SetParent(workerParent);
        Worker worker = go.AddComponent<Worker>();
        worker.Init(data);
        activeWorkers.Add(worker);
        return true;
    }

    // 광고 보상: 일꾼 속도 부스트
    public void ApplySpeedBoost(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        boostTimer = duration;
    }

    public float GetSpeedMultiplier() => speedMultiplier;

    public int GetWorkerCount() => activeWorkers.Count;

    // 오프라인 보상 계산
    public void ApplyOfflineEarnings(float offlineSeconds)
    {
        foreach (var worker in activeWorkers)
        {
            double earned = worker.CalcOfflineEarnings(offlineSeconds);
            ResourceManager.Instance.Add(worker.GetData().targetResource, earned);
        }
    }
}
