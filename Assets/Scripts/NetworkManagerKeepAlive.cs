using UnityEngine;
using Unity.Netcode;

public class NetworkManagerKeepAlive : MonoBehaviour
{
    void Awake()
    {
        // если NetworkManager уже существует — уничтожаем дубликат
        if (FindObjectsByType<NetworkManagerKeepAlive>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}