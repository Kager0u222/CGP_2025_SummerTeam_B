using UnityEngine;
using UnityEngine.UIElements;

public class GotoOption : MonoBehaviour
{
    [SerializeField] private GameObject OptionPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void onclick()
    {
        OptionPanel.SetActive(true);
        
    }
}
