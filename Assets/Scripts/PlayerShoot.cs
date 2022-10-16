using Unity.Netcode;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour {
    public GameObject bulletPrefab;
    public Camera playerCam;

    private Transform dump;

    private void Awake() {
        dump = GameObject.FindWithTag("Dump").transform;
    }

    private void Update() {
        if (!IsOwner) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            Physics.Raycast(playerCam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0)), out var hit);
            if (hit.collider && hit.collider.TryGetComponent(out Zombie z)) {
                ShotZombieServerRPC(z.GetComponent<NetworkObject>().NetworkObjectId);
            }
        }
    }

    [ServerRpc]
    private void ShotZombieServerRPC(ulong zombieID) {
        foreach (var zombie in FindObjectsOfType<NetworkObject>()) {
            if (zombie.NetworkObjectId == zombieID) {
                zombie.Despawn();
                return;
            }
        }
    }
}