using UnityEngine;
using TMPro;

public class Teste : MonoBehaviour
{

    public TMP_Dropdown myDropdown; // Assign your TMP_Dropdown in the Inspector

    public void GetSelectedIndex()
    {
        int selectedIndex = myDropdown.value;
        Debug.Log("Selected index: " + selectedIndex);
        
        string selectedText = myDropdown.options[myDropdown.value].text;
        Debug.Log("Selected text: " + selectedText);
    }
    
    void Start()
    {
        
    }

   
    void Update()
    {
        GetSelectedIndex();
    }
}
