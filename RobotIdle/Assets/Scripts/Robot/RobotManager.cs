using System;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public static RobotManager Instance { get; private set; }

    [SerializeField] private RobotPartData[] allParts;   // Inspector에서 ScriptableObject 연결

    private int[] partLevels;   // 파츠별 현재 레벨

    public int TotalWorkerSlots { get; private set; } = 2;   // 초기 일꾼 슬롯

    public event Action<RobotPartType, int> OnPartUpgraded;   // 파츠 업그레이드 시 이벤트
    public event Action<int> OnWorkerSlotsChanged;            // 일꾼 슬롯 변경 시 이벤트

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        partLevels = new int[allParts.Length];
    }

    public int GetPartLevel(RobotPartType type) => partLevels[(int)type];

    public bool CanUpgrade(RobotPartType type)
    {
        int index = (int)type;
        RobotPartData data = allParts[index];
        int currentLevel = partLevels[index];

        if (currentLevel >= data.maxLevel) return false;

        ResourceCost[] cost = data.upgradeCosts[currentLevel];
        return ResourceManager.Instance.CanAfford(cost);
    }

    public bool Upgrade(RobotPartType type)
    {
        int index = (int)type;
        RobotPartData data = allParts[index];
        int currentLevel = partLevels[index];

        if (!CanUpgrade(type)) return false;

        ResourceManager.Instance.Spend(data.upgradeCosts[currentLevel]);

        partLevels[index]++;

        // 일꾼 슬롯 증가
        int slotBonus = data.workerSlotBonus[currentLevel];
        if (slotBonus > 0)
        {
            TotalWorkerSlots += slotBonus;
            OnWorkerSlotsChanged?.Invoke(TotalWorkerSlots);
        }

        OnPartUpgraded?.Invoke(type, partLevels[index]);
        return true;
    }

    // 세이브/로드
    public int[] GetPartLevels() => (int[])partLevels.Clone();

    public void LoadPartLevels(int[] saved)
    {
        partLevels = saved;
        RecalculateWorkerSlots();
    }

    private void RecalculateWorkerSlots()
    {
        TotalWorkerSlots = 2;
        for (int i = 0; i < allParts.Length; i++)
        {
            for (int lvl = 0; lvl < partLevels[i]; lvl++)
                TotalWorkerSlots += allParts[i].workerSlotBonus[lvl];
        }
        OnWorkerSlotsChanged?.Invoke(TotalWorkerSlots);
    }
}
