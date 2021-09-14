using System;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Weapon
{
/**
 * RailGun class extends the Rifle class using rechargeable energy instead of capacity
 *   - Energy: a number corresponding to a percentage of capacity
 *   - Recharge Rate: how fast energy is restored
 * TODO implement more functionalilty
 */
    public class RailGun : Rifle
    {
        private const float DEFAULT_DELAY = 1.5f;
        private const float SEMI_MEDIUM_DELAY = 0.45f;
        private const float AUTO_SMALL_DELAY = 0.09f;
        private const float AUTO_MEDIUM_DELAY = 0.16f;
        private const float AUTO_LARGE_DELAY = 0.3f;
        private const float BLAST_DELAY = 0.995f;

        private const float RECHARGE_RATE = 0.25f;
        private const float RECHARGE_AMOUNT = 1.125f;
        
        private float _energy = 100.0f;
        private float _nextRecharge;
        private Enum _firingMode = FiringModes.Automatic;
        private Enum _ballisticType = BallisticTypes.ElectroEnergy_MediumCaliber;

        public override string PrefabPath => "3D/Prefabs/RailGun";
        
        public override Enum FiringMode
        {
            get => _firingMode;
            set
            {
                // check if switching to burst, if so then change ballistic type to large caliber
                if (FiringModes.Burst.Equals(value))
                {
                    _ballisticType = BallisticTypes.ElectroEnergy_LargeCaliber;
                }

                _firingMode = value switch
                {
                    FiringModes.SemiAutomatic => FiringModes.SemiAutomatic,
                    FiringModes.Automatic => FiringModes.Automatic,
                    FiringModes.Burst => FiringModes.Burst,
                    _ => FiringModes.SemiAutomatic
                };
            }
        }
        
        public override Enum BallisticType
        {
            get => _ballisticType;
            set
            {
                // check if firing mode is set to burst, then switch it to automatic
                var isBurst = FiringModes.Burst.Equals(FiringMode);
                var isLargeCal = value.Equals(BallisticTypes.ElectroEnergy_LargeCaliber);
                if (isBurst && !isLargeCal)
                {
                    _firingMode = FiringModes.Automatic;
                }
                
                _ballisticType = value switch
                {
                    BallisticTypes.ElectroEnergy_SmallCaliber => BallisticTypes.ElectroEnergy_SmallCaliber,
                    BallisticTypes.ElectroEnergy_MediumCaliber => BallisticTypes.ElectroEnergy_MediumCaliber,
                    BallisticTypes.ElectroEnergy_LargeCaliber => BallisticTypes.ElectroEnergy_LargeCaliber,
                    _ => BallisticTypes.ElectroEnergy_SmallCaliber
                };
            }
        }

        public override float DischargeRate
        {
            get
            {
                // a function of firing mode and ballistic type
                var rate = DEFAULT_DELAY;
                switch (FiringMode)
                {
                    case FiringModes.SemiAutomatic:
                        switch (BallisticType)
                        {
                            case BallisticTypes.ElectroEnergy_SmallCaliber:
                            case BallisticTypes.ElectroEnergy_MediumCaliber:
                            case BallisticTypes.ElectroEnergy_LargeCaliber:
                                rate = SEMI_MEDIUM_DELAY;
                                break;
                            default:
                                rate = BLAST_DELAY;
                                break;
                        }
                        break;
                    case FiringModes.Automatic:
                        rate = BallisticType switch
                        {
                            BallisticTypes.ElectroEnergy_SmallCaliber => AUTO_SMALL_DELAY,
                            BallisticTypes.ElectroEnergy_MediumCaliber => AUTO_MEDIUM_DELAY,
                            BallisticTypes.ElectroEnergy_LargeCaliber => AUTO_LARGE_DELAY,
                            _ => AUTO_MEDIUM_DELAY
                        };
                        break;
                    case FiringModes.Burst:
                        rate = BLAST_DELAY;
                        break;
                    default:
                        rate = DEFAULT_DELAY;
                        break;
                }

                return rate;
            }
        }
        
        public override float MaximumViewAngleY => (float) Math.PI * 1.43f;
        
        public float Energy
        {
            get => _energy;
            protected set
            {
                if (value < 0)
                {
                    _energy = 0;
                    return;
                }
                if (value > 100)
                {
                    _energy = 100;
                    return;
                }
                _energy = value;
            }
        }
        
        public override uint Fire()
        {
            var fired = 0u;
            if (Energy < 1) return fired;

            // calculate how much energy to deplete based on ballistic type and firing mode
            Energy -= ShotUtility.GetShotEnergy(this);

            fired = 1;

            return fired;
        }

        private void Update()
        {
            // energy weapons have an electric/radioactive battery/magazine so they are always in a state of recharging
            if (Energy > 99.99) return;
            if (Time.time < _nextRecharge) return;
            _nextRecharge = Time.time + RECHARGE_RATE;
            Energy += RECHARGE_AMOUNT;
        }
    }
}