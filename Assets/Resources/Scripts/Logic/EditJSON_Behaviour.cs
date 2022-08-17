using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class EditJSON_Behaviour : MonoBehaviour
{
    public static EditJSON_Behaviour instance;
    public string jsonPath;
    public string jsonFullname;
    private string firstBackup;
    private string backupContent;
    [TextArea] public string jsonContent;
    public bool notSavedChanges = false;
    private Color32 normalColor = new Color32(255, 255, 255, 255);
    private Color32 wrongColor = new Color32(255, 200, 200, 255);
    [Header("UI")]
    public GameObject JSONMenu_obj;
    public GameObject notSavedChangesMenu_obj;
    public GameObject restoreJSON_obj;
    public TextMeshProUGUI nameLabel;
    public TMP_InputField contentJSON_inputField;
    public Button openJSONedit_btn;
    public Button saveJSONedit_btn;
    public CanvasGroup savedDataLabel;

    private void Awake() 
    {
        instance = this;
        jsonContent = GetTextFromPath();
        firstBackup = jsonContent;
        backupContent = jsonContent;
    }

    public void ModifySavedChanges(bool value)
    {
        notSavedChanges = value;
    }

    public void OpenJSONMenu()
    {
        notSavedChanges = false;
        JSONMenu_obj.SetActive(true);
        jsonContent = GetTextFromPath();
        contentJSON_inputField.text = jsonContent;
        backupContent = contentJSON_inputField.text;
        ModifySavedChanges(false);
    }

    public void CloseJSONMenu()
    {
        if(notSavedChanges)
        {
            notSavedChangesMenu_obj.SetActive(true);
        }
        else
        {
            JSONMenu_obj.SetActive(false);
        }
    }

    public void RestoreContent()
    {
        try
        {
            JsonUtility.FromJson<StudentList>(backupContent);
            jsonContent = backupContent;
            File.WriteAllText(GetFullPath(), jsonContent);
        }
        catch (System.Exception)
        {
            jsonContent = firstBackup;
            File.WriteAllText(GetFullPath(), jsonContent);
        }
    }

    public string GetTextFromPath()
    {
        return File.ReadAllText(GetFullPath());
    }

    private string GetFullPath()
    {
        jsonPath = Application.streamingAssetsPath+'/';
        DirectoryInfo info = new DirectoryInfo(jsonPath);
        FileInfo[] fileInfo = info.GetFiles();
        jsonFullname = fileInfo[0].Name;
        nameLabel.text = "Nombre del JSON: "+jsonFullname;
        return fileInfo[0].ToString();
    }

    public void SaveNewContent()
    {
        LeanTween.alphaCanvas(savedDataLabel, 1f, 0.2f).setOnComplete(() => {
            LeanTween.alphaCanvas(savedDataLabel, 0f, 0.2f).setDelay(0.6f);
        });
        File.WriteAllText(GetFullPath(), contentJSON_inputField.text);
        jsonContent = contentJSON_inputField.text;

        notSavedChanges = false;
    }
}
