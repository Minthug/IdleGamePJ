using UnityEngine;

[CreateAssetMenu(fileName = "RobotPartData", menuName = "IdleGame/Robot Part Data")]
public class RobotPartData : ScriptableObject
{
    public string partName;
    public RobotPartType partType;
    public Sprite[] upgradeSprites;        // 단계별 스프라이트

    [Header("업그레이드 설정")]
    public int maxLevel = 5;
    public ResourceCost[] upgradeCosts;    // 레벨별 업그레이드 비용
    public int[] workerSlotBonus;          // 레벨별 일꾼 슬롯 증가량
}

[System.Serializable]
public class ResourceCost
{
    public ResourceType resourceType;
    public double amount;
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
