using Unity.VisualScripting;
using UnityEngine;

namespace Controls
{
    [RequireComponent(typeof(Unit_Base))]
    public class PlayerControl : Control_Base
    {
        private Unit_Base unit;

        private bool disable;
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
            
            if (Input.GetButtonDown("Jump"))
            {
                unit.TryDash();
            }
            if (Input.GetButtonDown("Fire1"))
            {
                unit.TryAttack();
            }
            if (Input.GetButtonDown("Fire2"))
            {
                unit.TryAbility();
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
    }
}