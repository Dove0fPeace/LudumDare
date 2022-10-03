using Base_Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zones
{
    public class Poison_Zone : Zone_Base
    {
        public float Damage;
        public float DAmageTickInterval;

        private Timer timer;

        private List<Unit_Base> unitsInZone = new List<Unit_Base>();

        protected override void UnitEnter(Unit_Base unit)
        {
            unitsInZone.Add(unit);
        }

        protected override void UnitExit(Unit_Base unit)
        {
            unitsInZone.Remove(unit);
        }

        protected override void UnitInside(Unit_Base unit)
        {

        }

        private void Start()
        {
            timer = Timer.CreateTimer(DAmageTickInterval, true, true);
            timer.OnTimeRunOut += DamageTick;

        }

        private void DamageTick()
        {
            foreach(Unit_Base unit in unitsInZone)
            {
                unit.TakeDamage(Damage, true);
            }
        }

        private void OnDestroy()
        {
            timer.OnTimeRunOut -= DamageTick;
            timer.Destroy();
        }
    }

}
