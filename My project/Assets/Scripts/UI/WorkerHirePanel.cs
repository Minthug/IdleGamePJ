using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 일꾼 고용 패널 전체를 관리.
/// availableWorkers 배열에 WorkerData ScriptableObject를 연결하면
/// 자동으로 고용 카드를 생성한다.
/// </summary>
public class WorkerHirePanel : MonoBehaviour
{
    [Header("데이터")]
    [SerializeField] private WorkerData[] availableWorkers;

    [Header("UI 참조")]
    [SerializeField] private Transform cardContainer;       // 카드들이 붙을 부모 (Vertical Layout Group)
    [SerializeField] private GameObject workerCardPrefab;  // WorkerCard 프리팹
    [SerializeField] private TextMeshProUGUI slotText;     // "일꾼 2 / 5" 표시

    private WorkerCard[] cards;

    void Start()
    {
        BuildCards();
        UpdateSlotText();

        // 일꾼 슬롯이 늘어날 때 슬롯 텍스트 갱신
        RobotManager.Instance.OnWorkerSlotsChanged += _ => UpdateSlotText();
    }

    void OnDestroy()
    {
        if (RobotManager.Instance != null)
            RobotManager.Instance.OnWorkerSlotsChanged -= _ => UpdateSlotText();
    }

    private void BuildCards()
    {
        cards = new WorkerCard[availableWorkers.Length];

        for (int i = 0; i < availableWorkers.Length; i++)
        {
            GameObject go = Instantiate(workerCardPrefab, cardContainer);
            cards[i] = go.GetComponent<WorkerCard>();
            cards[i].Init(availableWorkers[i], this);
        }
    }

    // 고용 버튼 눌렸을 때 WorkerCard가 호출
    public void OnHireClicked(WorkerData data)
    {
        bool success = WorkerManager.Instance.HireWorker(data);

        if (success)
        {
            UpdateSlotText();
            RefreshAllCards();
            Debug.Log($"{data.workerName} 고용 완료");
        }
        else
        {
            Debug.Log("고용 실패: 슬롯 부족 또는 재료 부족");
        }
    }

    public void RefreshAllCards()
    {
        foreach (var card in cards)
            card.Refresh();
    }

    private void UpdateSlotText()
    {
        if (slotText == null) return;
        int current = WorkerManager.Instance.GetWorkerCount();
        int max = RobotManager.Instance.TotalWorkerSlots;
        slotText.text = $"일꾼  {current} / {max}";
    }
}
