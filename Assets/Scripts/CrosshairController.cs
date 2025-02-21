using UnityEngine;
using TMPro;

public class CrosshairController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject crosshairDot;
    [SerializeField] private TextMeshProUGUI interactionPrompt;

    public void ShowInteractable(InteractionType action, string objectName)
    {
        crosshairDot.SetActive(true);
        interactionPrompt.text = $"{GetActionText(action)} {objectName}";
    }

    public void HideInteractable()
    {
        crosshairDot.SetActive(false);
        interactionPrompt.text = "";
    }

    private string GetActionText(InteractionType action)
    {
        return action switch
        {
            InteractionType.Take => "Take",
            InteractionType.Talk => "Talk to",
            InteractionType.Examine => "Examine",
            InteractionType.Use => "Use",
            _ => ""
        };
    }
}