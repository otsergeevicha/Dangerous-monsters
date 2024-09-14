using System;

namespace Infrastructure.GroupData
{
    [Serializable]
    public class DataStateLevel
    {
        public bool OpenLeftSection;
        public bool OpenRightSection;

        public bool HaveLeftTurret;
        public bool HaveCenterTurret;
        public bool HaveRightTurret;
        
        public bool OpenOneTransition;
        public bool OpenTwoTransition;
    }
}