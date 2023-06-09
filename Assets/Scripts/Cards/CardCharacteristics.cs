namespace FTS.Cards
{
    public enum DraftArchetypes
    {
        Offence, Regeneragtion, DamageOverTime, Knowledge, ArmourPiercing, PowerUp, Debuffing, Discard, OnDraw
    }

    public enum EffectType
    {
        Attack, Heal, Armour, Move, Draw, Enhancement, Build
    }

    public enum CardRarity
    {
        Common, Uncommon, Rare, Ledendary
    }
    public enum CardType
    {
        Weapon, Ability, Summon
    }
    public enum CardLocation
    {
        Deck, Hand, Discard, Atomized
    }
    public enum CardTargeting
    {
        None, Unit, Ground, Projectile, Piercing, Trajectory
    }
}
