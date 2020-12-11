using UnityEngine;

public class UnitSun : Unit
{
    [SerializeField] private float minAngle = 26.3f;
    [SerializeField] private float maxAngle = -26.3f;

    public override void Update()
    {
        base.Update();

        var days = (float) (TimeManager.Days + (TimeManager.Months - 1) * 30f);
        var angle = 0f;

        if (days >= 0 && days < 182)
            angle = Mathf.LerpAngle(maxAngle, minAngle, days / 182f);
        else if (days >= 182)
            angle = Mathf.LerpAngle(minAngle, maxAngle, (days - 182f) / 182f);

        transform.eulerAngles = new Vector3(angle, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}