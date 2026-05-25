using UnityEngine;
using Unity.Netcode;

public class ResourceSpawner : NetworkBehaviour
{
    public GameObject woodPrefab;
    public GameObject stonePrefab;
    public GameObject ropePrefab;

    public float[] zoneRadii = new float[4] { 5f, 15f, 30f, 25f };

    public Vector3 spawnAreaCenter = Vector3.zero;

    public int baseSpawnPoints = 30;
    public int spawnPointsPerLevel = 10;
    public int baseLevel = 1;

    public int woodCost = 1;
    public int stoneCost = 2;
    public int ropeCost = 4;

    // спавн только на сервере после того как сеть готова
    public override void OnNetworkSpawn()
    {
        if (IsServer)
            SpawnResources();
    }

    public void SpawnResources()
    {
        int totalPoints = baseSpawnPoints + spawnPointsPerLevel * (baseLevel - 1);
        float zone1Ratio = 0.5f;
        float zone2Ratio = 0.35f;
        //zone1
        int zone1Points = Mathf.Max(woodCost, Mathf.RoundToInt(totalPoints * zone1Ratio));
        int woodCountZone1 = Mathf.Max(1, zone1Points / woodCost);
        SpawnInRing(woodPrefab, woodCountZone1, zoneRadii[0], zoneRadii[1]);
        //zone2
        int zone2Points = Mathf.Max(stoneCost + woodCost, Mathf.RoundToInt(totalPoints * zone2Ratio));
        int minWood2 = woodCost;
        int minStone2 = stoneCost;
        int woodPoints2 = Random.Range(minWood2, zone2Points - minStone2 + 1);
        int stonePoints2 = zone2Points - woodPoints2;
        int woodCountZone2 = Mathf.Max(1, woodPoints2 / woodCost);
        int stoneCountZone2 = Mathf.Max(1, stonePoints2 / stoneCost);
        SpawnInRing(woodPrefab, woodCountZone2, zoneRadii[1], zoneRadii[2]);
        SpawnInRing(stonePrefab, stoneCountZone2, zoneRadii[1], zoneRadii[2]);
        //zone3
        int zone3Points = Mathf.Max(ropeCost, Mathf.RoundToInt(totalPoints - zone1Points - zone2Points));
        int ropeCountZone3 = Mathf.Max(1, zone3Points / ropeCost);
        SpawnInRing(ropePrefab, ropeCountZone3, zoneRadii[2], zoneRadii[3]);
    }

    void SpawnInRing(GameObject prefab, int count, float innerRadius, float outerRadius)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Mathf.Sqrt(Random.Range(innerRadius * innerRadius, outerRadius * outerRadius));
            float x = spawnAreaCenter.x + radius * Mathf.Cos(angle);
            float z = spawnAreaCenter.z + radius * Mathf.Sin(angle);
            float y = spawnAreaCenter.y + 0.5f;
            Vector3 pos = new Vector3(x, y, z);

            // спавним через NetworkObject чтобы ресурс появился у всех клиентов
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            obj.GetComponent<NetworkObject>().Spawn();
        }
    }
}