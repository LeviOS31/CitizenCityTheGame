using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ConsultantManagerUI : MonoBehaviour
{
    readonly ConsultantManager consultantManager = new ConsultantManager();
    [SerializeField] List<GameObject> consultantSlots = new List<GameObject>();

    private void Start()
    {
        ConsultantOption[] consultantOptions = Resources.LoadAll<ConsultantOption>("ScriptableObjects/Consultants");
        Consultant[] consultants = new Consultant[consultantOptions.Length];

        for (int i = 0; i < consultantOptions.Length; i++) 
        {
            ConsultantOption option = consultantOptions[i];
            
            Consultant consultant = 
                new Consultant(
                    option.Name,
                    option.Icon,
                    option.Specializations,
                    option.BasePrice,
                    option.PricePerData,
                    option.Duration
                    );

            consultants[i] = consultant;
        }

        consultantManager.SetConsultantOptions(consultants);
        CreateConsultants();
    }

    public void CreateConsultants()
    {
        consultantSlots[0].GetComponent<ConsultantUI>().Initialize(consultantManager.consultantOptions[0]);
    }

    private void ClearConsultants()
    {

    }
}
