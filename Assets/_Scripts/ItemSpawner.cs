using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    [Header("References")]
    [SerializeField] private List<GameObject> _itemPrefabs;
    [SerializeField] private Collider _itemCollectorZoneCollider;
    [SerializeField] private LayerMask _propsLayer;
    [SerializeField] private Transform _itemSpawnParent;

    private float _spawnHeight = 1f;
    private float _safeRadius = 0.5f;
    

    public void SpawnItem() {
        Vector3 spawnPosition = GetSafePosition();
        GameObject randomItemPrefab = _itemPrefabs[Random.Range(0, _itemPrefabs.Count)];
        Instantiate(randomItemPrefab, spawnPosition, Quaternion.identity, _itemSpawnParent);
    }

    private Vector3 GetSafePosition() {
        for (int i = 0; i < 50; i++) {
            // Get random XZ point on the item collector zone
            Vector3 randomPointXZ = GetRandomXZInsideBounds(_itemCollectorZoneCollider.bounds);
        
            // Add height to spawn point
            Vector3 pos = new Vector3(randomPointXZ.x, _itemCollectorZoneCollider.bounds.max.y + _spawnHeight, randomPointXZ.z);
        
            // Check for props overlap
            bool isOverlapping = Physics.CheckSphere(pos, _safeRadius, _propsLayer);

            if (!isOverlapping)
                return pos;
        }
        
        return _itemCollectorZoneCollider.bounds.center + Vector3.up * _spawnHeight;
    }
    
    private Vector3 GetRandomXZInsideBounds(Bounds b)
    {
        float x = Random.Range(b.min.x, b.max.x);
        float z = Random.Range(b.min.z, b.max.z);
        return new Vector3(x, 0, z);
    }

}
