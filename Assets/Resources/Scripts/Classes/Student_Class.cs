[System.Serializable]
public class Student_Class
{
    public int id = 0;
    public string nombre = "";
    public string apellido = "";
    public int edad = -1;
    public string correo = "noname@any.com";
    public float nota = 0.0f;

    public Student_Class(){}

    public Student_Class(Student_Class newInfo)
    {
        this.id = newInfo.id;
        this.nombre = newInfo.nombre;
        this.apellido = newInfo.apellido;
        this.edad = newInfo.edad;
        this.correo = newInfo.correo;
        this.nota = newInfo.nota;
    }
}

[System.Serializable]
public class StudentList
{
    public Student_Class[] datos;
}
