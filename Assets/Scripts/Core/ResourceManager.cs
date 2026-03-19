using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    // 재료별 현재 보유량
    private Dictionary<ResourceType, double> resources = new();

    // 재료 변경 시 UI에 알리는 이벤트
    public event Action<ResourceType, double> OnResourceChanged;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 모든 재료 타입 초기화
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            resources[type] = 0;
    }

    public double Get(ResourceType type) => resources[type];

    public void Add(ResourceType type, double amount)
    {
        resources[type] += amount;
        OnResourceChanged?.Invoke(type, resources[type]);
    }

    // 재료가 충분한지 확인
    public bool CanAfford(ResourceCost[] costs)
    {
        foreach (var cost in costs)
            if (resources[cost.resourceType] < cost.amount) return false;
        return true;
    }

    // 재료 소비 (업그레이드, 고용 등)
    public bool Spend(ResourceCost[] costs)
    {
        if (!CanAfford(costs)) return false;

        foreach (var cost in costs)
        {
            resources[cost.resourceType] -= cost.amount;
            OnResourceChanged?.Invoke(cost.resourceType, resources[cost.resourceType]);
        }
        return true;
    }

    // 세이브용
    public Dictionary<ResourceType, double> GetAllResources() => new(resources);

    // 로드용
    public void LoadResources(Dictionary<ResourceType, double> saved)
    {
        foreach (var pair in saved)
        {
            resources[pair.Key] = pair.Value;
            OnResourceChanged?.Invoke(pair.Key, pair.Value);
        }
    }
}
