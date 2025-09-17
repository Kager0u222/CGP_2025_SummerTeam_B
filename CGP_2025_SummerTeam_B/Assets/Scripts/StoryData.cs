using System.Collections.Generic;
using UnityEngine;

public class StoryData : MonoBehaviour
{
    public List<Story> stories = new List<Story>();

    [System.Serializable]
    public class Story
    {
        public Sprite Background;
        public Sprite CharacterImage;
        [TextArea]
        public string StoryText;
        public string CharacterName;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
