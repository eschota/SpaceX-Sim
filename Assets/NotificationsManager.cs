using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationsManager : MonoBehaviour
{
    [SerializeField] Notification notification;
    public static NotificationsManager instance;
    void Start()
    {
        instance = this;
    }
    public void AddNotification(Unit unit)
    {
        if(TimeManager.Days>1)
        Instantiate(notification, transform).NotificationIni(unit);
    }
}
