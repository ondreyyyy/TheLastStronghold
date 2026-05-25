using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class MainMenuUI : MonoBehaviour
{
    public TMP_InputField ipInputField;

    public void StartGame()
    {
        Debug.Log("StartGame called");
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    //новая кнопка подключиться к хосту по айпи
    public void JoinGame()
    {
        string ip = (ipInputField != null && ipInputField.text != "")
            ? ipInputField.text
            : "127.0.0.1";

        Debug.Log($"JoinGame called, connecting to {ip}");

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(ip, 7777);

        NetworkManager.Singleton.StartClient();
        //сцена грузится автоматически от хоста
    }
}