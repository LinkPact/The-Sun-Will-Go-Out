using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum LevelObjective
{
    Finish,
    KillPercentage,
    KillPercentageOrSurvive,
    KillNumberOrSurvive,
    KillNumber,
    CountMayNotPass,
    Time,

    Boss            // Can only be created in custom logic
}

public enum Movement
{
    None,
    Line,
    SlantingLine,

    SmallZigzag,
    MediumZigzag,
    BigZigzag,

    SearchAndLockOn,
    
    CrossOver,
    Following,
    Stopping,
    FullStop,
    RightHorizontal,
    LeftHorizontal,
    AI,

    BossStop_X
}

//The different base states for squares.
public enum EnemyType
{
    none                = 0,
    turret              = 2,
    meteor              = 3,
    medium              = 4,
    big_R               = 5,
    homingBullet_R      = 6,
    homingMissile_R     = 7,
    bigMissile_R        = 8,
    smallShooter_R      = 9,

    
    allianceFighterAlly = 10,
    freighterAlly       = 11,
    rebelFighterAlly    = 12,
    
    R_mosquito          = 100,
    R_smallAttack       = 101,
    R_thickShooter      = 102,
    R_smallSniper       = 103,
    R_burster           = 104,
    R_minelayer         = 106,
    R_missileAttackShip = 107,
    
    // To be implemented
    R_lightMinelayer    = 108,
    R_homingMissile     = 109,
    R_bomber            = 110,
    R_fatzo             = 111,

    A_drone             = 200,
    A_smallLaserShip    = 201,
    A_stealthShip       = 202,
    A_shielder          = 203,
    A_attackStealth     = 204,

    // To be implemented
    A_singleHoming      = 205,
    A_lightBeamer       = 206,
    A_multipleShot      = 207,
    A_heavyBeamer       = 208,
    A_ballistic         = 209,
    A_hangar            = 210,

}

//The different base states for squares.
public enum PointEventType
{
    point,      // 0
    line,       // 1
    square,     // 2
    vformation, // 3
    horizontal  // 4
}

public enum DurationEventType
{
    even,       // 0
    gradient    // 1
}

public enum ShipPartType
{ 
    // Primary
    LongShot                = 0,
    BasicLaser              = 1,
    DualLaser               = 2,
    SpreadBullet            = 3,

    Beam                    = 4,
    MultipleShot            = 5,
    WaveBeam                = 6,

    BallisticLaser          = 7,
    FragmentMissile         = 8,
    Burster                 = 9,

    AdvancedLaser           = 10,
    ProximityLaser          = 11,
    AdvancedBeam            = 12,
    FlameShot               = 36,

    // Secondary
    Turret                  = 13,
    FieldDamage             = 14,

    SideMissiles            = 15,
    HomingMissile           = 16,
    Disruptor               = 17,
    
    MineLayer               = 18,

    RegularBomb             = 19,
    PunyTurret              = 20,

    // Plating
    BasicPlating            = 21,
    RegularPlating          = 22,
    AdvancedPlating         = 23,
    HeavyPlating            = 24,
    LightPlating            = 25,

    // Energy cell
    BasicEnergyCell         = 26,
    RegularEnergyCell       = 27,
    AdvancedEnergyCell      = 28,
    WeaponBoostEnergyCell   = 29,
    ShieldBoostEnergyCell   = 30,

    // Shield
    BasicShield             = 31,
    RegularShield           = 32,
    AdvancedShield          = 33,
    CollisionShield         = 34,
    BulletShield            = 35
}

public enum ShipPartAvailability
{ 
    rare,
    uncommon,
    common,
    ubiquitous
}