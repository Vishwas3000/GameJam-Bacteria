using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHealAnimation : MonoBehaviour
{
    public static TriggerHealAnimation instance;
    private Animator animator;
    [SerializeField] private float animationTimeSpan;

    void Start()
    {
        instance = this;
        animator = GetComponent<Animator>();

    }

   public IEnumerator StartAnimationOnce()
    {
        float t = Time.time + animationTimeSpan;
        Debug.Log(Time.time);
        
        while(t>Time.time)
        {
            if(animator.enabled == false)
                animator.enabled = true;

            yield return null;
        }
        if(t<=Time.time)
        {
            animator.enabled = false;
        }

    }
}
