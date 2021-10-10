using UnityEngine;

namespace emmVRC.Objects
{
    public class Waypoint
    {
        public string Name = "";
        public float x = 0;
        public float y = 0;
        public float z = 0;
        public float rx = 0;
        public float ry = 0;
        public float rz = 0;
        public float rw = 0;

        public void Goto()
        {
            if (VRCPlayer.field_Internal_Static_VRCPlayer_0 == null)
                return;

            VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = new Vector3(x, y, z);
            VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation = new Quaternion(rx, ry, rz, rw);
        }

        public bool IsEmpty() => string.IsNullOrEmpty(Name) && x == 0 && y == 0 && z == 0 && rx == 0 && ry == 0 && rz == 0 && rw == 0;
    }
}