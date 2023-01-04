using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    [SerializeField] private Vector2 soundDistance;
    [SerializeField] private GameObject particlesOnBreak;

    [SerializeField] private AudioClip clip;

    public void Break()
    {
        SoundManager.PlayEnvironmentSound(clip);

        AlarmManager.AlarmEnemiesByQuietSound(transform, soundDistance);
        Instantiate(particlesOnBreak, transform.position, transform.rotation);
        Destroy(gameObject);    
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(transform.position, soundDistance);
    //}
}
