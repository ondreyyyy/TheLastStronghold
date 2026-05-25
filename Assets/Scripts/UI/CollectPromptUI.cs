using UnityEngine;
using TMPro;

public class CollectPromptUI : MonoBehaviour
{
    public TMP_Text promptText;

    void Start()
    {
        if (promptText != null)
            promptText.enabled = false;
    }

    public void ShowPrompt(bool show)
    {
        if (promptText != null)
            promptText.enabled = show;
    }
}
