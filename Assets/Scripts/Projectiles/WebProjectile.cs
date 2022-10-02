using UnityEngine;
using Zones;

namespace Projectiles
{
    public class WebProjectile: Projectile_Base
    {
        public GameObject webZone;
        public WebEffect webEffect;
        public float maxDistance = 8f;
        
        private Vector3 initialPosition;

        protected override void Awake()
        {
            initialPosition = transform.position;
            base.Awake();
        }

        protected override void Update()
        {
            if ((transform.position - initialPosition).sqrMagnitude > maxDistance * maxDistance)
            {
                Instantiate(webZone, transform.position, Quaternion.identity);
                OnProjectileLifeEnd();
            }
            base.Update();
        }

        protected override void OnEnemyHit(Unit_Base unit)
        {
            Instantiate(webEffect, unit.transform).Apply(unit);
        }
    }
}