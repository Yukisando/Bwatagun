using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour {
    private NetworkObject no;

    private void Awake() {
        no = GetComponent<NetworkObject>();
    }

    public override void OnNetworkSpawn() {
        Destroy(gameObject, 2f);
    }

    public void Init(Vector3 spawnPosition) {
        transform.position = spawnPosition;
        no.Spawn();

        if (IsServer) {
            Invoke(nameof(Remove), 2f);
        }
    }

    private void Remove() {
        no.Despawn();
    }
}