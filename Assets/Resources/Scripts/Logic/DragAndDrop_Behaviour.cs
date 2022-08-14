using UnityEngine;
using TMPro;

public class DragAndDrop_Behaviour : MonoBehaviour
{
    public static DragAndDrop_Behaviour instance;
    public StudentList studentArray = new StudentList();
    public GameObject draggableStudent_prefabObj;
    public GameObject neutralZone_content;
    [Header("Error menu UI")]
    public GameObject errorMenu_obj;
    public TextMeshProUGUI errorMenu_desc;
    [Header("Restart menu UI")]
    public GameObject restartMenu_obj;

    private void Awake() 
    {
        instance = this;    
    }

    public void CreateDraggableStudents(StudentList tableStudentList)
    {
        studentArray = tableStudentList;

        for (int i = 0; i < studentArray.datos.Length; i++)
        {
            GameObject auxPref = Instantiate(draggableStudent_prefabObj, neutralZone_content.transform);
            draggableStudent_prefab script = auxPref.GetComponent<draggableStudent_prefab>();
            script.UpdateStudentInfo(studentArray.datos[i]);
        }
    }

    public void VerifyIfCorrectZones()
    {
        draggableStudent_prefab[] allDraggableStudents = FindObjectsOfType<draggableStudent_prefab>();

        //check if there are still students on the neutral zone.
        for (int i = 0; i < allDraggableStudents.Length; i++)
        {
            if(allDraggableStudents[i].actualZone == 0)
            {
                OpenErrorMenu(1);
                return;
            }
        }

        //check if the students are on the correct zones
        for (int i = 0; i < allDraggableStudents.Length; i++)
        {
            if(!allDraggableStudents[i].CheckFinalNote())
            {
                OpenErrorMenu(2);
                return;
            }
        }

        //if those are okay, we finished.
        restartMenu_obj.SetActive(true);
    }

    private void OpenErrorMenu(int caseID)
    {
        errorMenu_obj.SetActive(true);

        switch (caseID)
        {
            //if something is wrong with the range of the grades
            case 1:
            {
                errorMenu_desc.text = "Todavía hay estudiantes en la zona neutral. \n Por favor mover a las zonas de Aprob@do/Reprob@do.";
                break;
            }
            //if something is wrong with the checkbox
            case 2:
            {
                errorMenu_desc.text = "Existe uno o más estudiantes en las zonas incorrectas. \n Puedes revisar sus notas previas pulsando el botón con flecha a la derecha de la pantalla ";
                break;
            }
        }
    }

    public void CloseErrorMenu()
    {
        errorMenu_obj.SetActive(false);
    }

    public void RestartApplication()
    {
        draggableStudent_prefab[] allDraggableStudents = FindObjectsOfType<draggableStudent_prefab>();
        for (int i = 0; i < allDraggableStudents.Length; i++)
        {
            Destroy(allDraggableStudents[i].gameObject);
        }
        
        GoBack();
        Table_Behaviour.instance.ResetTable();
    }

    public void GoBack()
    {
        restartMenu_obj.SetActive(false);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }
}
