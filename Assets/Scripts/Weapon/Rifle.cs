using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
/**
 * Rifle class extends the Gun class adding firing modes; semi-auto, burst, and auto
 *   - Firing Mode: one of available values in the Firing_Modes Enum
 * TODO implement more functionalilty
 */
    public class Rifle : Gun
    {
        private const float SEMI_DELAY = 0.65f;
        private const float BURST_DELAY = 0.85f;
        private const float AUTO_DELAY = 0.2f;
        
        private Enum _firingMode = FiringModes.Automatic;

        private readonly Dictionary<Enum, float> _modeDelayMapping = new Dictionary<Enum, float>
        {
            { FiringModes.SemiAutomatic, SEMI_DELAY },
            { FiringModes.Burst, BURST_DELAY },
            { FiringModes.Automatic, AUTO_DELAY }
        };

        public override string PrefabPath => "3D/Prefabs/Rifle";

        public override uint Capacity => 24;

        public override float DischargeRate => _modeDelayMapping[FiringMode];

        public override Enum BallisticType
        {
            get => BallisticTypes.Bullet;
            set { }
        }

        public override Enum FiringMode
        {
            get => _firingMode;
            set
            {
                _firingMode = value switch
                {
                    FiringModes.SemiAutomatic => FiringModes.SemiAutomatic,
                    FiringModes.Burst => FiringModes.Burst,
                    FiringModes.Automatic => FiringModes.Automatic,
                    _ => FiringModes.SemiAutomatic
                };
            }
        }
        
        public override float MaximumViewAngleY => (float) Math.PI / 5.5f;

        public override uint Fire()
        {
            var fired = 0u;
            if (LoadedAmmunition < 1) return fired;
            
            if (FiringMode.Equals(FiringModes.Burst))
            {
                if (LoadedAmmunition < 4)
                {
                    fired = LoadedAmmunition * 1;
                    LoadedAmmunition = 0;
                    return fired;
                }

                LoadedAmmunition -= 3;
                fired = 3;
            }
            else
            {
                LoadedAmmunition--;
                fired = 1;
            }

            return fired;
        }
    }
}