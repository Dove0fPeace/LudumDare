namespace Zones
{
    public class SpiderWeb : StickyZone
    {
        protected override void UnitEnter(Unit_Base unit)
        {
            if (unit.Move.InsectType == Insects.Spider || unit.Ability.InsectType == Insects.Spider)
            {
                return;
            }
            base.UnitEnter(unit);
        }
    }
}