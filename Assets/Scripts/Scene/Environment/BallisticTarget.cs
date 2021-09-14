using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scene.Environment
{
    public class BallisticTarget : MonoBehaviour
    {
        public float Health { get; set; }

        public void Damage(float amount)
        {
            Health = Mathf.Max(Health - amount, 0);
            if (Health == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
