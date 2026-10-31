using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SectionUI : MonoBehaviour
{
    [SerializeField] private Section section;
    [SerializeField] private WorkTask workTask;
    [SerializeField] private TMP_Text unlockCostText;
    [SerializeField] private Image progressBar; // Image Type: Filled

    private void Start()
    {
        unlockCostText.text = section.UnlockCost.ToString();
        unlockCostText.gameObject.SetActive(!section.IsUnlocked);
        progressBar.gameObject.SetActive(section.IsUnlocked);   

        section.OnUnlocked += HandleSectionUnlocked;
        workTask.OnStarted += HandleTaskStarted;
        workTask.OnProgressChanged += HandleProgressChanged;
        workTask.OnCompleted += HandleTaskCompleted;               
    }

    private void OnDisable()
    {
        section.OnUnlocked -= HandleSectionUnlocked;
        workTask.OnStarted -= HandleTaskStarted;
        workTask.OnProgressChanged -= HandleProgressChanged;
        workTask.OnCompleted -= HandleTaskCompleted;
    }

    private void HandleSectionUnlocked(Section unlocked)
    {
        if (unlocked != section) return;
        unlockCostText.gameObject.SetActive(false);
    }

    private void HandleTaskStarted()
    {
        progressBar.fillAmount = 0f;
        progressBar.gameObject.SetActive(true);
    }

    private void HandleProgressChanged(float progress) => progressBar.fillAmount = progress;

    private void HandleTaskCompleted() => progressBar.gameObject.SetActive(false);
}