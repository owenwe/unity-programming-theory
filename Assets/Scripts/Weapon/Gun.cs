using System;

namespace Weapon
{
    /**
 * Gun class is the base-level class from the ProjectileWeapon abstract class
 */
    public class Gun : ProjectileWeapon
    {
        private uint _gunLoadedAmmunition;

        protected override uint LoadedAmmunition
        {
            get => _gunLoadedAmmunition;
            set { }
        }

        public override string PrefabPath => "3D/Prefabs/Gun";

        // TODO add power set accessor when other BallisticTypes can be used for this class
        public override float Power => _power;

        public override Enum FiringMode => FiringModes.SemiAutomatic;

        public override Enum BallisticType => BallisticTypes.Bullet;

        public override void Reload()
        {
            if (LoadedAmmunition == 0)
            {
                _gunLoadedAmmunition = Capacity * 1;
            }
        }
    }
}