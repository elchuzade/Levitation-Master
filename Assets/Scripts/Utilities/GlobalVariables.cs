using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public enum RotateDirection { Clockwise, CounterClockwise }

    public enum ChestColors { None, Red, Gold, Silver }

    public enum Rewards { Diamond, Coin, RedKey, SilverKey, GoldKey }

    public enum Currency { Diamond, Coin }

    public enum Boxes { Lightning, Shield, Diamond, Coin, Question }

    public enum Buff { Lightning, Shield };

    public enum TrapType { Bomb, Dynamite, Chainsaw };

    public enum SkillType { Bullet, Lightning, Shield };
}
