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





    public virtual void Update()
    {
        if (!TestWorking()) return;
    }
    public bool TestWorking()
    {
        if (manager == null) return false;
        if (manager.CurrentState != State) return false;
        return true;
    }
 
}
