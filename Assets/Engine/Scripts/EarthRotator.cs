using UnityEngine;

public class EarthRotator : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Vector3 Axis = Vector3.back;

    private void Update()
    {
        if (GameManager.CurrentState == GameManager.State.MenuStartGame)
            transform.Rotate(Axis, Time.deltaTime * speed);

        if (GameManager.CurrentState == GameManager.State.Play)
        {
            var hours = TimeManager.Hours;
            var angle = Mathf.Lerp(360, 0f, hours / 24f);

            transform.rotation = Quaternion.Euler(-90f, 180f, angle);
        }
    }
}