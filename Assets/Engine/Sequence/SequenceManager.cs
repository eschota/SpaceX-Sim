using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class SequenceManager : MonoBehaviour
{
    public enum State { None=0, Idle1=1, Idle2=2, Action1=3, Action2=4, Cancel1=5, Cancel2=6}
    [Header ("Текущее состояние")]
    [SerializeField] public State CurrentState;
    [Header ("Конкретное время состояния")]

    [Range (0f,1f)]
    [SerializeField] public float Timer;
    [Header ("Длительность каждого состояния в секундах")]

    [SerializeField] public List<float> StateTimer = new List<float> { 1, 1, 1, 1, 1, 1, 1 };
    //[Header("Порядок выполнения состояния")]

    //[SerializeField] List<State> Order;




    private void Awake()
    {
        
    }
    private void Update()
    {
        if(Application.isPlaying)
        Timer += Time.deltaTime/StateTimer[(int)CurrentState];
        if (Timer > 1)
        {
            Timer = 0;
            CurrentState++;
            if ((int)CurrentState == 7) CurrentState = State.Idle1;
        }
    }
}
