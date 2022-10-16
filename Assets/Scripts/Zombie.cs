using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : NetworkBehaviour {
    private Player currentTarget;
    private NavMeshAgent ai;

    private void Awake() {
        ai = GetComponent<NavMeshAgent>();
    }

    public override void OnNetworkSpawn() {
        if (!IsServer) {
            Destroy(GetComponent<NavMeshAgent>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());
        }
    }

    private void Update() {
        if (IsServer) {
            FindPlayer();
        }
    }

    private void FindPlayer() {
        currentTarget = FindObjectOfType<Player>();
        if (currentTarget) {
            ai.SetDestination(currentTarget.transform.position);
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