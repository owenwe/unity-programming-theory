using UnityEngine;
using Weapon;

namespace UI
{
    public class Selection : MonoBehaviour
    {
        private ProjectileWeapon _weapon;
        private float _alpha;
        private float _alphaGoal;
        private float alphaChangeStep = 0.01f;
        private bool _isOpaque;

        private Material _opaqueMaterial;
        private Material _transparentMaterial;

        private int ChangeDirection { get; set; }
        public bool IsMoving { get; set; }

        public ProjectileWeapon ProjectileWeapon
        {
            get => _weapon;
            set
            {
                _weapon = value;
                WeaponMeshRenderer = _weapon.GetComponentInChildren<MeshRenderer>();
            }
        }

        public MeshRenderer WeaponMeshRenderer { get; private set; }
        public string WeaponPrefab { get; private set; }

        private void Awake()
        {
            IsMoving = false;
            ChangeDirection = -1;
            _opaqueMaterial = Instantiate(Resources.Load<Material>("Materials/Opaque"));
            _transparentMaterial = Instantiate(Resources.Load<Material>("Materials/Transparent"));
            ProjectileWeapon = gameObject.GetComponentInChildren<ProjectileWeapon>();
            WeaponMeshRenderer.material = _transparentMaterial;
            WeaponPrefab = ProjectileWeapon.PrefabPath;
        }

        private void Update()
        {
            if (!IsMoving) return;

            var step = alphaChangeStep * ChangeDirection;
            var aDiff = _alphaGoal - _alpha;

            if (ChangeDirection * aDiff < alphaChangeStep)
            {
                if (_isOpaque)
                {
                    WeaponMeshRenderer.material = _opaqueMaterial;
                }
                else
                {
                    var c = WeaponMeshRenderer.material.color;
                    c.a = _alphaGoal;
                    WeaponMeshRenderer.material.SetColor("_Color", c);
                }

                _alpha = _alphaGoal;
                IsMoving = false;
            }
            else
            {
                var c = WeaponMeshRenderer.material.color;
                _alpha += step;
                c.a = _alpha;
                WeaponMeshRenderer.material.SetColor("_Color", c);
            }
        }

        public void UpdateTransparency(float newAlpha, bool newOpaque, int newDirection)
        {
            if (WeaponMeshRenderer != null)
            {
                WeaponMeshRenderer.material = _transparentMaterial;
            }

            //print($"updating transparency for {gameObject.name}: opaque={newOpaque}, alpha goal={newAlpha}");
            IsMoving = true;
            ChangeDirection = newDirection;
            _alphaGoal = newAlpha;
            _isOpaque = newOpaque;
        }
    }
}
