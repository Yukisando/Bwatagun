using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace {
    public class EnemySpawner : NetworkBehaviour {
        public float spawnRate;
        public GameObject zombiePrefab;
        public List<Transform> spawnPoints;

        public override void OnNetworkSpawn() {
            if (!IsHost) {
                return;
            }

            InvokeRepeating(nameof(SpawnEnemy), 1f, spawnRate);
        }

        private void SpawnEnemy() {
            var zombie = Instantiate(zombiePrefab).GetComponent<NetworkObject>();
            zombie.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
            zombie.Spawn();
        }
    }
}