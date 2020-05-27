using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace emmVRC.Objects
{
    public class Notification
    {
        public string Message;
        public string Button1Text;
        public string Button2Text;
        public System.Action Button1Action;
        public System.Action Button2Action;
        public Sprite Icon;
        public int Timeout = -1;
        public List<object> PersistentObjects = new List<object>();
    }
}
