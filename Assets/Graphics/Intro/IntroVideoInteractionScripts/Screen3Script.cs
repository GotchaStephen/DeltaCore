using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen3Script : MonoBehaviour {

    public UnityEngine.UI.Text subtitle;
    Animator animator;
    public GameObject nextScreen;
    AudioSource audioSource;
    public AudioClip arch, whorl, loop, aLotToTake;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        animator.Play("0-ARCH");
        audioSource.PlayOneShot(arch);
        subtitle.text = "An arch is the most simplist type of print but it is also quite rare. Arch prints have ridges that begin on one side and end on the other side in a line that looks like an arch, hence the name.";
    }

    int numberOfClicks = 0;
    // Update is called once per frame
    void Update() {
        if (Input.anyKeyDown) {
            numberOfClicks++;
            switch (numberOfClicks) {
                case 1:
                    animator.Play("ARCH-WHORL");
                    audioSource.PlayOneShot(whorl);
                    subtitle.text = "Lastly, we have a Whorl pattern where the ridges create a circular pattern. Whorl patterns have at least two deltas.";
                    break;
                case 2:
                    animator.Play("WHORL-LOOP");
                    audioSource.PlayOneShot(loop);
                    subtitle.text = "Loop finger print types are the most common. Loop prints have ridges that begin and end on the same side. In every loop there is a triangular shaped pattern where the finger print ridges meet. This shape is known as a Delta.";
                    break;
                case 3:
                    animator.Play("LOOP-0");
                    audioSource.PlayOneShot(aLotToTake);
                    subtitle.text = "This is a lot to take in, but in time you begin to identify the different types of prints quite quickly, Now that you have a brief understanding about the different types of prints, lets look at more specific details.";
                    break;
            }
        }
    }

    public void onFinish() {
        StartCoroutine(onFinishEnumerator());
    }

    IEnumerator onFinishEnumerator() {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        nextScreen.SetActive(true);
        gameObject.SetActive(false);
        subtitle.text = "As you can see here, there are a number of features that identify a print. Some are easy to see, others will be more difficult, depending on the quality of the print. Scroll over each of the features within the print to know a bit more about them.";
    }
}
