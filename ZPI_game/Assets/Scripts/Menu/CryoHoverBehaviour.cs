using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Cryo.Script;

namespace Assets.Scripts.Menu
{
    class CryoHoverBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IMenuActive
    {
        [SerializeField] private CryoUI cryo;
        [SerializeField] private EyeType eyesNormalState;
        [SerializeField] private EyeType eyesTriggeredState;
        [SerializeField] private MouthType mouthNormalState;
        [SerializeField] private MouthType mouthTriggeredState;

        public bool IsEnabled { get; set; } = true;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsEnabled) return;
            cryo.SetBothEyesTypes(eyesTriggeredState);
            cryo.SetMouthType(mouthTriggeredState);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsEnabled) return;
            cryo.SetBothEyesTypes(eyesNormalState);
            cryo.SetMouthType(mouthNormalState);
        }

        public void SetEnabled(bool isActive)
        {
            IsEnabled = isActive;
        }
    }
}
