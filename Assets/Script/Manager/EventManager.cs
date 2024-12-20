using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SR
{
    public static class EventManager
    {
        [Header("NPC Event")]
        public static UnityAction<ObjectCharacteristicData> onSetCharacteristicData = delegate { };

        [Header("UI Componenent")]

        //Use On Player Information Controller
        public static UnityAction<NPCStatus> onUpdateNPCStatusUI = delegate { };


        [Header("Object Interaction")]
        //Using on PlayerInformationController and ABaseObjectInteraction
        public static UnityAction OnUpdatePlayerInformation = delegate { };
    }
}
