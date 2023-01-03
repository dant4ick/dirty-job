using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEffect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animationName;
    private static int muzzleFlashAnimation;

    private void Start()
    {
        muzzleFlashAnimation = Animator.StringToHash(animationName);

        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        animator.Play(muzzleFlashAnimation);

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }

}
