using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequirementUI : MonoBehaviour
{
    [SerializeField] Image Icon;
    [SerializeField] Image Color;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    
    //public void Setup(RequiredDataCard datacard)
    //{
    //    Icon.sprite = sprites[(int)datacard.type];
    //    Color.color = datacard.color;
    //}
}
