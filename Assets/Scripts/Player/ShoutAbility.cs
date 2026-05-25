using UnityEngine;
using Unity.Netcode;

public class ShoutAbility : NetworkBehaviour
{
    [Header("Cooldown")]
    public float cooldown = 10f;
    private float lastShoutTime = -10f;

    [Header("Highlight")]
    public GameObject highlightEffectPrefab;
    public float effectHeight = 2f;
    public float effectDestroyTime = 2f;

    [Header("Shout Settings")]
    public float radius = 15f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip shoutClip;

    // NetworkVariable — значение forestAlert синхронизируется всем клиентам автоматически
    public NetworkVariable<int> forestAlert = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public float LastShoutTime => lastShoutTime;
    public bool CanShout => Time.time - lastShoutTime > cooldown;

    public void UseShout()
    {
        float timeSinceLast = Time.time - lastShoutTime;
        if (timeSinceLast < cooldown)
        {
            float timeLeft = cooldown - timeSinceLast;
            Debug.Log($"The shout is on cooldown! {timeLeft:F1} seconds left.");
            return;
        }

        lastShoutTime = Time.time;

        // отправляем запрос на сервер
        UseShoutServerRpc();
    }

    // ServerRpc — вызывается клиентом, выполняется на сервере
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void UseShoutServerRpc()
    {
        // сервер увеличивает счётчик — NetworkVariable сам разошлёт всем
        forestAlert.Value++;

        // сервер рассылает эффекты всем клиентам
        UseShoutClientRpc();
        Debug.Log("Shout! Resources are highlighted");
    }

    // ClientRpc — вызывается сервером, выполняется на всех клиентах
    [ClientRpc]
    void UseShoutClientRpc()
    {
        HighlightResources();
        PlayShoutSound();
        TriggerResourceAnimation();
    }

    void HighlightResources()
    {
        GameObject[] resources = GameObject.FindGameObjectsWithTag("Resource");
        foreach (GameObject resource in resources)
        {
            Collider col = resource.GetComponent<Collider>();
            if (col == null) continue;
            Vector3 center = col.bounds.center;
            GameObject effect = Instantiate(
                highlightEffectPrefab,
                center,
                Quaternion.identity
            );
            Destroy(effect, effectDestroyTime);
        }
    }

    void PlayShoutSound()
    {
        if (audioSource == null || shoutClip == null)
            return;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(0.8f, 1f);
        audioSource.PlayOneShot(shoutClip);
    }

    void TriggerResourceAnimation()
    {
        GameObject[] resources = GameObject.FindGameObjectsWithTag("Resource");
        foreach (GameObject resource in resources)
        {
            //keyframe анимация
            Animator anim = resource.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Shaken");
            }

            //процедурная анимация
            ResourcePulseReaction pulse = resource.GetComponent<ResourcePulseReaction>();
            if (pulse != null)
            {
                pulse.Pulse();
            }
        }
    }

    public void RestoreLastShoutTime(float value)
    {
        lastShoutTime = value;
    }
}