using System;
using UnityEngine;

/**
 * Gun class is the base-level class from the ProjectileWeapon abstract class
 */
public class Gun : ProjectileWeapon
{
    private readonly Enum _gunBallisticType = BallisticTypes.Bullet;
    private uint _gunLoadedAmmunition;
    
    public override uint LoadedAmmunition
    {
        get => _gunLoadedAmmunition;
        set { }
    }

    public override string PrefabPath => "3D/Prefabs/Gun";

    public override float Power
    {
        get { return Power; }
    }
    // TODO add power set accessor when other BallisticTypes can be used for this class

    public override Enum FiringMode
    {
        get { return FiringModes.SemiAutomatic; }
    }
    public override Enum BallisticType => _gunBallisticType;

    public override void Reload()
    {
        if (LoadedAmmunition == 0)
        {
            _gunLoadedAmmunition = Capacity * 1;
        }
    }
}