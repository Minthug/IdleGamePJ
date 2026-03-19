using UnityEngine;

public class Worker : MonoBehaviour
{
    private WorkerData data;
    private float timer;
    private bool isActive = false;

    public void Init(WorkerData workerData)
    {
        data = workerData;
        timer = 0f;
        isActive = true;
    }

    void Update()
    {
        if (!isActive || data == null) return;

        timer += Time.deltaTime;
        if (timer >= data.collectInterval)
        {
            timer = 0f;
            Collect();
        }
    }

    private void Collect()
    {
        double amount = data.baseCollectAmount * WorkerManager.Instance.GetSpeedMultiplier();
        ResourceManager.Instance.Add(data.targetResource, amount);
    }

    // 부스트 종료 후 일꾼 해제
    public void SetActive(bool active) => isActive = active;

    // 오프라인 보상 계산용
    public double CalcOfflineEarnings(float seconds)
    {
        float ticks = seconds / data.collectInterval;
        return data.baseCollectAmount * ticks;
    }

    public WorkerData GetData() => data;
}
