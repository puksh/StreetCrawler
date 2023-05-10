using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light spotlight;
    private bool isOn = false;
    public int toolIndex = 1;
    void Start()
    {
        spotlight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && GetComponentInParent<ToolSwitcher>().currentToolIndex == toolIndex)
        {
                isOn = !isOn;
                spotlight.enabled = isOn;

        }
        else if(GetComponentInParent<ToolSwitcher>().currentToolIndex != toolIndex)
        {
            isOn = !isOn;
            spotlight.enabled = false;

        }
    }
}
