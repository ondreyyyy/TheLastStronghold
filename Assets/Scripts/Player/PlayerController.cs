using UnityEngine;
using TMPro;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    public ShoutAbility shoutAbility;
    public CollectPromptUI collectPromptUI;
    public PauseMenuUI pauseMenuUI;

    private Rigidbody rb;
    private float cameraPitch = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (shoutAbility == null)
            shoutAbility = GetComponent<ShoutAbility>();

        if (playerCamera == null)
            playerCamera = transform.Find("playerCamera/POVCamera") ?? transform.Find("playerCamera");
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            if (playerCamera != null)
                playerCamera.gameObject.SetActive(false);
            rb.isKinematic = true;
            return;
        }

        if (collectPromptUI == null)
            collectPromptUI = FindFirstObjectByType<CollectPromptUI>();

        if (pauseMenuUI == null)
            pauseMenuUI = FindFirstObjectByType<PauseMenuUI>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.RestorePlayerData();
    }

    void Update()
    {
        if (!IsOwner) return;

        // если ссылка слетела — ищем снова
        if (collectPromptUI == null)
            collectPromptUI = FindFirstObjectByType<CollectPromptUI>();

        if (pauseMenuUI == null)
            pauseMenuUI = FindFirstObjectByType<PauseMenuUI>();

        if (pauseMenuUI != null && pauseMenuUI.IsPauseMenuActive)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        //проверка ресурса для подсказки
        var resource = GetLookedAtResource();
        if (collectPromptUI != null)
            collectPromptUI.ShowPrompt(resource != null);

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryCollectResource(resource);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            shoutAbility.UseShout();
        }
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        if (pauseMenuUI != null && pauseMenuUI.IsPauseMenuActive)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = (transform.right * h + transform.forward * v) * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }

    ResourceBase GetLookedAtResource()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            return hit.collider.GetComponent<ResourceBase>();
        }
        return null;
    }

    void TryCollectResource(ResourceBase resource = null)
    {
        if (resource == null)
            resource = GetLookedAtResource();
        if (resource != null)
        {
            resource.Collect();
        }
    }
}