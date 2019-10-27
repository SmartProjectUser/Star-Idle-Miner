using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

public class BaseMiningView : AbstractMiningView
{
    protected override void DoStart()
    {
        base.DoStart();
        StartMining();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
