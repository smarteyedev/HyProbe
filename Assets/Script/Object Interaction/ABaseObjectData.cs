using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR
{
    [System.Serializable]
    public abstract class ABaseObjectData
    {
        public string textName;
        public string textMessage;
        public string textDescription;
    }


    [System.Serializable]
    public class ObjectCharacteristicData : ABaseObjectData
    {
        public NPCStatus npcStatus;
    }
}

public enum StatusType
{
    
}


