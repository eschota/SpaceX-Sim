using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceTransform : Sequence
{
    [SerializeField] Vector3 StartPosition;

    [SerializeField] public AnimationCurve PosX = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0, 0));
    [SerializeField] public AnimationCurve PosY = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0, 0));
    [SerializeField] public AnimationCurve PosZ = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0, 0));
    [SerializeField] public AnimationCurve RotationX = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0, 0));
    [SerializeField] public AnimationCurve RotationY = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0, 0));
    [SerializeField] public AnimationCurve RotationZ = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0, 0));
    [SerializeField] Transform[] Objects;


    private void Reset()
    {
        StartPosition = transform.position;
        manager = FindObjectOfType<SequenceManager>();
        
    }
    public override void Update()
    {
        base.Update();
        Processing();
    }
    void Processing()
    {
        if (Objects == null) return;
        

        foreach (var item in Objects)
        {

            item.transform.localRotation = Quaternion.Euler(360*RotationX.Evaluate(manager.Timer), 360 * RotationY.Evaluate(manager.Timer), 360 * RotationZ.Evaluate(manager.Timer));
            item.transform.position = StartPosition + new Vector3(PosX.Evaluate(manager.Timer), PosY.Evaluate(manager.Timer), PosZ.Evaluate(manager.Timer));
        }
    }

}
