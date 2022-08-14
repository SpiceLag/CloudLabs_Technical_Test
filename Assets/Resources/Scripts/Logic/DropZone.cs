using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public int zoneIndex = 0; 

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.tag.Equals("draggable") && this.tag.Equals("droppableZone"))
        {
            //print("Dropped "+eventData.pointerDrag.name);
            eventData.pointerDrag.GetComponent<draggableStudent_prefab>().actualZone = zoneIndex;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<RectTransform>().SetParent(this.transform);
        }
    }
}
