using Base_Components;

namespace Zones
{
    public class StickyZone: Zone_Base
    {
        public float slowCoef = 0.7f;
        protected override void UnitInside(Unit_Base unit)
        {
        }

        protected override void UnitEnter(Unit_Base unit)
        {
            unit.Move.speedModifier = slowCoef;
        }

        protected override void UnitExit(Unit_Base unit)
        {
            unit.Move.speedModifier = 1f;
        }
    }
}