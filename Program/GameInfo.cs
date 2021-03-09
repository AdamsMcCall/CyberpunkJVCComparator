using System;
using System.Collections.Generic;
using System.Text;

namespace Program
{
    public class GameInfo
    {
        public string Name { get; set; }
        public string Grade { get; set; }
        public string Link { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as GameInfo;
            if (other == null)
                return false;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(GameInfo other)
        {
            if (Name == other.Name)
                return true;
            else
                return false;
        }
    }
}
