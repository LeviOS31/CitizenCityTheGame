using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveContractUI : MonoBehaviour
{
    [SerializeField] TMP_Text ConsultantName;
    [SerializeField] Image ConsultantIcon;
    [SerializeField] TMP_Text Duration;

    public void Initialize(HiredConsultant consultant)
    {
        ConsultantName.text = consultant.Name;
        ConsultantIcon.sprite = consultant.Icon;
        Duration.text = consultant.turnsLeft.ToString();
    }
}
