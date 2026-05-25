using UnityEngine;

public static class GameManager
{
    public static Vector3 lastPlayerPosition;
    public static int lastForestAlert;
    public static float lastShoutTime;
    public static Vector3 defaultPlayerPosition = new Vector3(0f, 0.93f, -6f);
    public static bool hasSavedPosition = false;

    public static void SavePlayerData()
    {
        var player = GameObject.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            lastPlayerPosition = player.transform.position;
            hasSavedPosition = true;
        }

        var shout = GameObject.FindFirstObjectByType<ShoutAbility>();
        if (shout != null)
        {
            // .Value — читаем значение NetworkVariable
            lastForestAlert = shout.forestAlert.Value;
            lastShoutTime = shout.LastShoutTime;
        }
    }

    public static void RestorePlayerData()
    {
        var player = GameObject.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            if (hasSavedPosition)
                player.transform.position = lastPlayerPosition;
            else
                player.transform.position = defaultPlayerPosition;
        }

        var shout = GameObject.FindFirstObjectByType<ShoutAbility>();
        if (shout != null)
        {
            // NetworkVariable писать может только сервер, поэтому через Value не присваиваем напрямую
            // RestoreLastShoutTime оставляем как есть
            shout.RestoreLastShoutTime(lastShoutTime);
        }
    }
}