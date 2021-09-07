using System;

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
        private float _energy;
        private float _rechargeRate;
        private readonly Enum _ballisticType = BallisticTypes.ElectroEnergy;

        public override string PrefabPath => "3D/Prefabs/RailGun";

        public override Enum BallisticType
        {
            get => _ballisticType;
        }

        public override void Fire()
        {

        }
    }
}