using System.Collections;
using UnityEngine;

namespace Controls
{
    [RequireComponent(typeof(Unit_Base))]
    public class AI : MonoBehaviour
    {
        private Unit_Base unit;
        public float smartness = 5f;
        public float movePrecision = 1f;
        public Transform target;

        private Coroutine moving;
        
        private void Awake()
        {
            unit = GetComponent<Unit_Base>();
            StartCoroutine(AILoop());
        }

        IEnumerator AILoop()
        {
            var waitForSeconds = new WaitForSeconds(1f / smartness);
            while (true)
            {
                yield return waitForSeconds;
                unit.LookAt(target.position);
                MeleeAttackDecision();
            }
        }

        private void MeleeAttackDecision()
        {
            //TODO range
            float range = 3f;
            if ((target.position - transform.position).sqrMagnitude > range * range)
            {
                if (moving != null)
                {
                    StopCoroutine(moving);
                }
                moving = StartCoroutine(Moving(target.position));
            }
            else
            {
                unit.TryAttack();
            }
        }

        IEnumerator Moving(Vector3 dest)
        {
            while ((transform.position - dest).sqrMagnitude > movePrecision*movePrecision)
            {
                unit.TryMove((dest - transform.position).normalized);
                yield return null;
            }
        }
    }
}