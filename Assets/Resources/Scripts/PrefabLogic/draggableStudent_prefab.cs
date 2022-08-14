using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class draggableStudent_prefab : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Student_Class estudianteActual = new Student_Class();
    public int actualZone = 0;
    public Transform prevParent;
    public TextMeshProUGUI nameUI;

    private RectTransform objTransform;
    private CanvasGroup canvasGroup;

    private void Awake() 
    {
        objTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        prevParent = objTransform.parent.transform;
    }

    public void UpdateStudentInfo(Student_Class newInformation)
    {
        estudianteActual = new Student_Class(newInformation);
        nameUI.text = estudianteActual.nombre + " " +estudianteActual.apellido;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        objTransform.SetParent(FindObjectOfType<Canvas>().transform);

        objTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        objTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if(objTransform.parent.GetComponent<Canvas>())
        {
            //print("Resetting position");
            ResetPositionAndParent();
        }
    }

    public void ResetPositionAndParent()
    {
        objTransform.SetParent(prevParent);
        objTransform.anchoredPosition = prevParent.GetComponent<RectTransform>().anchoredPosition;
        actualZone = 0;
    }

    public bool CheckFinalNote()
    {
        if(actualZone == 1) //is on approval
        {
            if(estudianteActual.nota >= 3.0f)
                return true;
            else
                return false;
        }
        else //is on not approval/neutral
        {
            if(estudianteActual.nota < 3.0f)
                return true;
            else
                return false;
        }
    }
}
