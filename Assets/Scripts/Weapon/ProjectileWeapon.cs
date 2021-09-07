using System;
using System.Collections;
using UnityEngine;

namespace Weapon
{
    /**
 * ProjectileWeapon class is the abstract class for ballistic-firing objects
 *   - Capacity: how many units of ballistic projectiles can be loaded
 *   - Loaded Ammunition: how many units of ballistic projectiles currently loaded
 *   - Ballistic Type: the type of projectile that will be fired
 *   - Discharge Rate: the amount of time (in milliseconds) between projectiles being fired
 *   - Power: how much force (energy impacted onto target) a ballistic projectile applies to a target
 *   - Firing Mode: the method projectiles are fired, i.e. one at a time, continuously, etc.
 */
    public abstract class ProjectileWeapon : MonoBehaviour
    {
        private uint _capacity = 9;
        private uint _loadedAmmunition;
        private readonly uint _dischargeRate = 750;
        protected float _power = 1200f;
        private bool _isDischarging;

        public virtual uint Capacity
        {
            get => _capacity;
            set { }
        }

        protected virtual uint LoadedAmmunition
        {
            get => _loadedAmmunition;
            set { }
        }

        public virtual uint DischargeRate
        {
            get => _dischargeRate;
            set { }
        }

        public virtual float Power
        {
            get => _power;
            set { }
        }

        public abstract Enum BallisticType { get; }

        public virtual Enum FiringMode
        {
            get => FiringModes.SemiAutomatic;
            set
            {
                if (Enum.IsDefined(typeof(FiringModes), value))
                {
                    Debug.LogWarning($"The Firing Mode given {value} is not a known firing mode");
                }
            }
        }

        public virtual string PrefabPath { get; set; }

        public abstract void Reload();

        public virtual void Fire()
        {
            if (LoadedAmmunition > 0 && !_isDischarging)
            {
                _loadedAmmunition--;
                _isDischarging = true;
                StartCoroutine(FireCoroutine());
            }
        }

        private IEnumerator FireCoroutine()
        {
            yield return new WaitForSeconds((float)_dischargeRate / 1000);
            _isDischarging = false;
        }
    }
}