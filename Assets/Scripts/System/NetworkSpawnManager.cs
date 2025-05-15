using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

namespace XRMultiplayer
{
    public class NetworkSpawnManager : NetworkBehaviour
    {
        #region Serialized Fields
        [Header("Spawn Points")]
        [SerializeField] private Transform[] spawnPoints;
        
        [Header("References")]
        [SerializeField] private CharacterResetter characterResetter;
        #endregion

        #region Private Fields
        private Dictionary<ulong, Transform> playerSpawnPoints = new Dictionary<ulong, Transform>();
        #endregion

        #region Unity Lifecycle Methods
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
            
            // Only process for our local player
            if (playerId != NetworkManager.Singleton.LocalClientId)
            {
                Debug.Log($"Ignoring player {playerId} as it's not our local player ({NetworkManager.Singleton.LocalClientId})");
                return;
            }
            
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
            Debug.Log($"Assigning spawn point for player {playerId} (local player: {NetworkManager.Singleton.LocalClientId})");
            
            // Make sure we have at least 2 spawn points
            if (spawnPoints.Length < 2)
            {
                Debug.LogError("At least 2 spawn points are required for server/client distinction.");
                return;
            }
            
            // Determine if this player is the server
            bool isServerPlayer = playerId == NetworkManager.ServerClientId;
            
            // Get the appropriate spawn point - index 0 for server, index 1 for client
            Transform spawnPoint = isServerPlayer ? spawnPoints[0] : spawnPoints[1];
            
            // Add to player spawn points dictionary
            playerSpawnPoints[playerId] = spawnPoint;
            
            Debug.Log($"Player {playerId} is {(isServerPlayer ? "SERVER" : "CLIENT")} - spawning at {(isServerPlayer ? "spawn point 1" : "spawn point 2")}");
            
            // Find the character resetter if not assigned
            if (characterResetter == null)
            {
                if (XRINetworkGameManager.Instance.GetPlayerByID(playerId, out XRINetworkPlayer player))
                {
                    characterResetter = player.GetComponentInChildren<CharacterResetter>();
                    if (characterResetter == null)
                    {
                        Debug.LogError("CharacterResetter not found on player");
                        return;
                    }
                }
                else
                {
                    Debug.LogWarning($"Could not find player object for ID {playerId}");
                    return;
                }
            }
            
            // Configure the CharacterResetter based on whether this is server or client
            if (isServerPlayer)
            {
                // Server configuration
                characterResetter.SetOnlinePosition(spawnPoint.position);
                characterResetter.SetResetDistance(150f);
                Debug.Log("Configured CharacterResetter for SERVER player");
            }
            else
            {
                // Client configuration
                characterResetter.SetOnlinePosition(spawnPoint.position);
                characterResetter.SetResetDistance(200f);
                Debug.Log("Configured CharacterResetter for CLIENT player");
            }
            
            // Reset the player to apply new settings
            characterResetter.ResetPlayer();
        }
        
        private void ReleaseSpawnPoint(ulong playerId)
        {
            playerSpawnPoints.Remove(playerId);
        }
        
        /// <summary>
        /// Method to manually teleport a player to their assigned spawn point
        /// </summary>
        public void RespawnPlayer(ulong playerId)
        {
            // Only respawn if it's our local player
            if (playerId != NetworkManager.Singleton.LocalClientId)
            {
                return;
            }
            
            if (playerSpawnPoints.TryGetValue(playerId, out Transform spawnPoint) &&
                XRINetworkGameManager.Instance.GetPlayerByID(playerId, out XRINetworkPlayer player))
            {
                CharacterResetter resetter = player.GetComponentInChildren<CharacterResetter>();
                if (resetter != null)
                {
                    resetter.ResetPlayer();
                }
            }
        }
        #endregion
    }
}