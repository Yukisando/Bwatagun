using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : NetworkBehaviour {
    public float speed = 10f;
    private Transform target;
    private NavMeshAgent ai;

    private Transform currentTarget;

    private void Awake() {
        ai = GetComponent<NavMeshAgent>();
    }

    public override void OnNetworkSpawn() {
        var player = FindObjectOfType<PlayerMovement>();
        if (!player) {
            return;
        }

        target = player.transform;

        if (!IsServer) {
            Destroy(GetComponent<NavMeshAgent>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());
        }
    }

    private void Update() {
        if (target && target != currentTarget) {
            ai.speed = speed;
            ai.SetDestination(target.position);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log($"Kill player: {collision.gameObject.name}");
            OnPlayerTouchedServerRPC(collision.gameObject.name);
        }
    }

    [ServerRpc]
    private void OnPlayerTouchedServerRPC(string playerName) {
        GameObject.Find(playerName).GetComponent<NetworkObject>().Despawn();
    }
}