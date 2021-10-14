namespace emmVRC.Objects
{
    public class Alarm
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Alarm objAsAlarm)
                return objAsAlarm.Id == Id;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string Name = "New Alarm";
        public bool IsEnabled = false;
        public long Time = 0;
        public bool Repeats = false;
        public bool IsSystemTime = true;
        public float Volume = 0.5f;
        public readonly int Id = 0;

        // For json
        public Alarm() { }

        public Alarm(int id)
        {
            Id = id;
        }
    }
}