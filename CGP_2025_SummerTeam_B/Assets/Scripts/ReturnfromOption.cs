using UnityEngine;

public class ReturnfromOption : MonoBehaviour
{
    [SerializeField] private GameObject OptionPanel;
     
    public void onclick()
    {
        OptionPanel.SetActive(false);
        
    }
}
