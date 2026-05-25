using UnityEngine;
using UnityEngine.UI;

public class ForestAlertBarUI : MonoBehaviour
{
    public ShoutAbility shoutAbility;
    public Image fillImage;
    public int maxAlert = 10;

    void Start()
    {
        if (shoutAbility == null)
            shoutAbility = FindFirstObjectByType<ShoutAbility>();
    }

    void Update()
    {
        if (shoutAbility != null && fillImage != null)
        {
            float fill = Mathf.Clamp01((float)shoutAbility.forestAlert.Value / maxAlert);
            fillImage.fillAmount = fill;
        }
    }
}