using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 일꾼 1명의 고용 카드 UI.
/// WorkerHirePanel이 프리팹 인스턴스화 후 Init()을 호출한다.
/// </summary>
public class WorkerCard : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private Image workerIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI statsText;    // 수집량 / 주기
    [SerializeField] private TextMeshProUGUI costText;     // 고용 비용
    [SerializeField] private Button hireButton;
    [SerializeField] private TextMeshProUGUI hireButtonText;

    private WorkerData data;
    private WorkerHirePanel panel;

    public void Init(WorkerData workerData, WorkerHirePanel ownerPanel)
    {
        data = workerData;
        panel = ownerPanel;

        if (workerIcon != null && data.sprite != null)
            workerIcon.sprite = data.sprite;

        if (nameText != null)
            nameText.text = data.workerName;

        if (statsText != null)
            statsText.text = $"{data.targetResource} +{data.baseCollectAmount:F1} / {data.collectInterval}초";

        hireButton.onClick.AddListener(() => panel.OnHireClicked(data));

        Refresh();
    }

    // 고용 가능 여부에 따라 버튼/비용 텍스트 갱신
    public void Refresh()
    {
        bool canAfford = ResourceManager.Instance.CanAfford(data.hireCost);
        bool hasSlot = WorkerManager.Instance.GetWorkerCount() < RobotManager.Instance.TotalWorkerSlots;
        bool canHire = canAfford && hasSlot;

        hireButton.interactable = canHire;

        if (costText != null)
        {
            string costStr = "";
            foreach (var cost in data.hireCost)
                costStr += $"{cost.resourceType} x{cost.amount:F0}  ";
            costText.text = costStr.TrimEnd();
        }

        if (hireButtonText != null)
        {
            if (!hasSlot)
                hireButtonText.text = "슬롯 부족";
            else if (!canAfford)
                hireButtonText.text = "재료 부족";
            else
                hireButtonText.text = "고용";
        }
    }
}
