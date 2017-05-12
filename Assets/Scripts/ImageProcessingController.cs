/* Copyright (C) 2017 Francesco Sapio - All Rights Reserved
 * The license use of this code and/or any portion is restricted to the 
 * the project DELTA CORE, subject to the condition that this
 * copyright notice shall remain.
 *  
 * Modification, distribution, reproduction or sale of this 
 * code for purposes outside of the license agreement is 
 * strictly forbidden.
 * 
 * NOTICE:  All information, intellectual and technical
 * concepts contained herein are, and remain, the property of Francesco Sapio.
 *
 * Attribution is required.
 * Please write to: contact@francescosapio.com
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ImageProcessingController : MonoBehaviour {

	public static ImageProcessingController instance;	

    [HideInInspector]
    float brightness, contrast;
    [HideInInspector]
    bool grayscale, invert, gamma;

    Sprite sp;

	Sprite importedSp;

    SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start ()
	{
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        sp = spriteRenderer.sprite;
        contrast = 1;
	}

    public void Process()
	{
        StopCoroutine(ProcessAsync());
        StartCoroutine(ProcessAsync());
    }

    public GameObject loadingDisplayer;

    public IEnumerator ProcessAsync()
	{
        loadingDisplayer.SetActive(true);
		/*
		Debug.Log(	"spriteRenderer.sprite.name: " + spriteRenderer.sprite);
		Debug.Log(	"sp.texture: " + sp.texture.name + 
					"brightness: " + brightness + 
					"contrast: " + contrast + 
					"grayscale: " + grayscale + 
					"invert: " + invert + 
					"gamma: " + gamma);
		*/

		// Process sprite with new values
        yield return DeltaCorePrototypeImageProcessingLib.ProcessAsync(sp.texture, brightness, contrast, grayscale, invert, gamma);
        // Update sprite with new values
		spriteRenderer.sprite = DeltaCorePrototypeImageProcessingLib.lastAsyncProcessedSprite;
		//Play sound
		AudioController.instance.SmallButtonClick();
        loadingDisplayer.SetActive(false);

    }

    private void logAction(string action, string tag = "")
    {
        if ( UserInfo.currentGameMode == DeltaCore.GameMode.Training)
        {
            TrainingAnalyseScreenScript.LogAction(action, tag);
        }
        else
        {
            AnalyseScreenScript.LogAction(action, tag);
        }
    }

    public void SetBrightness(float brightness) {
        this.brightness = brightness;
        Process();
        logAction("Fingerprint has now a brightness level of: " + brightness, "imageProcessing");
    }

    public void SetContrast(float contrast) {
        this.contrast = contrast;
        Process();
        logAction("Fingerprint has now a contrast level of: " + contrast, "imageProcessing");
    }

    public void SetGrayScale(bool grayscale) {
        this.grayscale = grayscale;
        Process();
        logAction("Fingerprint grayscale: " + grayscale, "imageProcessing");
    }

    public void SetInvert(bool invert) {
        this.invert = invert;
        Process();
        logAction("Fingerprint invert: " + invert, "imageProcessing");
    }

    public void SetGamma(bool gamma) {
        this.gamma = gamma;
        Process();
        AnalyseScreenScript.LogAction("Fingerprint gamma: " + gamma, "imageProcessing");
    }

	public void setSprite(Sprite newSp)
	{
		spriteRenderer = this.GetComponent<SpriteRenderer>();

		spriteRenderer.sprite = newSp;
		sp = spriteRenderer.sprite;

        //uiImage.SetNativeSize();
        
		Process();
    }

    public void Reset() {
        contrast = 0.5f;
        brightness = 0;
        grayscale = false;
        invert = false;
        gamma = false;
        Process();
        logAction("Fingerprint has been reset to defult values", "imageProcessing");
    }


    Vector2 lastMouseClick;
    Vector3 lastRotation;
    Vector3 lastPosition;

    void Update() {

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            lastMouseClick = Input.mousePosition;
            lastRotation = transform.eulerAngles;
        }


        //Temporary code for rotation, replace with a rotation system that takes into consideration the starting rotation level of the image.
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Mouse0)) {
            Vector3 mouse_pos = Input.mousePosition;
            mouse_pos.z = 10; //The distance between the camera and object
            Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90f));
        }
    }
}
