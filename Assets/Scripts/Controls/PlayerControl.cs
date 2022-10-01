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
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            unit.TryMove(input);
            
            if (Input.GetAxisRaw("Fire3") != 0)
            {
                unit.TryDash();
            }
            if (Input.GetAxisRaw("Fire1") != 0)
            {
                unit.TryAttack();
            }
            if (Input.GetAxisRaw("Fire2") != 0)
            {
                unit.TryAbility();
            }
        }
    }
}