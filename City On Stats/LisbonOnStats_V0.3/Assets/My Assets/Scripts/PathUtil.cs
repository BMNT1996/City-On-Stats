using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtil : MonoBehaviour
{
    static string path;

    public void setPath(string newPath)
    {
        path = newPath;
    }

    public string getPath()
    {
        return path;
    }
}
