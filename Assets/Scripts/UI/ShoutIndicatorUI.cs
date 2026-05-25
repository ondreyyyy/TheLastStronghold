using UnityEngine;
using UnityEngine.UI;

public class ShoutIndicatorUI : MonoBehaviour
{
    public Image fillImage;
    public ShoutAbility shoutAbility;

    void Start()
    {
        if (shoutAbility == null)
            shoutAbility = FindFirstObjectByType<ShoutAbility>();
    }

    void Update()
    {
        if (shoutAbility == null || fillImage == null) return;

        float cooldown = shoutAbility.cooldown;
        float timeSinceLast = Time.time - shoutAbility.LastShoutTime;
        float fill = Mathf.Clamp01(timeSinceLast / cooldown);

        fillImage.fillAmount = fill;

        fillImage.color = fill >= 1f ? Color.white : new Color(1, 1, 1, 0.5f);
    }
}