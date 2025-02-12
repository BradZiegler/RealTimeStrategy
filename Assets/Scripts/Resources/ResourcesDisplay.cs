using System;
using Mirror;
using TMPro;
using UnityEngine;

public class ResourcesDisplay : MonoBehaviour {
    [SerializeField] private TMP_Text resourcesText = null;

    private RTSPlayer player;

    private void Update() {
        if (player == null) {
            if (NetworkClient.connection != null && NetworkClient.connection.identity != null) {
                player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
            }

            if (player != null) {
                ClientHandleResourcesUpdated(player.GetResources());

                player.ClientOnResourcesUpdated += ClientHandleResourcesUpdated;
            }
        }
    }

    private void OnDestroy() {
        player.ClientOnResourcesUpdated -= ClientHandleResourcesUpdated;
    }

    private void ClientHandleResourcesUpdated(int resources) {
        resourcesText.text = $"Resources: {resources}";
    }
}
