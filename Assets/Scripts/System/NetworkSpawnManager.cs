using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

namespace XRMultiplayer
{
    public class NetworkSpawnManager : NetworkBehaviour
    {
        #region Serialized Fields
        [Header("Spawn Points")]
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private bool randomizeSpawnPoints = true;
        
        [Header("References")]
        [SerializeField] private GameObject xrOrigin;
        #endregion

        #region Private Fields
        private TeleportationProvider teleportationProvider;
        private List<Transform> availableSpawnPoints = new List<Transform>();
        private Dictionary<ulong, Transform> playerSpawnPoints = new Dictionary<ulong, Transform>();
        #endregion

        #region Unity Lifecycle Methods
        private void Awake()
        {
            InitializeSpawnPoints();
            FindXROriginIfNeeded();
            GetTeleportationProvider();
        }
        
        private void Start()
        {
            RegisterEvents();
        }
        
        private void OnDestroy()
        {
            UnregisterEvents();
        }
        #endregion

        #region Initialization Methods
        private void InitializeSpawnPoints()
        {
            // Initialize available spawn points
            availableSpawnPoints = new List<Transform>(spawnPoints);
            
            if (randomizeSpawnPoints)
            {
                // Shuffle the spawn points
                availableSpawnPoints = availableSpawnPoints.OrderBy(x => Random.value).ToList();
            }
        }

        private void FindXROriginIfNeeded()
        {
            // Find XR Origin if not assigned
            if (xrOrigin == null)
            {
                xrOrigin = FindFirstObjectByType<Unity.XR.CoreUtils.XROrigin>()?.gameObject;
                if (xrOrigin != null)
                {
                    Debug.Log("Found XR Origin automatically: " + xrOrigin.name);
                }
            }
        }

        private void GetTeleportationProvider()
        {
            // Get TeleportationProvider from XR Origin
            if (xrOrigin != null)
            {
                teleportationProvider = xrOrigin.GetComponentInChildren<TeleportationProvider>();
                if (teleportationProvider != null)
                {
                    Debug.Log("Found TeleportationProvider on XR Origin");
                }
                else
                {
                    Debug.LogWarning("No TeleportationProvider found on XR Origin");
                }
            }
        }

        private void RegisterEvents()
        {
            if (XRINetworkGameManager.Instance != null)
            {
                XRINetworkGameManager.Instance.playerStateChanged += OnPlayerStateChanged;
            }
        }

        private void UnregisterEvents()
        {
            if (XRINetworkGameManager.Instance != null)
            {
                XRINetworkGameManager.Instance.playerStateChanged -= OnPlayerStateChanged;
            }
        }
        #endregion

        #region Event Handlers
        private void OnPlayerStateChanged(ulong playerId, bool isConnected)
        {
            Debug.Log($"Player {playerId} state changed: {(isConnected ? "Connected" : "Disconnected")}");
            
            if (isConnected)
            {
                // Player joined - assign a spawn point
                AssignSpawnPoint(playerId);
            }
            else
            {
                // Player left - release spawn point
                ReleaseSpawnPoint(playerId);
            }
        }
        #endregion

        #region Spawn Point Management
        private void AssignSpawnPoint(ulong playerId)
        {
            Debug.Log($"Assigning spawn point for player {playerId}");
            
            // Make sure we have spawn points available
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("No available spawn points for player: " + playerId);
                return;
            }
            
            // Get the first available spawn point
            Transform spawnPoint = availableSpawnPoints[0];
            availableSpawnPoints.RemoveAt(0);
            playerSpawnPoints[playerId] = spawnPoint;
            
            // Move the player to the spawn point
            if (XRINetworkGameManager.Instance.GetPlayerByID(playerId, out XRINetworkPlayer player))
            {
                Debug.Log($"Teleporting player {playerId} to spawn point at {spawnPoint.position}");
                TeleportPlayerToSpawnPoint(player, spawnPoint);
                
                // Check position after teleport
                StartCoroutine(VerifyPlayerPosition(player, spawnPoint));
            }
            else
            {
                Debug.LogWarning($"Could not find player object for ID {playerId}");
            }
        }
        
        private void ReleaseSpawnPoint(ulong playerId)
        {
            if (playerSpawnPoints.TryGetValue(playerId, out Transform spawnPoint))
            {
                availableSpawnPoints.Add(spawnPoint);
                playerSpawnPoints.Remove(playerId);
            }
        }
        
        /// <summary>
        /// Method to manually teleport a player to their assigned spawn point
        /// </summary>
        public void RespawnPlayer(ulong playerId)
        {
            if (playerSpawnPoints.TryGetValue(playerId, out Transform spawnPoint) &&
                XRINetworkGameManager.Instance.GetPlayerByID(playerId, out XRINetworkPlayer player))
            {
                TeleportPlayerToSpawnPoint(player, spawnPoint);
            }
        }
        #endregion

        #region Teleportation
        private void TeleportPlayerToSpawnPoint(XRINetworkPlayer player, Transform spawnPoint)
        {
            Debug.Log($"Attempting to teleport player to {spawnPoint.position}");
            
            // Use XR Origin's TeleportationProvider if available
            if (teleportationProvider != null)
            {
                Debug.Log("Using XR Origin's TeleportationProvider for teleportation");
                
                TeleportRequest teleportRequest = new TeleportRequest
                {
                    destinationPosition = spawnPoint.position,
                    destinationRotation = spawnPoint.rotation
                };
                
                if (!teleportationProvider.QueueTeleportRequest(teleportRequest))
                {
                    Debug.LogError("Failed to queue teleport request through XR Origin");
                    // Fall back to direct positioning as a last resort
                    player.transform.position = spawnPoint.position;
                    player.transform.rotation = spawnPoint.rotation;
                }
                return;
            }
            
            // Fallback to player's TeleportationProvider if XR Origin's is not available
            TeleportationProvider playerTeleportProvider = player.GetComponentInChildren<TeleportationProvider>();
            
            if (playerTeleportProvider != null)
            {
                Debug.Log($"Found TeleportationProvider on player - teleporting via request");
                
                TeleportRequest teleportRequest = new TeleportRequest
                {
                    destinationPosition = spawnPoint.position,
                    destinationRotation = spawnPoint.rotation
                };
                
                if (!playerTeleportProvider.QueueTeleportRequest(teleportRequest))
                {
                    Debug.LogError($"Failed to queue teleport request for player {player.name}");
                    player.transform.position = spawnPoint.position;
                    player.transform.rotation = spawnPoint.rotation;
                }
            }
            else
            {
                Debug.LogWarning($"No TeleportationProvider found - direct positioning");
                player.transform.position = spawnPoint.position;
                player.transform.rotation = spawnPoint.rotation;
            }
        }
        
        private IEnumerator VerifyPlayerPosition(XRINetworkPlayer player, Transform spawnPoint)
        {
            yield return null;
            Debug.Log($"Player position after teleport: {player.transform.position}, expected: {spawnPoint.position}");
            
            // Force position again after a frame
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
            
            yield return new WaitForSeconds(0.5f);
            Debug.Log($"Player position after delay: {player.transform.position}");
        }
        #endregion
    }
}