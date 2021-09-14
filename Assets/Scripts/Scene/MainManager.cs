using System;
using Scene.Environment;
using UI;
using UnityEngine;
using UnityEngine.Profiling;
using Weapon;
using Weapon.Movement;

namespace Scene
{
    public class MainManager : MonoBehaviour
    {
        private readonly Vector3 _weaponPosition = new Vector3(0f, 1.3f, 0f);
        private readonly Quaternion _weaponRotation = Quaternion.AngleAxis(0, Vector3.up);
        private readonly Vector3 _weaponScale = new Vector3(0.25f, 0.25f, 0.25f);

        private Canvas _uiCanvas;
        public UIMainManager _uiManager;

        private string SelectedWeapon { get; set; }

        private GameObject FocalPoint { get; set; }

        private WeaponControl Weapon { get; set; }
        
        private BallisticTarget Target { get; set; }

        private void Start()
        {
            _uiCanvas = GameObject.FindObjectOfType<Canvas>();
            _uiManager = _uiCanvas.GetComponentInChildren<UIMainManager>();

            FocalPoint = GameObject.Find("Focal Point");
            //SelectedWeapon = "3D/Prefabs/Gun";
            //SelectedWeapon = "3D/Prefabs/Rifle";
            SelectedWeapon = "3D/Prefabs/RailGun";
            //SelectedWeapon = "3D/Prefabs/GatlingGun";
            Target = GameObject.FindWithTag("Targets").GetComponent<BallisticTarget>();
            Target.Health = 100;
            if (TitleMainManager.Instance != null)
            {
                SelectedWeapon = TitleMainManager.Instance.Selected;
            }

            // load a prefab weapon based on SelectedWeapon value
            Weapon = Instantiate(Resources.Load<WeaponControl>(SelectedWeapon), _weaponPosition, _weaponRotation);
            Weapon.transform.localScale = _weaponScale;
            Weapon.FpsCam = FocalPoint.GetComponentInChildren<Camera>();
            Weapon.MovementEnabled = true;
        }

        private void Update()
        {
            //Profiler.BeginSample("Main Manager Update");
            if (Weapon == null) return;

            FocalPoint.transform.position = Weapon.transform.position;
            
            // TODO need the signed version of this angle
            Weapon.CameraFocalAngleY = Mathf.Round(Vector3.Angle(FocalPoint.transform.position - Weapon.FocalPoint.transform.position, Weapon.transform.right));

            if (Weapon.FocalPoint != null)
            {
                FocalPoint.transform.LookAt(Weapon.FocalPoint.transform, Vector3.up);
            }

            var ammo = new LogItem(Weapon.IsEnergyWeapon ? "Energy Remaining" : "Shots Remaining", $"{Weapon.AmmoRemaining}");
            var logOutput = _uiManager.FormatLogString(
                new LogItem("View Angle [θ]", $"{Weapon.CameraFocalAngleY}"),
                new LogItem("Shot Count", $"{Weapon.ShotsFired}"),
                ammo,
                new LogItem("Target Damage", $"{Math.Round(Target.Health, 2)}"),
                new LogItem("Weapon Power (Vx)", $"{ShotUtility.GetPower(Weapon.Weapon)}"),
                new LogItem("Shot Duration (Time)", $"{ShotUtility.CalculateShotDuration(Weapon.FirePoint.transform.position.y)}"),
                new LogItem("Shot Distance", $"{ShotUtility.CalculateShotDistance(Weapon.FirePoint.transform.position, Weapon.Weapon)}")
            );
            _uiManager.Log(logOutput);

            if (Weapon.transform.position.y < 0)
            {
                ResetWeapon();
            }
            //Profiler.EndSample();
        }

        // if the weapon falls through the ground, reset it's position/rotation
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
