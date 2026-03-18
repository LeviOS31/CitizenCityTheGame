using UnityEngine;
using UnityEngine.UI;

public class Consultantcontroller : MonoBehaviour
{
    public Slider progressbar;
    public Animator animator;

    private bool startbar = false;

    private void Update()
    {
        if (!startbar) return;

        if (progressbar.value < 89.5f)
        {
            progressbar.value += Time.deltaTime; // Adjust the speed of progress here
        }
        else if (progressbar.value > 89.5f) 
        {
            startbar = false;
            progressbar.value = 0;
            animator.SetTrigger("Return-left");
        }
    }

    public void StartProgressBar()
    {
        progressbar.value = 0; // Reset the progress bar
        startbar = true;
    }
}
