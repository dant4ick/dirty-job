using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    [SerializeField] private Vector2 soundDistance;
    [SerializeField] private GameObject particlesOnBreak;

    public void Break()
    {
        AlarmManager.AlarmEnemiesByQuietSound(transform, soundDistance);
        Instantiate(particlesOnBreak, transform.position, transform.rotation);
        Destroy(gameObject);    
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(transform.position, soundDistance);
    //}
}
