using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Table_Behaviour : MonoBehaviour
{
    public static Table_Behaviour instance;
    public bool canUpdateJSON = true;
    public StudentList studentArray = new StudentList();
    public GameObject tableStudent_prefabObj;
    public GameObject tableStudent_content;
    public GameObject elements;
    [Header("UI")]
    public TextMeshProUGUI fileTitle;
    [Header("ErrorMenu_UI")]
    public GameObject errorMenu_obj;
    public TextMeshProUGUI errorMenu_desc;
    public Button verifyButton;
    public Button moveRightButton;
    public Button moveLeftButton;

    private void Awake() 
    {
        instance = this;
    }

    private void Update()
    {
        //check if the name of the array is "datos"
        if(canUpdateJSON)
        {
            if(EditJSON_Behaviour.instance.jsonContent.Contains("datos"))
            {
                //tries to load the JSON and catches exceptions. if not exception, continue to load normally
                try
                {
                    fileTitle.text = "Revisando archivo: "+EditJSON_Behaviour.instance.jsonFullname;
                    studentArray = JsonUtility.FromJson<StudentList>(EditJSON_Behaviour.instance.jsonContent);
                    CreateStudentsFromArray();
                    verifyButton.interactable = true;
                    EditJSON_Behaviour.instance.restoreJSON_obj.SetActive(false);
                }
                catch (System.Exception) //if the system catch a bad parse type, then un-load students. has the possibility to restore data
                {
                    fileTitle.text = "No se ha reconocido el archivo. \n Al JSON le falta una coma, comillas o algun simbolo.";
                    studentArray = new StudentList();
                    verifyButton.interactable = false;
                    DeleteAllContent();
                    EditJSON_Behaviour.instance.restoreJSON_obj.SetActive(true);
                }
            }
            //if not, not recognized file and un-load students. has the possibility to restore data
            else
            {
                fileTitle.text = "No se ha reconocido el archivo. \n Revisar que el nombre de objeto sea la palabra 'datos'.";
                studentArray = new StudentList();        
                verifyButton.interactable = false;
                DeleteAllContent();
                    EditJSON_Behaviour.instance.restoreJSON_obj.SetActive(true);
            }
        }
    }

    private void DeleteAllContent()
    {
        for (int i = 0; i < tableStudent_content.transform.childCount; i++)
        { 
            Destroy(tableStudent_content.transform.GetChild(i).gameObject);
        }
    } 

    public void CreateStudentsFromArray()
    {
        //updates the information constantly

        //if the content is zero by initial
        if(tableStudent_content.transform.childCount.Equals(0))
        {
            for (int i = 0; i < studentArray.datos.Length; i++)
            {
                GameObject auxPref = Instantiate(tableStudent_prefabObj, tableStudent_content.transform);
                tableStudent_prefab script = auxPref.GetComponent<tableStudent_prefab>();
                script.UpdateStudentInfo(studentArray.datos[i]);
            }
        }

        //if the length is not same between items and array
        else if(!studentArray.datos.Length.Equals(tableStudent_content.transform.childCount))
        {   
            //delete all of children
            DeleteAllContent();

            //instantiate them again
            for (int i = 0; i < studentArray.datos.Length; i++)
            {
                GameObject auxPref = Instantiate(tableStudent_prefabObj, tableStudent_content.transform);
                tableStudent_prefab script = auxPref.GetComponent<tableStudent_prefab>();
                script.UpdateStudentInfo(studentArray.datos[i]);
            }
        }

        //if the length is the same, update the items
        else
        {   
            for (int i = 0; i < tableStudent_content.transform.childCount; i++)
            {
                //first review if item already exists
                if(tableStudent_content.transform.GetChild(i).GetComponent<tableStudent_prefab>())
                {
                    tableStudent_prefab script = tableStudent_content.transform.GetChild(i).GetComponent<tableStudent_prefab>();
                    script.UpdateStudentInfo(studentArray.datos[i]);
                }
                //If item does not exists, instantiate
                else
                {
                    GameObject auxPref = Instantiate(tableStudent_prefabObj, tableStudent_content.transform);
                    tableStudent_prefab script = auxPref.GetComponent<tableStudent_prefab>();
                    script.UpdateStudentInfo(studentArray.datos[i]);
                }
            }
        }
    }

    public void VerifyButton()
    {
        canUpdateJSON = false;

        //check if the grades for the students are in range
        for (int i = 0; i < tableStudent_content.transform.childCount; i++)
        {
            if(tableStudent_content.transform.GetChild(i).GetComponent<tableStudent_prefab>().somethingWrong)
            {
                OpenErrorMenu(1, i);
                return;
            }
        }

        //check if the checkboxes are correct regarding the minimum grade for student
        for (int i = 0; i < tableStudent_content.transform.childCount; i++)
        {
            if(!tableStudent_content.transform.GetChild(i).GetComponent<tableStudent_prefab>().CheckFinalNote())
            {
                OpenErrorMenu(2, i);
                return;
            }
        }

        //if those are okay, we can proceed on drag and drop
        ChangeToDragAndDrop();
    }

    private void OpenErrorMenu(int caseID, int objectIndex)
    {
        errorMenu_obj.SetActive(true);
        EditJSON_Behaviour.instance.JSONMenu_obj.SetActive(false);

        switch (caseID)
        {
            //if something is wrong with the range of the grades
            case 1:
            {
                errorMenu_desc.text = "El rango de notas debe de ser entre 0.0 a 5.0. \n Revisar estudiante en posición de lista "+(objectIndex+1);
                break;
            }
            //if something is wrong with the checkbox
            case 2:
            {
                errorMenu_desc.text = "Existe una o más casillas marcadas de manera incorrecta. \n Revisar estudiante en posición de lista "+(objectIndex+1);
                break;
            }
        }
    }

    public void CloseErrorMenu()
    {
        errorMenu_obj.SetActive(false);
        canUpdateJSON = true;
    }

    private void ChangeToDragAndDrop()
    {
        canUpdateJSON = false;
        verifyButton.interactable = false;
        DragAndDrop_Behaviour.instance.CreateDraggableStudents(studentArray);

        for (int i = 0; i < tableStudent_content.transform.childCount; i++)
        {
            tableStudent_content.transform.GetChild(i).GetComponent<tableStudent_prefab>().approvedCheckbox.interactable = false;
        }

        LeanTween.moveLocalX(elements, 1930f, 0.4f).setEaseOutQuart().setOnComplete(() =>{
            EditJSON_Behaviour.instance.openJSONedit_btn.gameObject.SetActive(false);
            verifyButton.gameObject.SetActive(false);
            moveLeftButton.gameObject.SetActive(true);
            moveRightButton.gameObject.SetActive(true);
        });
    }

    public void MoveElements(int index)
    {
        moveLeftButton.interactable = false;
        moveRightButton.interactable = false;

        switch (index)
        {
            //move to left
            case 0:
            {
                LeanTween.moveLocalX(elements, 0f, 0.4f).setEaseOutQuart().setOnComplete(() => { 
                    moveLeftButton.interactable = true;
                    moveRightButton.interactable = true;});
                break;
            }
            //move to right
            case 1:
            {
                LeanTween.moveLocalX(elements, 1930f, 0.4f).setEaseOutQuart().setOnComplete(() => { 
                    moveLeftButton.interactable = true;
                    moveRightButton.interactable = true;});
                break;
            }
        }
    }

    public void ResetTable()
    {
        elements.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f,0f);
        EditJSON_Behaviour.instance.openJSONedit_btn.gameObject.SetActive(true);
        verifyButton.gameObject.SetActive(true);
        moveLeftButton.gameObject.SetActive(false);
        moveRightButton.gameObject.SetActive(false);
        
        EditJSON_Behaviour.instance.openJSONedit_btn.interactable = true;
        verifyButton.interactable = true;
        moveLeftButton.interactable = true;
        moveRightButton.interactable = true;

        DeleteAllContent();
        canUpdateJSON = true;
    }
}
