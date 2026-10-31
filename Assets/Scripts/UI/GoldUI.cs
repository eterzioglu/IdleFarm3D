using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI goldText;

    private void Start()
    {
        if (goldText != null)
        {
            UpdateGoldText(GoldManager.Instance.Gold);
        }
        GoldManager.Instance.OnGoldChanged += UpdateGoldText;
        UpdateGoldText(GoldManager.Instance.Gold);
    }

    private void UpdateGoldText(int gold)
    {
        if (goldText != null)
        {
            goldText.text =  gold.ToString();
        }
    }

    private void OnDisable()
    {
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged -= UpdateGoldText;
        }
    }
}
