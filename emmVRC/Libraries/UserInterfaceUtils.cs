using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Libraries
{
    public class UserInterfaceUtils
    {
        // Internal cache of the User Interface transform
        private static Transform UserInterfaceReference;

        // Fetch the User Interface transform
        public static Transform GetUserInterface()
        {
            if (UserInterfaceReference == null)
            {
                /*foreach(GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    emmVRCLoader.Logger.Log(obj.name);
                }
                UserInterfaceReference = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().First<GameObject>(i => i.name == "UserInterface").transform; */
                UserInterfaceReference = GameObject.Find("UserInterface").transform;
                
            }
            return UserInterfaceReference;
        }
    }
}
