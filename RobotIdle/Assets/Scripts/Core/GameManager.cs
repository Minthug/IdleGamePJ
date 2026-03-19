using UnityEngine;

/// <summary>
/// 씬에 하나만 존재하는 최상위 게임 매니저.
/// 각 매니저들의 초기화 순서를 보장하고 전체 흐름을 제어한다.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("매니저 참조")]
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private RobotManager robotManager;
    [SerializeField] private WorkerManager workerManager;
    [SerializeField] private SaveManager saveManager;

    public GameStage CurrentStage { get; private set; } = GameStage.Earth;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 스테이지 이동 (지구 → 달 → 화성 → 우주)
    public void MoveToNextStage()
    {
        if ((int)CurrentStage < System.Enum.GetValues(typeof(GameStage)).Length - 1)
        {
            CurrentStage++;
            Debug.Log($"스테이지 이동: {CurrentStage}");
        }
    }
}

public enum GameStage
{
    Earth,
    Moon,
    Mars,
    Space
}
