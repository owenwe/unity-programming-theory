using UnityEngine;

namespace Weapon.Movement
{
    /**
 * handles motion and rotation control of weapon objects in the Main scene
 */
    public class WeaponControl : MonoBehaviour
    {
        private float _maxFocalY = 5.3f;

        public bool MovementEnabled { get; set; }
        public float MoveSpeed { get; set; }
        public float RotateSpeed { get; set; }
        public float Rotation { get; set; }
        public float MoveX { get; set; }
        public float MoveY { get; set; }

        public GameObject FocalPoint { get; set; }

        public Vector3 FocalPointLocalPosition
        {
            get => FocalPoint.transform.localPosition;
            private set { FocalPoint.transform.localPosition = value; }
        }

        public bool FocalViewAngleInRange { get; private set; }

        private void Start()
        {
            MoveSpeed = 10f;
            RotateSpeed = 50f;
            FocalPoint = GameObject.Find("focalPoint");
        }

        private void Update()
        {
            if (MovementEnabled)
            {
                UpdateTransform();
                UpdateFocalPointPosition();
            }
        }

        private void UpdateTransform()
        {
            var hInput = Input.GetAxis("Horizontal");
            var vInput = Input.GetAxis("Vertical");
            var mouseInputX = Input.GetAxis("Mouse X");
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
            FocalViewAngleInRange = nextFocalViewY > -_maxFocalY && nextFocalViewY < _maxFocalY;

            if (FocalViewAngleInRange)
            {
                FocalPoint.transform.Translate(focalViewDeltaY * Vector3.up);
            }
            else
            {
                FocalPointLocalPosition = new Vector3(
                    FocalPointLocalPosition.x,
                    nextFocalViewY < 0 ? -_maxFocalY : _maxFocalY,
                    FocalPointLocalPosition.z);
            }
        }
    }
}