using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "BIGMESS/Tasks logos")]
class TaskIcons : ScriptableObject
{
    [SerializeField]
    Sprite emptyIcon;

    [SerializeField]
    private List<ObjectTypeIcon> objectTypeIcons;

    [SerializeField]
    private List<EventTypeIcon> eventTypeIcons;

    [SerializeField]
    private List<ZoneAreaTypeIcon> zoneAreaTypeIcons;

    public List<Sprite> GetIcons(ObjectTask currentTask)
    {
        List<Sprite> icons = new List<Sprite>();

        ObjectTypeIcon objectTypeIcon = objectTypeIcons.Find(s => s.objectType.Equals(currentTask.objectTypeConcerned));
        if (objectTypeIcon != null) {
            icons.Add(objectTypeIcon.spriteObjectType);
        }
        else
        {
            icons.Add(emptyIcon);
        }

        EventTypeIcon eventTypeIcon = eventTypeIcons.Find(s => s.eventType.Equals(currentTask.eventType));
        if (eventTypeIcon != null)
        {
            icons.Add(eventTypeIcon.spriteEventType);
        }
        else
        {
            icons.Add(emptyIcon);
        }

        ZoneAreaTypeIcon destinationTypeIcon = zoneAreaTypeIcons.Find(s => s.zoneAreaType.Equals(currentTask.destinationForBring));
        if (destinationTypeIcon != null)
        {
            icons.Add(destinationTypeIcon.spriteZoneAreaType);
        }
        else
        {
            icons.Add(emptyIcon);
        }

        return icons;
    }

}

[Serializable]
public class ObjectTypeIcon
{
    public ObjectSettings.ObjectType objectType;
    public Sprite spriteObjectType;
}

[Serializable]
public class EventTypeIcon
{
    public ObjectTask.EventKeyWord eventType;
    public Sprite spriteEventType;
}

[Serializable]
public class ZoneAreaTypeIcon
{
    public ObjectZoneArea.ZoneAreaType zoneAreaType;
    public Sprite spriteZoneAreaType;
}
