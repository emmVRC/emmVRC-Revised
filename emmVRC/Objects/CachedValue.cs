using System;

namespace emmVRC.Objects
{
    public struct CachedValue<T>
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is T)
                return obj.Equals(value);
            else if (obj is CachedValue<T> objAsCached)
                return objAsCached.value.Equals(value);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public T value;
        public DateTime cachedTime;
        public int maxAge;

        public CachedValue(T value, DateTime? cachedTime = null, int maxAge = 3600)
        {
            if (value == null)
                throw new ArgumentNullException("Value cannot be null");

            if (cachedTime == null)
                cachedTime = DateTime.Now;

            this.value = value;
            this.cachedTime = cachedTime.Value;
            this.maxAge = maxAge;
        }

        public bool Validate(DateTime? currentTime = null)
        {
            if (currentTime == null)
                currentTime = DateTime.Now;

            return (currentTime.Value - cachedTime).TotalSeconds < maxAge;
        }

        public static implicit operator T(CachedValue<T> value) => value.value;
        public static implicit operator CachedValue<T>(T value) => new CachedValue<T>(value);
    }
}