using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveContractUI : MonoBehaviour
{
    [SerializeField] TMP_Text ConsultantName;
    [SerializeField] Image ConsultantIcon;
    [SerializeField] TMP_Text Duration;
    [SerializeField] GameObject specializationContainer;
    [SerializeField] GameObject specializtionIconPrefab;

    public void Initialize(HiredConsultant consultant)
    {
        foreach (DataCardType card in consultant.Specializations)
        {
            GameObject instance = Instantiate(specializtionIconPrefab);
            instance.GetComponent<Image>().sprite = card.dataIcon;
            instance.transform.SetParent(specializationContainer.transform, false);
        }

        ConsultantName.text = consultant.Name;
        ConsultantIcon.sprite = consultant.Icon;
        Duration.text = consultant.turnsLeft.ToString();
    }
}
