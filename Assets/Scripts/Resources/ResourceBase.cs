using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Collider))]
public class ResourceBase : NetworkBehaviour
{
    public string resourceName;
    public int value;

    public virtual void Collect()
    {
        if (!IsServer)
        {
            CollectServerRpc();
            return;
        }
        Debug.Log($"Collected {resourceName}, value: {value}");
        GetComponent<NetworkObject>().Despawn();
    }

    [Rpc(SendTo.Server)]
    void CollectServerRpc()
    {
        Collect();
    }
}