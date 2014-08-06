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
    Time
}

public enum Movement
{
    None,
    Line,
    SlantingLine,

    SmallZigzag,
    MediumZigzag,
    BigZigzag,
    
    CrossOver,
    Following,
    Stopping,
    FullStop,
    RightHorizontal,
    LeftHorizontal,
    AI
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

    
    fighterAlly         = 10,
    freighterAlly       = 11,
    
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
    BasicLaser          = 1,
    BallisticLaser      = 2,
    DualLaser           = 3,
    SpreadBullet        = 4,
    Beam                = 5,
    MineLayer           = 6,
    MultipleShot        = 7,
    WaveBeam            = 8,
    AdvancedLaser       = 9,
    ProximityLaser      = 10,
    DrillBeam           = 11,
    FlameShot           = 12,
    
    PunyTurret          = 13,
    FragmentMissile     = 14,
    SideMissiles        = 15,
    HomingMissile       = 16,
    RegularBomb         = 17,
    Turret              = 18,

    HeavyPlating        = 19,
    LightPlating        = 20,
    RegularPlating      = 21,

    DurableEnergyCell   = 22,
    PlasmaEnergyCell    = 23,
    RegularEnergyCell   = 24,
    WeaponBoostEnergyCell = 25,
    ShieldBoostEnergyCell = 26,

    DurableShield       = 27,
    PlasmaShield        = 28,
    RegularShield       = 29,
    CollisionShield     = 30,
    BulletShield        = 31
}

public enum ShipPartAvailability
{ 
    rare,
    uncommon,
    common,
    ubiquitous
}