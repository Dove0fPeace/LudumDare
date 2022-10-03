using Base_Components;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Zones
{

    public class PoisonZone : Zone_Base
    {
        public float Damage;
        public float TickRate;

        private Timer timer;
        private List<Unit_Base> Units = new List<Unit_Base>();

        private void Start()
        {
            timer = Timer.CreateTimer(TickRate, true,true);
            timer.OnTimeRunOut += DamageUnits;
        }
        protected override void UnitEnter(Unit_Base unit)
        {
            Units.Add(unit);
        }

        protected override void UnitExit(Unit_Base unit)
        {
            Units.Remove(unit);
        }

        protected override void UnitInside(Unit_Base unit)
        {
        }

        private void OnDestroy()
        {
            timer.OnTimeRunOut -= DamageUnits;
            timer.Destroy();
        }

        private void DamageUnits()
        {
            foreach(Unit_Base unit in Units)
            {
                unit.TakeDamage(Damage, true);
            }
        }
    }
}

