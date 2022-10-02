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
        private int tangentDir;
        
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
                if (target is null)
                {
                    StopCoroutine(moving);
                    yield break;
                }
                unit.LookAt(target.position);
                if (unit.Attack is not MeleeAttack_Base)
                {
                    StartMove(transform.position*2-target.position);
                }
                if (unit.Attack.CanAttack)
                {
                    AttackDecision();
                }
                else
                {
                    Evade();
                }
            }
        }

        private void Evade()
        {
            if (Random.Range(0, 3) == 0)
            {
                StartMove(transform.position - transform.up);
                return;
            }
            Vector3 dest = transform.position - transform.up + transform.right*30*tangentDir;
            StartMove(dest);
        }

        private void AttackDecision()
        {
            float range = unit.Attack.AttackRange;
            if ((target.position - transform.position).sqrMagnitude > range * range)
            {
                StartMove(target.position);
            }
            else
            {
                unit.TryAttack();
                tangentDir = Random.Range(0, 2) == 0 ? 1 : -1;
            }
        }

        private void StartMove(Vector3 destination)
        {
            if (moving != null)
            {
                StopCoroutine(moving);
            }

            moving = StartCoroutine(Moving(destination));
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