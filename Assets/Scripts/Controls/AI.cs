using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controls
{
    [RequireComponent(typeof(Unit_Base))]
    public class AI : Control_Base
    {
        private Unit_Base unit;
        public float smartness = 3f;
        public float movePrecision = 1f;
        public Transform target;
        public float wallDistanceCheck = 3f;

        private Coroutine moving;
        private int tangentDir;
        private Transform realUnit;
        
        private void Awake()
        {
            unit = GetComponent<Unit_Base>();
            realUnit = unit.RotationRoot;
        }

        private void OnEnable()
        {
            StartCoroutine(AILoop());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator AILoop()
        {
            var waitForSeconds = new WaitForSeconds(1f / smartness);
            while (true)
            {
                yield return waitForSeconds;
                if (!target.gameObject.activeInHierarchy)
                {
                    if (moving != null) StopCoroutine(moving);
                    continue;
                }
                unit.LookAt(target.position);
                if (unit.Ability.CanUse())
                {
                    unit.Ability.Use();
                    continue;
                }
                if (unit.Dash.CanDash && (IsCastHit(-realUnit.right, wallDistanceCheck) || (unit.CurrentHP/unit.MaxHitPoints) < 0.5f))
                {
                    if (unit.TryDash())
                    {
                        continue;
                    }
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
                StartMove(realUnit.position - realUnit.right);
                return;
            }
            Vector3 dest = realUnit.position - realUnit.right + realUnit.up*30*tangentDir;
            StartMove(dest);
        }

        private void AttackDecision()
        {
            float range = unit.Attack.AttackRange;
            //works while we are facing the player
            if (!IsCastHit(realUnit.right, range))
            {
                StartMove(target.position);
            }
            else
            {
                StartMove(target.position);
                unit.TryAttack();
                tangentDir = Random.Range(0, 2) == 0 ? 1 : -1;
            }
        }

        private bool IsCastHit(Vector3 direction, float range)
        {
            RaycastHit2D hit;
            hit = Physics2D.CircleCast(realUnit.position, 1, direction, range);
            if (hit)
            {
                return true;
            }
            return false;
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
            while ((realUnit.position - dest).sqrMagnitude > movePrecision*movePrecision)
            {
                unit.TryMove((dest - realUnit.position).normalized);
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(realUnit.position, realUnit.position-realUnit.right * wallDistanceCheck);
        }
    }
}