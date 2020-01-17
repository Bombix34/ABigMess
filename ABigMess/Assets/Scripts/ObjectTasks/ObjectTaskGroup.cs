using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "BIGMESS/Object tasks group")]
[Serializable]
public class ObjectTaskGroup : ScriptableObject
{
    public List<ObjectTask> objectTasks;
    public ObjectTaskGroup nextTasks;
}
