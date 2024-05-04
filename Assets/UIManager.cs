using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;

    // Reference to the camera controller
    public cameraController cameraController;

    private void Start()
    {
        // Ensure that all panels are initially hidden
        foreach (var panel in panels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    public void TogglePanel(int index)
    {
        if (index >= 0 && index < panels.Length)
        {
            var panel = panels[index];
            if (panel != null)
            {
                panel.SetActive(!panel.activeSelf);

                // Set interacting with UI based on panel active state
                cameraController.SetInteractingWithUI(panel.activeSelf);
            }
        }
        else
        {
            Debug.LogError("Invalid index for panels array.");
        }
    }
}
