using Base_Components;

namespace Zones
{
    public class WebEffect : Zone_Base
    {
        public float slowCoef = 0.7f;
        private Unit_Base unit;
        
        protected override void UnitInside(Unit_Base unit)
        {
        }

        protected override void UnitEnter(Unit_Base unit)
        {
        }

        protected override void UnitExit(Unit_Base unit)
        {
        }

        public void Apply(Unit_Base unit)
        {
            unit.Move.speedModifier = slowCoef;
            this.unit = unit;
        }

        protected override void ZoneRemoved()
        {
            unit.Move.speedModifier = slowCoef;
            base.ZoneRemoved();
        }
    }
}