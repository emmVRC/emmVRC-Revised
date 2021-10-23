using UnityEngine;

namespace emmVRC.Objects
{
    // This class is neccessary to control the hash code of unity objects in hashsets
    public class UnityObjectWrapper<T> where T : Object
    {
        public readonly T value;

        public UnityObjectWrapper(T value) => this.value = value;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is T objAsT)
                return objAsT.GetInstanceID() == GetHashCode();
            else if (obj is UnityObjectWrapper<T> objAsWrapper)
                return objAsWrapper.GetHashCode() == GetHashCode();
            else
                return false;
        }

        public override int GetHashCode() => value.GetInstanceID();

        public static implicit operator UnityObjectWrapper<T>(T value) => new UnityObjectWrapper<T>(value);
        public static implicit operator T(UnityObjectWrapper<T> value) => value.value;
    }
}