using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : PlayerAnimationController
{
    // Start is called before the first frame update
    void Start()
    {
        base.Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        base.UpdateRotation();
    }
}
