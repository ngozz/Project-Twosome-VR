using UnityEngine;
using Unity.XR.CoreUtils;
using XRMultiplayer;

public class Test : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private KeyCode teleportKey = KeyCode.T;
    
    [Header("References")]
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private bool findPlayerAutomatically = true;
    
    private void Start()
    {
        if (findPlayerAutomatically)
        {
            // Try to find player automatically
            if (XRINetworkPlayer.LocalPlayer != null)
            {
                playerRigidbody = XRINetworkPlayer.LocalPlayer.GetComponent<Rigidbody>();
                if (playerRigidbody != null)
                {
                    Debug.Log("Found player Rigidbody automatically");
                }
            }
            
            // If still not found, try XR Origin
            if (playerRigidbody == null)
            {
                XROrigin xrOrigin = FindObjectOfType<XROrigin>();
                if (xrOrigin != null)
                {
                    playerRigidbody = xrOrigin.GetComponent<Rigidbody>();
                    if (playerRigidbody == null)
                    {
                        playerRigidbody = xrOrigin.gameObject.AddComponent<Rigidbody>();
                        playerRigidbody.isKinematic = true; // So physics doesn't affect the XR Rig
                    }
                }
            }
        }
    }
    
    // Can be called directly from a UI button's onClick event
    public void TeleportPlayer()
    {
        if (teleportDestination == null)
        {
            Debug.LogError("No teleport destination assigned!");
            return;
        }
        
        if (playerRigidbody == null)
        {
            Debug.LogError("No player Rigidbody assigned!");
            return;
        }
        
        Debug.Log($"Teleporting player to {teleportDestination.position}");
        playerRigidbody.position = teleportDestination.position;
        
        // Also set rotation if needed
        playerRigidbody.rotation = teleportDestination.rotation;
    }
}