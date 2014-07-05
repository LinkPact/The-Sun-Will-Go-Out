﻿using System;
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
    Zigzag,
    //FollowingPlayer,
    //FollowingAll,
    CrossOver,
    //CrossOver2,
    //SweepingShooting,
    Following,
    Stopping,
    AI
}

//The different base states for squares.
public enum EnemyType
{
    none                = 0,
    green               = 1,
    blue                = 2,
    turret              = 3,
    meteor              = 4,
    fighterAlly         = 5,
    freighterAlly       = 6,
    
    R_red               = 100,
    R_yellow            = 101,
    R_medium            = 102,
    R_minelayer         = 103,
    
    
    A_big_I             = 200,
    A_homingBullet      = 201,
    A_homingMissile     = 202,
    A_bigMissile        = 203,
    A_stealthShip       = 204,
    A_smallShooter      = 205,
    A_smallAttack       = 206
}

//The different base states for squares.
public enum PointEventType
{
    point,      // 0
    line,       // 1
    square,     // 2
    vformation  // 3
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