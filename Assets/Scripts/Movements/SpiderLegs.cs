public class SpiderLegs : Move_Base
{
    public override Insects InsectType => Insects.Spider;

    public override bool Dash()
    {
        print("Spider web dash");
        return base.Dash();
    }
}
