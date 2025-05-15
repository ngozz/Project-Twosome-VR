using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private Transform hostSpawnPoint; // Spawn point for the host
    [SerializeField]
    private Transform clientSpawnPoint; // Spawn point for the client

    private float spawnLockTime = 1.0f; // Time to lock the player's position after spawn
    private float spawnTimer = 0.0f; // Timer to track the spawn lock time
    private bool isSpawnLocked = false; // Flag to track if the player's position is locked
}