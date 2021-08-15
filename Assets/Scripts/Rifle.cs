using System;
using System.Collections.Generic;
using UnityEditor;

/**
 * Rifle class extends the Gun class adding firing modes; semi-auto, burst, and auto
 *   - Firing Mode: one of available values in the Firing_Modes Enum
 * TODO implement more functionalilty
 */
public class Rifle : Gun
{
    private uint _rifleCapacity = 24;
    private uint _rifleLoadedAmmunition = 0;
    private Enum _firingMode = FiringModes.SemiAutomatic;
    private readonly Dictionary<Enum, float> _powerToTypeMap = new Dictionary<Enum, float> {
        { FiringModes.SemiAutomatic, 1200f },
        { FiringModes.Burst, 2000f },
        { FiringModes.Automatic, 1200f }
    };
    private readonly Dictionary<Enum, uint> _rateToTypeMap = new Dictionary<Enum, uint>
    {
        { FiringModes.SemiAutomatic, 350 },
        { FiringModes.Burst, 50 },
        { FiringModes.Automatic, 100 }
    };

    public override string PrefabPath => "3D/Prefabs/Rifle";
    public override uint Capacity
    {
        get => _rifleCapacity;
        set => _rifleCapacity = value;
    }
    public override uint LoadedAmmunition => _rifleLoadedAmmunition;
    public override float Power => _powerToTypeMap[FiringMode];

    public override uint DischargeRate
    {
        get => _rateToTypeMap[FiringMode];
    }

    public override Enum FiringMode
    {
        get { return _firingMode; }
        set
        {
            switch (value)
            {
                case FiringModes.SemiAutomatic:
                    _firingMode = value;
                    break;
                case FiringModes.Burst:
                    _firingMode = value;
                    break;
                case FiringModes.Automatic:
                    _firingMode = value;
                    break;
            }
        }
    }
}