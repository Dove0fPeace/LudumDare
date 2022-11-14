using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controls
{
    [RequireComponent(typeof(Unit_Base))]
    public class PlayerControl : Control_Base
    {
        private Unit_Base unit;
        [HideInInspector]public Unit_Base CurrentUnit => unit;

        private bool disable;

        private bool canDashUse = true;
        private bool canAttackUse = true;
        private bool canAbilityUse = true;
        private void Awake()
        {
            unit = GetComponent<Unit_Base>();
        }

        private void Update()
        {
            if (!GameLoop.Instance.GameOn)
            {
                return;
            }
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if(disable)
            {
                input = Vector2.zero;
            }
            unit.TryMove(input);
            
            if (Input.GetButtonDown("Jump") && canDashUse)
            {
                unit.TryDash();
                StartCoroutine(DashRepeatDelay());
            }
            if (Input.GetButtonDown("Fire1") && canAttackUse)
            {
                unit.TryAttack();
                StartCoroutine(AttackRepeatDelay());
            }
            if (Input.GetButtonDown("Fire2") && canAbilityUse)
            {
                unit.TryAbility();
                StartCoroutine(AbilityRepeatDelay());
            }
        }

        private void FixedUpdate()
        {
            var targetLookPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            unit.LookAt(targetLookPos);          
        }

        private void OnEnable()
        {
            disable = false;
        }
        private void OnDisable()
        {
            unit.TryMove(Vector2.zero);
            disable = true;
        }

        private IEnumerator AttackRepeatDelay()
        {
            canAttackUse = false;
            yield return new WaitForSeconds(0.2f);
            canAttackUse = true;
        }
        
        private IEnumerator AbilityRepeatDelay()
        {
            canAbilityUse = false;
            yield return new WaitForSeconds(0.2f);
            canAbilityUse = true;
        }
        
        private IEnumerator DashRepeatDelay()
        {
            canDashUse = false;
            yield return new WaitForSeconds(0.2f);
            canDashUse = true;
        }
    }
}