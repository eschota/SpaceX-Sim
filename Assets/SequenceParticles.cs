using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceParticles : Sequence
{
    [Header("Кривая управления партикл системами")]
    [SerializeField] public AnimationCurve ParticlesProcess;
    [SerializeField] public List<ParticleSystem> PS;



    public override void Update()
    {
        base.Update();
        ProcessParticles();
    }
void ProcessParticles()
{
        if (PS == null) return ;
        if (ParticlesProcess == null) return;

        foreach (var item in PS)
    {
        if (ParticlesProcess.Evaluate(manager.Timer) > 0f)
        {
            if (!item.isPlaying) item.Play();
            Debug.Log("Play");
        }
        else
        {
            if (item.isPlaying) item.Stop();
        }
    }
}
 
}
