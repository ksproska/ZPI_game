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
    class CryoHoverBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CryoUI cryo;
        [SerializeField] private EyeType eyesNormalState;
        [SerializeField] private EyeType eyesTriggeredState;
        [SerializeField] private MouthType mouthNormalState;
        [SerializeField] private MouthType mouthTriggeredState;
        public void OnPointerEnter(PointerEventData eventData)
        {
            cryo.SetBothEyesTypes(eyesTriggeredState);
            cryo.SetMouthType(mouthTriggeredState);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            cryo.SetBothEyesTypes(eyesNormalState);
            cryo.SetMouthType(mouthNormalState);
        }
    }
}
