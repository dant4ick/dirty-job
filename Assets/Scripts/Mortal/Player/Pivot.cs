using System;
using UnityEngine;

namespace Player
{
    public class Pivot : MonoBehaviour
    {
        public void ChangeRotation(Vector3 target)
        {
            Vector3 difference = target - transform.position;
            difference.Normalize();

            float rotationX = transform.parent.localScale.x;

            float rotationZ = Mathf.Atan2(difference.y * rotationX, difference.x * rotationX) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        }
    }
}
