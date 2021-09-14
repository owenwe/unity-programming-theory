using System;

namespace Weapon
{
    /**
 * Gun class is the base-level class from the ProjectileWeapon abstract class
 */
    public class Gun : ProjectileWeapon
    {
        public override uint LoadedAmmunition { get; protected set; }

        public override string PrefabPath => "3D/Prefabs/Gun";

        public override Enum FiringMode => FiringModes.SemiAutomatic;

        public override Enum BallisticType
        {
            get => BallisticTypes.Bullet;
            set { }
        }

        public override float MaximumViewAngleY => (float) Math.PI / 12;

        public override void Reload()
        {
            LoadedAmmunition = Capacity * 1;
        }
    }
}