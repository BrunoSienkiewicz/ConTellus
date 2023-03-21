using System;
using System.Collections;
using UnityEngine;
using Weapons;

namespace Entities
{
    public class TankEnemy : BaseEnemy
    {
        private GameObject _followedPlayer;
        private Transform _followedPlayerTransform;

        [Header("Movement")] public float maxShootingRange;
        public float shootingBeginRange;

        [Header("Shooting")] public float shootingSpeed;
        public float bulletDamage;
        public float bulletSpeed;
        public float shotsOffset;
        public GameObject aim1;
        public GameObject aim2;

        private float _timeTillNextShot;
        private bool _approachingPlayer;

        public static void Spawn(GameObject player, Vector3 position, Vector3 rotation)
        {
            var enemy = Instantiate(Game.Instance.tankEnemyPrefab, position, Quaternion.Euler(rotation));
            var component = enemy.GetComponent<TankEnemy>();
            component._followedPlayer = player;
            component._followedPlayerTransform = player.GetComponent<Transform>();
        }

        protected override void Awake()
        {
            base.Awake();

            _approachingPlayer = true;
            _timeTillNextShot = 0;
        }

        private void Update()
        {
            var position = _followedPlayerTransform.position;
            //NavAgent.destination = position;
            var distanceToPlayer = Vector3.Distance(position, Transform.position);

            if (_approachingPlayer && distanceToPlayer <= shootingBeginRange)
            {
                _approachingPlayer = false;
            }

            if (!_approachingPlayer && distanceToPlayer > maxShootingRange)
            {
                _approachingPlayer = true;
            }

            // Navigate towards player is set to approach player
            if (_approachingPlayer)
            {
                NavAgent.destination = _followedPlayerTransform.position;
            }

            NavAgent.isStopped = !_approachingPlayer;

            _timeTillNextShot = Math.Max(0, _timeTillNextShot - Time.deltaTime);

            if (distanceToPlayer <= maxShootingRange)
            {
                transform.LookAt(_followedPlayerTransform);
                if (_timeTillNextShot <= 0)
                {
                    _timeTillNextShot = shootingSpeed;
                    Shoot();
                }
            }
        }

        private void Shoot()
        {
            void CreateBullet(Vector3 position, Quaternion rotation)
            {
                var projectileInstance = Instantiate(Game.Instance.enemyBulletPrefab, position, rotation);
                var bulletConfig = projectileInstance.GetComponent<BulletScript>();
                bulletConfig.bulletSpeed = bulletSpeed;
                bulletConfig.bulletDamage = bulletDamage;
            }
            
            IEnumerator SecondShot()
            {
                yield return new WaitForSecondsRealtime(shotsOffset);
                CreateBullet(aim2.transform.position, transform.rotation);
            }
            
            CreateBullet(aim1.transform.position, transform.rotation);
            StartCoroutine(SecondShot());
        }
    }
}