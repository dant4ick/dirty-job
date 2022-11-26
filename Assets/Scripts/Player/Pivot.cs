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

            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

            if (rotationZ < -90 || rotationZ > 90)
            {
                if (transform.parent.transform.eulerAngles.y == 0)
                {
                    //transform.parent.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    //transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
                }
                else if (transform.parent.transform.eulerAngles.y == 180)
                {
                    //transform.parent.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    //transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
                }
            }
        }

        
    }
}
