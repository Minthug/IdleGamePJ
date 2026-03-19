using UnityEngine;

[CreateAssetMenu(fileName = "ResourceData", menuName = "IdleGame/Resource Data")]
public class ResourceData : ScriptableObject
{
    public string resourceName;
    public Sprite icon;
    public ResourceType resourceType;
}

public enum ResourceType
{
    Iron,
    Copper,
    LunarOre,
    MartianCrystal,
    CosmicDust
}
