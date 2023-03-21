using UnityEngine;

namespace Entities
{
    public class KamikazeEnemy : BaseEnemy
    {
        private GameObject _followedPlayer;
        private Transform _followedPlayerTransform;

        public float explodeDistance = 2f;
        public float explodeRadius = 2f;
        public float explodeDamage = 15f;

        public static void Spawn(GameObject player, Vector3 position, Vector3 rotation)
        {
            var enemy = Instantiate(Game.Instance.kamikazeEnemyPrefab, position, Quaternion.Euler(rotation));
            var component = enemy.GetComponent<KamikazeEnemy>();
            component._followedPlayer = player;
            component._followedPlayerTransform = player.GetComponent<Transform>();
        }

        private void Update()
        {
            var position = _followedPlayerTransform.position;

            NavAgent.destination = position;
            var distance = Vector3.Distance(position, Transform.position);

            if (distance < explodeDistance)
            {
                Explode();
            }
        }

        private void Explode()
        {
            var results = new Collider[30];
            var size = Physics.OverlapSphereNonAlloc(transform.position, explodeRadius, results);

            for (var i = 0; i < size; i++)
            {
                var curr = results[i];
                var entityComp = curr.GetComponent<Entity>();
                if (entityComp == null) continue;
                entityComp.TakeDamage(explodeDamage);
            }
            Kill();
        }
    }
}