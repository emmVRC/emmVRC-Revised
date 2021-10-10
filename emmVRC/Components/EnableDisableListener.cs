using System;
using UnityEngine;
using UnhollowerBaseLib.Attributes;

#pragma warning disable IDE0051 // Remove unused private members

namespace emmVRC.Components
{
    public class EnableDisableListener : MonoBehaviour
    {
        public EnableDisableListener(IntPtr obj) : base(obj) { }

        [method: HideFromIl2Cpp]
        public event Action OnDisabled;
        [method: HideFromIl2Cpp]
        public event Action OnEnabled;

        private void OnDisable() => OnDisabled?.Invoke();
        private void OnEnable() => OnEnabled?.Invoke();
    }
}