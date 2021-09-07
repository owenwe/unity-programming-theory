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
        private readonly Enum _ballisticType = BallisticTypes.Energy;

        public override string PrefabPath => "3D/Prefabs/GatlingGun";

        public override Enum BallisticType
        {
            get => _ballisticType;
        }
    }
}