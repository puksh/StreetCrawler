using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSwitcher : MonoBehaviour
{
    public Transform tool1;
    public Transform tool2;
    public int currentToolIndex = 0;

    private void Start()
    {
        // Disable the second tool
        tool2.gameObject.SetActive(false);
    }

    public void SwitchTool()
    {
        // Disable the current tool's gameObject
        if (currentToolIndex == 0)
        {
            tool1.gameObject.SetActive(false);
            currentToolIndex = -1;
        }
        else
        {
            tool2.gameObject.SetActive(false);
            currentToolIndex = 0;
        }

        // Enable the new tool's gameObject
        if (currentToolIndex == 0)
        {
            tool1.gameObject.SetActive(true);
        }
        else
        {
            tool2.gameObject.SetActive(true);
        }
    }
}
