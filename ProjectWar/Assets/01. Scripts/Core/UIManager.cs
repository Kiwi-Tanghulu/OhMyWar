using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UIType
{
    Chase = 0,
    Fixed,
    Panel,
    PopUp,
    Full,
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    private Stack<PanelUI> panels = new Stack<PanelUI>();

    public bool ActiveUI => panels.Count > 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private Transform mainCanvas = null;
    public Transform MainCanvas
    {
        get
        {
            if (mainCanvas == null)
                mainCanvas = GameObject.Find("MainCanvas")?.transform;
            return mainCanvas;
        }
    }

    public void PopPanelUI()
    {
        panels.Peek().GetComponent<PanelUI>().Hide();
        panels.Pop();
    }
    public void PushPanelUI(PanelUI panel)
    {
        panels.Push(panel);
    }
    public void ClaerStack()
    {
        panels.Clear();
    }
    public void SetCursorActive(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Debug.Log("set cursor");
    }
    public void HandleEscape()
    {
        if (ActiveUI)
        {
            PopPanelUI();
        }
        else
        {
            
        }
    }
}
