using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    [Header("References")]
    [SerializeField] private ItemSpawner _itemSpawner;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _itemSpawner.SpawnItem();
        _itemSpawner.SpawnItem();
        _itemSpawner.SpawnItem();
        _itemSpawner.SpawnItem();
        _itemSpawner.SpawnItem();
    }
}
