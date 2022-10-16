using Unity.Netcode;
using UnityEngine;

public class NetworkUI : MonoBehaviour {
    
    public void JoinGame() {
        NetworkManager.Singleton.StartClient();
    }

    public void HostGame() {
        NetworkManager.Singleton.StartHost();
    }
}