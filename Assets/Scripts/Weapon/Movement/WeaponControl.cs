using System;
using System.Collections;
using Scene.Environment;
using UnityEngine;
using UnityEngine.Profiling;

namespace Weapon.Movement
{
/**
 * WeaponControl handles the movement and rotation control of weapon objects based on mouse and keyboard input.
 * It's also the control class for everything related to firing from the ProjectileWeapon instance class.
 */
    public class WeaponControl : MonoBehaviour
    {
        private float _tempDist = 50f;
        private LineRenderer laserLine;
        private GameObject _firePoint;
        private uint _maxCameraFocalY = 16;
        private float _nextFire;
        private bool _triggerPressed;
        private bool _triggerReleased;
        private WaitForSeconds shotDuration;
        private ParticleSystem muzzleFlash;

        // weapon information properties
        public ProjectileWeapon Weapon { get; private set; }
        public float ProjectileDistance { get; set; }
        public uint ShotsFired { get; private set; }
        public bool IsEnergyWeapon { get; private set; }
        public float AmmoRemaining { get; private set; }
        
        // movement/rotation properties
        public bool MovementEnabled { get; set; }
        public float MoveSpeed { get; set; }
        public float RotateSpeed { get; set; }
        public float Rotation { get; set; }
        public float MoveX { get; set; }
        public float MoveY { get; set; }

        // camera/view properties
        public float CameraFocalAngleY { get; set; }
        public Camera FpsCam { get; set; }
        public GameObject FocalPoint { get; private set; }
        public GameObject FirePoint { get; private set; }
        public Vector3 FocalPointLocalPosition
        {
            get => FocalPoint.transform.localPosition;
            private set => FocalPoint.transform.localPosition = value;
        }
        public bool FocalViewAngleInRange { get; private set; }

        private void Start()
        {
            MoveSpeed = 10f;
            RotateSpeed = 500f;
            Weapon = GetComponent<ProjectileWeapon>();
            Weapon.Reload();
            IsEnergyWeapon = Weapon.BallisticType.Equals(BallisticTypes.Bullet) ? false : true;
            AmmoRemaining = GetRemainingAmmo();
            
            _triggerReleased = true;
            _firePoint = GameObject.Find("firePoint");
            FirePoint = _firePoint;
            FocalPoint = GameObject.Find("focalPoint");
            laserLine = GetComponentInChildren<LineRenderer>();
            muzzleFlash = GameObject.Find("muzzleFlash").GetComponent<ParticleSystem>();

            ShotsFired = 0;
            shotDuration = ShotUtility.CalculateShotWait(FirePoint.transform.position);
        }

        private void Update()
        {
            //Profiler.BeginSample("Weapon Control Update");
            AmmoRemaining = GetRemainingAmmo();
            
            //
            if (!IsEnergyWeapon && Input.GetKeyUp(KeyCode.R))
            {
                Weapon.Reload();
                return;
            }
            
            if (MovementEnabled)
            {
                UpdateTransform();
                UpdateFocalPointPosition();
            }
            
            _triggerPressed = Input.GetButton("Fire1");
            // i don't know why this works, but it does
            if (!_triggerPressed && !_triggerReleased)
            {
                _triggerReleased = true;
            }

            if (Time.time < _nextFire)
            {
                // TODO check weapon ballistic type (energy or bullet) and recharge
                return;
            }
            // enough time has passed so the weapon can fire again

            if (!Input.GetButton("Fire1")) return;
            // fire button pressed

            if (FiringModes.SemiAutomatic.Equals(Weapon.FiringMode) && !_triggerReleased) return;

            if (!CanFire()) return;

            _triggerReleased = false;
            _nextFire = Time.time + Weapon.DischargeRate;
            ShotsFired += Weapon.Fire();
            AmmoRemaining = GetRemainingAmmo();
            
            ProjectileDistance = ShotUtility.CalculateShotDistance(FirePoint.transform.position, Weapon);
            StartCoroutine(FireCoroutine());

            //Vector3 rayOrigin = FpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            Vector3 barrellOrigin = _firePoint.transform.TransformPoint(new Vector3(0, 0, 0));
            RaycastHit hit;
            DrawDebugRay(barrellOrigin);
            laserLine.SetPosition(0, _firePoint.transform.position);

            if (Physics.Raycast(barrellOrigin, -_firePoint.transform.right, out hit, _tempDist))
            {
                laserLine.SetPosition(1, hit.point);
                BallisticTarget target = hit.collider.GetComponent<BallisticTarget>();
                if (target != null)
                {
                    // calculate based on weapon power
                    //target.Damage(ShotUtility.CalculateDamage(Weapon));
                    target.Damage(0.25f);
                    
                    // ****************
                    // TODO having issues with impact particle system decal alignment, will need to fix
                    // ****************
                    // add particle prefab at hit location
                    Vector3 iV = FocalPoint.transform.position - hit.point;
                    Vector3 rV = Vector3.Reflect(iV, hit.normal);
                    //print($"hit point: {hit.point}, muzzle: {FocalPoint.transform.position}");
                    target.Hit(hit, iV);
                    Debug.DrawRay(hit.point, hit.normal, Color.red, 5);
                    Debug.DrawRay(hit.point, iV, Color.green, 5);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * ShotUtility.GetPower(Weapon), ForceMode.Impulse);
                }
            }
            else
            {
                laserLine.SetPosition(1, barrellOrigin + (-_firePoint.transform.right * ProjectileDistance));
            }
            //Profiler.EndSample();
        }

        private void DrawDebugRay(Vector3 originPoint)
        {
            Debug.DrawRay(originPoint, -_firePoint.transform.right * _tempDist, Color.black, Weapon.DischargeRate);
        }

        private void UpdateTransform()
        {
            var hInput = Input.GetAxis("Horizontal");
            var vInput = Input.GetAxis("Vertical");
            var mouseInputX = Input.GetAxis("Mouse X");
            
            // yaw (ψ) -- roll (Ф) disabled, TODO pitch (θ)
            Rotation = RotateSpeed * Time.deltaTime * mouseInputX;
            MoveX = MoveSpeed * hInput * Time.deltaTime;
            MoveY = MoveSpeed * vInput * Time.deltaTime;
            transform.Rotate(Vector3.up, Rotation, Space.World);
            transform.Translate(MoveX * Vector3.forward);
            transform.Translate(MoveY * Vector3.left);
        }

        private void UpdateFocalPointPosition()
        {
            var mouseInputY = Input.GetAxis("Mouse Y");
            var focalViewDeltaY = MoveSpeed * mouseInputY * Time.deltaTime;
            var nextFocalViewY = FocalPoint.transform.localPosition.y + focalViewDeltaY;
            var cameraAngleYInRange = CameraFocalAngleY < _maxCameraFocalY;
            FocalViewAngleInRange = nextFocalViewY > -Weapon.MaximumViewAngleY && nextFocalViewY < Weapon.MaximumViewAngleY;

            if (cameraAngleYInRange)
            {
                if (FocalViewAngleInRange)
                {
                    FocalPoint.transform.Translate(focalViewDeltaY * Vector3.up);
                }
                else
                {
                    TranslateFocalPoint(nextFocalViewY < 0 ? -Weapon.MaximumViewAngleY : Weapon.MaximumViewAngleY);
                }
            }
            else
            {
                TranslateFocalPoint(nextFocalViewY < 0 ? -Weapon.MaximumViewAngleY : Weapon.MaximumViewAngleY);
            }
        }

        private IEnumerator FireCoroutine()
        {
            laserLine.enabled = true;
            // TODO play muzzle flash particle system
            muzzleFlash.Play();
            yield return shotDuration;
            muzzleFlash.Stop();
            laserLine.enabled = false;
        }

        private void TranslateFocalPoint(float newY)
        {
            FocalPointLocalPosition = new Vector3(FocalPointLocalPosition.x, newY, FocalPointLocalPosition.z);
        }

        private float GetRemainingAmmo()
        {
            float ammo = 0;
            if (IsEnergyWeapon)
            {
                // TODO this seems weird. shouldn't Weapon.Energy be good enough?
                if (Weapon.GetType().Equals(typeof(RailGun)))
                {
                    ammo = ((RailGun)Weapon).Energy;
                }

                if (Weapon.GetType().Equals(typeof(GatlingGun)))
                {
                    ammo = ((GatlingGun)Weapon).Energy;
                }
            }
            else
            {
                ammo = Weapon.LoadedAmmunition;
            }
            return ammo;
        }
        
        // is there enough ammunition, either bullets or energy
        private bool CanFire()
        {
            if (IsEnergyWeapon)
            {
                // if weapon energy < shot energy
                return ShotUtility.GetShotEnergy(Weapon) < GetRemainingAmmo();
            }

            return Weapon.LoadedAmmunition > 0;
        }
    }
}