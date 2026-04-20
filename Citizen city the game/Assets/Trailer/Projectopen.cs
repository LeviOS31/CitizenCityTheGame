using UnityEngine;

public class Projectopen : MonoBehaviour
{
    public void Open()
    {
        GetComponent<Animator>().SetTrigger("open");
    }

    public void Close()
    {
        GetComponent<Animator>().SetTrigger("close");
    }
}
