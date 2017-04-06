using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen2Script : MonoBehaviour {

    Animator animator;
    public GameObject nextScreen;
    public AudioClip welcome, PlaceFinger, oneMoment;
    public UnityEngine.UI.Text subtitle;

    // Use this for initialization
    IEnumerator Start () {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        animator.Play("LogoFade");
        audioSource.PlayOneShot(welcome);
        subtitle.text = "Welcome to Deltacore";
        yield return new WaitForSeconds(welcome.length);
        audioSource.PlayOneShot(PlaceFinger);
        subtitle.text = "To access the training module, please place your finger on the touchpad or left mouse button and click";
        yield return new WaitUntil(() => Input.anyKeyDown);
        audioSource.PlayOneShot(oneMoment);
        subtitle.text = "Thank you! One moment please, while we register your print in the database";
        yield return new WaitForSeconds(PlaceFinger.length/2);
        subtitle.text = "";
        animator.Play("Scanning");
    }

   /* bool firstTime = true;
    float myTime;
    // Update is called once per frame
    void Update () {
        if (myTime > welcome.length) {
            if (Input.anyKeyDown && firstTime) {
                firstTime = false;
                audioSource.PlayOneShot(oneMoment);
                animator.Play("Scanning");
            }
        }
        else {
            myTime += Time.deltaTime;
            if(myTime > welcome.length) {
                audioSource.PlayOneShot(PlaceFinger);
            }
        }
	}*/


    AudioSource audioSource;
    public AudioClip scanning, databaseAccepted, voice, review;

    void playScanning() {
        audioSource.loop = true;
        audioSource.clip = scanning;
        audioSource.Play();
    }

    void onFinish() {
        StartCoroutine(onFinishEnumerator());

    }

    IEnumerator onFinishEnumerator() {
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.PlayOneShot(databaseAccepted);
        yield return new WaitForSeconds(databaseAccepted.length);
        audioSource.PlayOneShot(voice);
        subtitle.text = "Fingerprint registed in Database.";
        animator.Play("PrintDisappears");
        yield return new WaitForSeconds(voice.length);
        audioSource.PlayOneShot(review);
        yield return new WaitForSeconds(0.2f);
        subtitle.text = "To begin your training, let's review the different types of finger prints";
        yield return new WaitForSeconds(review.length);
        Debug.Log("MOVE ON");
        nextScreen.SetActive(true);
        gameObject.SetActive(false);
        subtitle.text = "";
    }
}
