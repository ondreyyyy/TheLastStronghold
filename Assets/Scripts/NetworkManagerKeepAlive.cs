using UnityEngine;
using Unity.Netcode;

public class NetworkManagerKeepAlive : MonoBehaviour
{
    void Awake()
    {
        //если менеджер уже существует дубликат уничтожается
        if (FindObjectsByType<NetworkManagerKeepAlive>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}