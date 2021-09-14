using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public static class ShotUtility
    {
        public const float G = 9.80665f;

        public const float UNKNOWN_POWER = 250f;//3.3
        public const float BULLET_SEMI_AUTOMATIC_POWER = 950f;//12.6
        public const float BULLET_BURST_POWER = 1400f;//18
        public const float BULLET_AUTOMATIC_POWER = 850f;//11.3
        public const float ELECTRO_ENERGY_SMALL_CALIBER_SEMI_AUTOMATIC_POWER = 1400f;//18.6
        public const float ELECTRO_ENERGY_MEDIUM_CALIBER_SEMI_AUTOMATIC_POWER = 1250f;//16.6
        public const float ELECTRO_ENERGY_LARGE_CALIBER_SEMI_AUTOMATIC_POWER = 1100f;//14.6
        public const float ELECTRO_ENERGY_SMALL_CALIBER_AUTOMATIC_POWER = 1000f;//13.3
        public const float ELECTRO_ENERGY_MEDIUM_CALIBER_AUTOMATIC_POWER = 1850f;//24.6
        public const float ELECTRO_ENERGY_LARGE_CALIBER_AUTOMATIC_POWER = 2475f;//33
        // TODO Electro Energy: sm-burst, med-burst, lrg-burst
        public const float ELECTRO_ENERGY_BURST_POWER = 10250f;//136.6
        public const float ENERGY_AUTOMATIC_POWER = 3275f;//43.6
        
        public const float SMALL_SHOT_ENERGY = 0.75f;
        public const float MEDIUM_SHOT_ENERGY = 2.5f;
        public const float LARGE_SHOT_ENERGY = 9.75f;
        public const float BURST_SHOT_ENERGY = 21.69f;

        // a function of V (power:p) and time (t) 
        public static float CalculateShotDistance(Vector3 position, ProjectileWeapon weapon)
        {
            var t = CalculateShotDuration(position.y);
            var p = GetPower(weapon);
            return p * t;
        }
        
        // Returns a WaitForSeconds using the passed position vector to calculate time
        public static WaitForSeconds CalculateShotWait(Vector3 position)
        {
            var duration = CalculateShotDuration(0, position.y);
            return new WaitForSeconds(duration);
        }
        
        // Calculates the time a projectile will traveling before hitting the ground ( y == 0 )
        // TODO factor in θ
        public static float CalculateShotDuration(float heightY, float angleY = 0.0f, float velocityY = 0.0f)
        {
            // standard rate of fall via Earth's gravity until we add in angle and Y velocity
            return Mathf.Sqrt(2 * G * heightY) / G;
        }

        public static float GetPower(ProjectileWeapon weapon)
        {
            return GetPower(weapon.BallisticType, weapon.FiringMode);
        }
        
        /**
         * takes the given ballistic type and firing mode and returns a shot duration value
         */
        public static float GetPower(Enum ballisticType, Enum firingMode)
        {
            var power = UNKNOWN_POWER;
            switch (firingMode)
            {
                case FiringModes.SemiAutomatic:
                    switch (ballisticType)
                    {
                        case BallisticTypes.Bullet:
                            power = BULLET_SEMI_AUTOMATIC_POWER;
                            break;
                        case BallisticTypes.ElectroEnergy_SmallCaliber:
                            power = ELECTRO_ENERGY_SMALL_CALIBER_SEMI_AUTOMATIC_POWER;
                            break;
                        case BallisticTypes.ElectroEnergy_MediumCaliber:
                            power = ELECTRO_ENERGY_MEDIUM_CALIBER_SEMI_AUTOMATIC_POWER;
                            break;
                        case BallisticTypes.ElectroEnergy_LargeCaliber:
                            power = ELECTRO_ENERGY_LARGE_CALIBER_SEMI_AUTOMATIC_POWER;
                            break;
                        default:
                            power = UNKNOWN_POWER;
                            break;
                    }
                    break;
                case FiringModes.Automatic:
                    switch (ballisticType)
                    {
                        case BallisticTypes.Bullet:
                            power = BULLET_AUTOMATIC_POWER;
                            break;
                        case BallisticTypes.ElectroEnergy_SmallCaliber:
                            power = ELECTRO_ENERGY_SMALL_CALIBER_AUTOMATIC_POWER;
                            break;
                        case BallisticTypes.ElectroEnergy_MediumCaliber:
                            power = ELECTRO_ENERGY_MEDIUM_CALIBER_AUTOMATIC_POWER;
                            break;
                        case BallisticTypes.ElectroEnergy_LargeCaliber:
                            power = ELECTRO_ENERGY_LARGE_CALIBER_AUTOMATIC_POWER;
                            break;
                        case BallisticTypes.Energy:
                            power = ENERGY_AUTOMATIC_POWER;
                            break;
                        default:
                            power = UNKNOWN_POWER;
                            break;
                    }
                    break;
                case FiringModes.Burst:
                    switch (ballisticType)
                    {
                        case BallisticTypes.Bullet:
                            power = BULLET_BURST_POWER;
                            break;
                        case BallisticTypes.ElectroEnergy_SmallCaliber:
                        case BallisticTypes.ElectroEnergy_MediumCaliber:
                        case BallisticTypes.ElectroEnergy_LargeCaliber:
                            power = ELECTRO_ENERGY_BURST_POWER;
                            break;
                        default:
                            power = UNKNOWN_POWER;
                            break;
                    }
                    break;
                default:
                    power = UNKNOWN_POWER;
                    break;
            }
            
            return power;
        }

        public static float GetShotEnergy(ProjectileWeapon weapon)
        {
            return GetShotEnergy(weapon.BallisticType, weapon.FiringMode);
        }

        private static float GetShotEnergy(Enum type, Enum mode)
        {
            if (type.Equals(BallisticTypes.Bullet)) return 0f;
            var energy = 0f;
            switch (mode)
            {
                case FiringModes.SemiAutomatic:
                case FiringModes.Automatic:
                    energy = type switch
                    {
                        BallisticTypes.ElectroEnergy_SmallCaliber => SMALL_SHOT_ENERGY,
                        BallisticTypes.ElectroEnergy_MediumCaliber => MEDIUM_SHOT_ENERGY,
                        BallisticTypes.ElectroEnergy_LargeCaliber => LARGE_SHOT_ENERGY,
                        _ => SMALL_SHOT_ENERGY
                    };
                    break;
                case FiringModes.Burst:
                    energy = BURST_SHOT_ENERGY;
                    break;
            }

            return energy;
        }

        public static float CalculateDamage(ProjectileWeapon weapon)
        {
            return CalculateDamage(weapon.BallisticType, weapon.FiringMode);
        }

        private static float CalculateDamage(Enum type, Enum mode)
        {
            var power = GetPower(type, mode);
            return power / 75;
        }
    }
}