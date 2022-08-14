using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tableStudent_prefab : MonoBehaviour
{
    public Student_Class estudianteActual = new Student_Class();
    public bool somethingWrong = false;
    [Header("UI")]
    public TextMeshProUGUI id_UI;
    public TextMeshProUGUI firstName_UI;
    public TextMeshProUGUI lastName_UI;
    public TextMeshProUGUI age_UI;
    public TextMeshProUGUI email_UI;
    public TextMeshProUGUI finalNote_UI;
    public Toggle approvedCheckbox;

    private Color32 correctColor = new Color32(255, 255, 255, 255);
    private Color32 wrongColor = new Color32(240, 132, 132, 255);
    
    private void Update() 
    {
        CheckIfWrong();
    }

    public void UpdateStudentInfo(Student_Class newStudentInfo)
    {
        estudianteActual = new Student_Class(newStudentInfo);
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        id_UI.text = estudianteActual.id.ToString();
        firstName_UI.text = estudianteActual.nombre;
        lastName_UI.text = estudianteActual.apellido;
        age_UI.text = estudianteActual.edad.ToString();
        email_UI.text = estudianteActual.correo;
        finalNote_UI.text = estudianteActual.nota.ToString();
    }

    public bool CheckFinalNote()
    {
        if(approvedCheckbox.isOn)
        {
            if(estudianteActual.nota >= 3.0f)
                return true;
            else
                return false;
        }
        else
        {
            if(estudianteActual.nota < 3.0f)
                return true;
            else
                return false;
        }
    }

    private void CheckIfWrong()
    {
        //check if someone of the fields are wrong
        if(estudianteActual.nota < 0.0f || estudianteActual.nota > 5.0f)
        {
            somethingWrong = true;
            finalNote_UI.GetComponentInParent<Image>().color = wrongColor;
        }
        else
        {
            somethingWrong = false;
            finalNote_UI.GetComponentInParent<Image>().color = correctColor;
        }
    }
}
