using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Sequence : MonoBehaviour
{
    [Header ("Нужно выбрать менеджер управляющий этим состоянием")]
    [SerializeField] public SequenceManager manager;

    [Header ("Управляет выбранным состоянием")]
    [SerializeField] public SequenceManager.State State;

    [Header ("Кривая управления партикл системами")]

    [SerializeField] public AnimationCurve ParticlesProcess;
    [SerializeField] public List<ParticleSystem> PS;


    private void Update()
    {
        if (!TestWorking()) return;

        ProcessParticles();
    }

    void ProcessParticles()
    {
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
     bool TestWorking()
    {
        if (manager == null) return false;
        if (manager.CurrentState != State) return false;
        if (PS == null) return false;
        if (ParticlesProcess == null) return false;
        return true;
    }
}
