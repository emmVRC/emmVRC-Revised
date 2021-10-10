using System;

namespace emmVRC.Objects.ModuleBases
{
    public class PriorityAttribute : Attribute
    {
        // Lower is sooner
        public readonly int priority = 0;

        public PriorityAttribute(int priority)
        {
            this.priority = priority;
        }
    }
}