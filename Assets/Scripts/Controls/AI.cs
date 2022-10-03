using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controls
{
    [RequireComponent(typeof(Unit_Base))]
    public class AI : Control_Base
    {
        private Unit_Base unit;
        public float smartness = 5f;
        public float movePrecision = 1f;
        public Transform target;
        public float wallDistanceCheck = 3f;

        private Coroutine moving;
        private int tangentDir;
        
        private void Awake()
        {
            unit = GetComponent<Unit_Base>();
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
                if (unit.Move.CanDash && IsCastHit(-transform.up, wallDistanceCheck))
                {
                    if (unit.TryDash())
                    {
                        continue;
                    }
                }
                //if (unit.Attack is not MeleeAttack_Base)
                {
                //    Evade();
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
            //works while we are facing the player
            if (!IsCastHit(transform.up, range))
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
            hit = Physics2D.CircleCast(transform.position, 1, transform.right, range);
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
            while ((transform.position - dest).sqrMagnitude > movePrecision*movePrecision)
            {
                unit.TryMove((dest - transform.position).normalized);
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, -transform.up * wallDistanceCheck);
        }
    }
}