using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace Challenges
{
    public class StaticCity: City, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public new void OnPointerDown(PointerEventData eventData) {}

        public new void OnBeginDrag(PointerEventData eventData) {}

        public new void OnDrag(PointerEventData eventData){}

        public new void OnEndDrag(PointerEventData eventData){}
    }
}