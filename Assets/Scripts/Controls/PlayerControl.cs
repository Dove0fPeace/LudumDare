using UnityEngine;

namespace Controls
{
    [RequireComponent(typeof(Unit_Base))]
    public class PlayerControl : MonoBehaviour
    {
        private Unit_Base unit;
        private void Awake()
        {
            unit = GetComponent<Unit_Base>();
        }

        private void Update()
        {
            var targetLookPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            unit.LookAt(targetLookPos);
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            unit.TryMove(input);
            
            if (Input.GetButtonDown("Fire3"))
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
    }
}