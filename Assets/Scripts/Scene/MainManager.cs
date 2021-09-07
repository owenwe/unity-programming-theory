using UnityEngine;
using Weapon.Movement;

namespace Scene
{
    public class MainManager : MonoBehaviour
    {
        private readonly Vector3 _weaponPosition = new Vector3(0f, 1.3f, 0f);
        private readonly Quaternion _weaponRotation = Quaternion.AngleAxis(0, Vector3.up);
        private readonly Vector3 _weaponScale = new Vector3(0.25f, 0.25f, 0.25f);

        private string SelectedWeapon { get; set; }

        private GameObject FocalPoint { get; set; }

        private WeaponControl Weapon { get; set; }

        private void Start()
        {
            //_weaponPosition = new Vector3(0f, 1.3f, 0f);
            //_weaponRotation = Quaternion.AngleAxis(0, Vector3.up);
            //_weaponScale = new Vector3(0.25f, 0.25f, 0.25f);
            FocalPoint = GameObject.Find("Focal Point");

            //SelectedWeapon = "3D/Prefabs/Gun";
            //SelectedWeapon = "3D/Prefabs/Rifle";
            SelectedWeapon = "3D/Prefabs/RailGun";
            //SelectedWeapon = "3D/Prefabs/GatlingGun";
            if (TitleMainManager.Instance != null)
            {
                SelectedWeapon = TitleMainManager.Instance.Selected;
            }

            // load a prefab weapon based on SelectedWeapon value
            Weapon = Instantiate(Resources.Load<WeaponControl>(SelectedWeapon), _weaponPosition, _weaponRotation);
            Weapon.transform.localScale = _weaponScale;
            Weapon.MovementEnabled = true;
        }

        private void Update()
        {
            if (Weapon == null) return;

            FocalPoint.transform.position = Weapon.transform.position;

            // might be useful later for clamping the camera Y rotation
            //var maxViewAngleY = 16;
            //var viewAngleY = Mathf.Round(
            //    Vector3.Angle(FocalPoint.transform.position - WeaponFocalTransform.position, 
            //        Weapon.transform.right));
            //if (viewAngleY <= maxViewAngleY)
            
            if (Weapon.FocalPoint != null)
            {
                FocalPoint.transform.LookAt(Weapon.FocalPoint.transform, Vector3.up);
            }

            if (Weapon.transform.position.y < 0)
            {
                ResetWeapon();
            }
        }

        // TODO if the weapon falls through the ground, reset it's position/rotation
        private void ResetWeapon()
        {
            Weapon.MovementEnabled = false;

            var weaponTransform = Weapon.transform;
            weaponTransform.position = _weaponPosition;
            weaponTransform.rotation = _weaponRotation;

            Weapon.MovementEnabled = true;
        }
    }
}
