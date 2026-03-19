using UnityEngine;

[CreateAssetMenu(fileName = "RobotPartData", menuName = "IdleGame/Robot Part Data")]
public class RobotPartData : ScriptableObject
{
    public string partName;
    public RobotPartType partType;
    public Sprite[] upgradeSprites;        // 단계별 스프라이트

    [Header("업그레이드 설정")]
    public int maxLevel = 5;
    public UpgradeCostList[] upgradeCosts; // 레벨별 업그레이드 비용 (레벨당 여러 재료 가능)
    public int[] workerSlotBonus;          // 레벨별 일꾼 슬롯 증가량
}

[System.Serializable]
public class ResourceCost
{
    public ResourceType resourceType;
    public double amount;
}

// Inspector에서 레벨별 비용 목록을 직렬화하기 위한 래퍼
[System.Serializable]
public class UpgradeCostList
{
    public ResourceCost[] costs;
}

public enum RobotPartType
{
    Head,
    Body,
    ArmLeft,
    ArmRight,
    LegLeft,
    LegRight,
    Weapon,
    Appearance
}
