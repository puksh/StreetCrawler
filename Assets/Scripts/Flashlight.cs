using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light spotlight;
    private bool isOn = false;
    public int toolIndex = 0;
    void Start()
    {
        spotlight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (GetComponentInParent<Movement>().currentToolIndex == toolIndex)
            {
                isOn = !isOn;
                spotlight.enabled = isOn;
            }
        }
    }
}
