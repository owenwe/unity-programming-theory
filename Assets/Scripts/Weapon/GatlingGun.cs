using System;

namespace Weapon
{
/**
 * GatlingGun class extends the RailGun class to make for specialized projectile firing
 *   - Rotation Rate: how fast the barrels rotate through a firing cycle
 *   - Barrels: the total number of barrels
 * TODO implement more functionalilty
 */
    public class GatlingGun : RailGun
    {
        private float _rotationRate;
        private uint _barrelCount;

        public override string PrefabPath => "3D/Prefabs/GatlingGun";

        public override Enum FiringMode => FiringModes.Automatic;
        
        public override Enum BallisticType => BallisticTypes.Energy;

        public override float DischargeRate => 0.03f;

        public override float MaximumViewAngleY => (float) Math.PI * 1.58f;

        public override uint Fire()
        {
            var fired = 0u;
            if (Energy < 1) return fired;
            
            Energy -= ShotUtility.GetShotEnergy(this);
            fired = 1;

            return fired;
        }
    }
}