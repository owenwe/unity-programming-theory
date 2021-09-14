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
 *   - Discharge Rate: the amount of time (in seconds) between projectiles being fired
 *   - Firing Mode: the method projectiles are fired, i.e. one at a time, continuously, etc.
 */
    public abstract class ProjectileWeapon : MonoBehaviour
    {
        private uint _capacity = 9;

        public virtual uint Capacity
        {
            get => _capacity;
            set { }
        }

        public virtual uint LoadedAmmunition { get; protected set; }

        public virtual float DischargeRate
        {
            get => 0.5f;
            set { }
        }

        public abstract Enum BallisticType { get; set; }

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

        public virtual string PrefabPath => "3D/Prefabs/Gun";
        
        public virtual float MaximumViewAngleY { get; }

        public abstract void Reload();

        // should return the number of projectiles fired
        public virtual uint Fire()
        {
            var fired = 0u;
            if (LoadedAmmunition < 1) return fired;

            LoadedAmmunition--;
            fired = 1;

            return fired;
        }
    }
}