using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Components
{
    public class emmVRCPanel : MonoBehaviour
    {
        public TextMesh activeUsers;
        public TextMesh totalUsers;
        public TextMesh globalChat;
        public bool updateRequested;
        public static bool classInjected = false;

        public emmVRCPanel(IntPtr ptr) : base(ptr) { }

        public void Start()
        {
            var mf = gameObject.AddComponent<MeshFilter>();

            var mesh = new Mesh();
            mf.mesh = mesh;

            var vertices = new Vector3[4]
            {
                Vector3.zero,
                new Vector3(1.65f, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(1.65f, 1, 0)
            };
            mesh.vertices = vertices;

            var tris = new int[6]
            {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
            };
            mesh.triangles = tris;

            var normals = new Vector3[4]
            {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
            };
            mesh.normals = normals;

            var uv = new Vector2[4]
            {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
            };
            mesh.uv = uv;
            MeshRenderer rend = gameObject.AddComponent<MeshRenderer>();
            rend.material = new Material(Shader.Find("Standard"));
            rend.material.SetTexture("_MainTex", Functions.Core.Resources.panelTexture);

            GameObject activeUsersWrapper = GameObject.Instantiate(new GameObject(), gameObject.transform);
            activeUsersWrapper.transform.localPosition = new Vector3(.325f, 0.65f, 0);
            activeUsersWrapper.transform.localScale = new Vector3(0.05f, 0.05f, 0.25f);
            activeUsers = activeUsersWrapper.AddComponent<TextMesh>();
            activeUsers.anchor = TextAnchor.UpperCenter;
            activeUsers.color = Color.black;
            GameObject totalUsersWrapper = GameObject.Instantiate(new GameObject(), gameObject.transform);
            totalUsersWrapper.transform.localPosition = new Vector3(1.3f, 0.65f, 0);
            totalUsersWrapper.transform.localScale = new Vector3(0.05f, 0.05f, 0.25f);
            totalUsers = totalUsersWrapper.AddComponent<TextMesh>();
            totalUsers.anchor = TextAnchor.UpperCenter;
            totalUsers.color = Color.black;
            GameObject globalChatWrapper = GameObject.Instantiate(new GameObject(), gameObject.transform);
            globalChatWrapper.transform.localPosition = new Vector3(0.0125f, 0.325f, 0);
            globalChatWrapper.transform.localScale = new Vector3(0.03f, 0.03f, 0.25f);
            globalChat = globalChatWrapper.AddComponent<TextMesh>();
            globalChat.color = Color.black;
        }
    }
}
