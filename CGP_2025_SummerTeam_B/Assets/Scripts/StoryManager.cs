using System.ComponentModel;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private StoryData[] storyDatas;
    [SerializeField] private Image background;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private TextMeshProUGUI characterName;

    public int storyIndex { get; private set; }
    public int textIndex { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    
    
}
