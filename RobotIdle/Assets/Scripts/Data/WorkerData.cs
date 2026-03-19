using UnityEngine;

[CreateAssetMenu(fileName = "WorkerData", menuName = "IdleGame/Worker Data")]
public class WorkerData : ScriptableObject
{
    public string workerName;
    public Sprite sprite;
    public WorkerType workerType;

    [Header("능력치")]
    public double baseCollectAmount = 1.0;    // 회당 수집량
    public float collectInterval = 1.0f;      // 수집 주기 (초)
    public ResourceType targetResource;       // 수집 대상 재료

    [Header("고용 비용")]
    public ResourceCost[] hireCost;
}

public enum WorkerType
{
    Basic,
    Premium    // 유료 일꾼
}
