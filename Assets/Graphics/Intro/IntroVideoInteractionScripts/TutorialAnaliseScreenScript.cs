using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialAnaliseScreenScript : MonoBehaviour {

    private int isOpen = Animator.StringToHash("isOpen");
    public Animator NavMenuAnimator;


    [System.Serializable]
    public class RobotExplanation {

        public static GameObject robot;

        public string message;
        public Vector2 robotPosition;
        public AudioClip voice;
        public GameObject selection;

        public void Enable() {
            robot.GetComponent<Text>().text = message;
            robot.transform.position = robotPosition;
            if(voice != null)
                robot.GetComponent<AudioSource>().PlayOneShot(voice);

            if(selection != null) {
                selection.SetActive(true);
            }
        }

        public void Disable() {
            robot.GetComponent<Text>().text = "";
            if (voice != null)
                robot.GetComponent<AudioSource>().Stop();

            if (selection != null) {
                selection.SetActive(false);
            }
        }
    }

    //[SerializeField]
    //public RobotExplanation[] robotExplanations = new RobotExplanation[10];


    //GameObject robotGameObject;
    public Text robotText;

	// Use this for initialization
	IEnumerator Start () {
        NavMenuAnimator.SetBool(isOpen, !NavMenuAnimator.GetBool(isOpen));
        //INIT
        FindObjectOfType<Debug_SecondarySideMenuManager>().Set(5);

        //PLAY
        //RobotExplanation.robot = robotGameObject;

        //This is the Analisys screen
        /*for (int i = 0; i < robotExplanations.Length; i++) {
            robotExplanations[i].Enable();
            yield return new WaitUntil(() => Input.anyKeyDown);
            robotExplanations[i].Disable();
        }*/
        robotText.text = "This is the Analisys screen.\nClick anywhere to continue";
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => Input.anyKeyDown);
        robotText.text = "You can minimize the side panel by clicking on the minimize handler. Try it!";
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => !NavMenuAnimator.GetBool(isOpen));
        robotText.text = "Wonderful! Open the menu again, so we can look what we can do it with it.";
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => NavMenuAnimator.GetBool(isOpen));
        robotText.text = "As you can see, there are different buttons. Click on the Markup button";
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => FindObjectOfType<Debug_SecondarySideMenuManager>().currentScreen == 0);
        robotText.text = "This is the markup menu, where there are three markup colours represent different levels of confidence from very to no so confident. You can select one of them and place it on the image.\nNow, Click on the Image Processing";
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => FindObjectOfType<Debug_SecondarySideMenuManager>().currentScreen == 1);
        robotText.text = "Here, you will find various image processing features such as brightness, greyscale, contrast.  Use these tools to change the image. In some cases you might find that adjusting the contrast or brightness will help you in identifying features. Try it!";
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => Input.anyKeyDown);
        robotText.text = "To make things easier, you can access all of these functions with the clicky wheel. Try to Right click to access the clicky wheel";
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(1));
        robotText.text = "The same features that we just explored are here, I, C, B, Gr, S and Ga represent image process effects, the red, green, and yellow dots represent markups. Furthermore, there is a tool to orient the direction of the markup point.\nYou can close it by right clicking again or moving your mouse away.";
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => !FindObjectOfType<ClickyWheelMenu>().isWheelOpen());
        robotText.text = "Once you have finished click \"Submit\" to submit the print to the database";
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => Input.anyKeyDown);
        robotText.text = "Congratulations! You finished your first print. Remember that you can revisit the tutorial at any time. For now, try your luck at the other prints! Click to continue.";
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => Input.anyKeyDown);
        SceneManager.LoadScene("00_IntroPage");



    }

    // Update is called once per frame
    void Update () {
		
	}


    /*This is the Analisys screen

    You can minimize the side panel by clicking on the minimize button.

    Now Open the menu again

    Now Click on the Markup button

    This is the markup menu, where there are three markup colours represent different levels of confidence from very to no so confident. You can select one of them and place it on the image. Now Click on the Image Processing.

    Here, you will find various image processing features such as brightness, greyscale, contrast.  Use these tools to change the image. In some cases you might find that adjusting the contrast or brightness will help you in identifying features.

    To make things easier, you can access all of these functions with the clicky wheel. Try to Right click to access the clicky wheel.

    The same features that we just explored are here, I, C, B, Gr, S and Ga represent image process effects, the red, green, and yellow dots represent markups. This represent the direction of the markup point. *points to direction*

    Once you have finished click "Submit" to submit the print to the database.


    Congratulations….you finished your first print.
    Remember that you can revisit the tutorial at any time. For now, try your luck at the other prints! */

}
