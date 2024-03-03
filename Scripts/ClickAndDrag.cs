using UnityEngine;
using UnityEngine.EventSystems;

namespace TMG
{
    public class ClickAndDrag : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        private Vector3 offset; // Change to Vector3

        public void OnPointerDown(PointerEventData eventData)
        {
            // Calculate the offset between the object's position and the mouse position
            offset = transform.position - (Vector3)eventData.position; // Convert eventData.position to Vector3
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Move the object to the mouse position plus the offset
            transform.position = (Vector3)eventData.position + offset; // Convert eventData.position to Vector3
        }
    }
}
