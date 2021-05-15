using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public enum RotateDirection { Clockwise, CounterClockwise }

    public enum ChestColors { None, Red, Gold, Silver }

    public enum Rewards { Diamond, Coin, RedKey, SilverKey, GoldKey }

    public enum Currency { Diamond, Coin }

    public enum Boxes { Lightning, Shield, Bullet, Speed, Diamond, Coin, Question }

    public enum TrapTypes { Bomb, Dynamite, Chainsaw };

    public enum ChestPrizeTypes { Coin, Diamond, Skill, Ball };

    public enum BallTypes { Common, Rare, Legendary };
}
