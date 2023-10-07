using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class WindowsList : MonoBehaviour
{
    public List<UIWidget> UIWindows = new List<UIWidget>()
    {
        new UIWidget()
    };
}

public class WindowData<T>
{
    private string _path;
    public string Path=> _path;
    public T Window;

    public WindowData(string path)
    {
        _path = path;
    }
}

public class UIWidget: MonoBehaviour
{
    private void Update()
    {
        if(Input.GetButtonUp("Cancel"))
        {
            gameObject.SetActive(false);
        }
    }
}
