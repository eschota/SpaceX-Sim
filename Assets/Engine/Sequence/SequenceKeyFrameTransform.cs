using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]

public class SequenceKeyFrameTransform : Sequence
{   
    [SerializeField] AnimationCurve X;
    [SerializeField] AnimationCurve Y;
    [SerializeField] AnimationCurve Z;
    [SerializeField] List<int> KeyFrame;
    [SerializeField] List<Vector3> Positions;

  
    public int id { get => Mathf.RoundToInt(manager.Timer * 100); }
    public void addKey()
    {
         
        if (!KeyFrame.Exists(X=>X==id )) 
        {
            newKey();
            Debug.Log("new Key");
        }
        else
           // if (Vector3.Distance(Positions[(int)manager.Timer * 100], transform.position) > 0.3f)
            {
            int ide = KeyFrame.FindIndex(X=>X==id);
            KeyFrame.RemoveAt(ide);
            
            Positions.RemoveAt(ide);
            newKey();
                Debug.Log("Key Owerwritten");
            }
    }
    private void Update()
    {
        if (Application.isPlaying) transform.position = new Vector3(X.Evaluate(manager.Timer), Y.Evaluate(manager.Timer), Z.Evaluate(manager.Timer));
    }
    public void newKey()
    {
        X.AddKey(new Keyframe(manager.Timer, transform.position.x));
        Y.AddKey(new Keyframe(manager.Timer, transform.position.y));
        Z.AddKey(new Keyframe(manager.Timer, transform.position.z));
        KeyFrame.Add(Mathf.RoundToInt( manager.Timer *100));
        Positions.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z));
    }
    public void ClearAll()
    {
        KeyFrame.Clear();
        KeyFrame = new List<int>();
        Positions.Clear();
        Positions = new List<Vector3>();
        X = new AnimationCurve();
        Y = new AnimationCurve();
        Z = new AnimationCurve();
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        for (int i = 1; i < 14; i++)
        {
            Gizmos.DrawSphere(new Vector3(X.Evaluate(1f / i), Y.Evaluate(1f / i), Z.Evaluate(1f / i)),.15f);
        }
    }
}

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(SequenceKeyFrameTransform))]
    class Keys : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Add Key"))
            {

                Selection.activeGameObject.GetComponent<SequenceKeyFrameTransform>().addKey();
            }
       
            if (GUILayout.Button("Clear All"))
            {

                Selection.activeGameObject.GetComponent<SequenceKeyFrameTransform>().ClearAll();
            }

           
        }
    }
#endif
