namespace Zones
{
    public class SpiderWeb : StickyZone
    {
        protected override void UnitEnter(Unit_Base unit)
        {
            //We should have a list of Insect Types in Unit_Base, this is kinda hardcode
            if (unit.Move.InsectType == Insects.Spider || unit.Ability.Insect == Insects.Spider)
            {
                return;
            }
            base.UnitEnter(unit);
        }
    }
}