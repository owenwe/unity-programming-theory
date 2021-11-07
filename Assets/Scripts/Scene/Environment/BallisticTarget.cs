using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scene.Environment
{
    public class BallisticTarget : MonoBehaviour
    {
        public float Health { get; private set; }

        private void Start()
        {
            Health = 100;
        }

        public void Damage(float amount)
        {
            Health = Mathf.Max(Health - amount, 0);
            if (Health == 0)
            {
                gameObject.SetActive(false);
            }
        }

        public void Hit(RaycastHit point, Vector3 fromDirection)
        {
            ParticleSystem impactFx = Instantiate(
                Resources.Load<ParticleSystem>("3D/Prefabs/StoneImpactParticles"), 
                point.point,
                Quaternion.FromToRotation(Vector3.left, point.normal),
                point.transform);
            impactFx.gameObject.transform.LookAt(point.normal);
            impactFx.Play();
            Destroy(impactFx);
        }
    }
}
