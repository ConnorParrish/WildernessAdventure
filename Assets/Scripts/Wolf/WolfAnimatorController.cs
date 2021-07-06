using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimatorController : AnimationController
{
    private Animator animator;
    private WolfController wolfController;
    void Awake()
    {
        animator = GetComponent<Animator>();
        wolfController = GetComponent<WolfController>();

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Locomotion(ref animator);
    }
}
