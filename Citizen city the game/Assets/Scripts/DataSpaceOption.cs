using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataSpaceOption : MonoBehaviour
{ 
    [Header("Elements")]
    [SerializeField] TextMeshPro companyName_ui;
    [SerializeField] TextMeshPro dataType_ui;
    [SerializeField] Image relations_ui;
    [SerializeField] TextMeshPro cost_ui;
    [SerializeField] TextMeshPro dataSetSize_ui;

    [Header("Options")]
    [SerializeField] public string companyName;
    [SerializeField] public float relationPercentage = 0.5f;
    [SerializeField] public int cost;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

}
