﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreatmentGUI : MonoBehaviour {

	public Treatment resection;
	public Treatment radical;
	public Treatment laparoscopic;
	public Treatment laser;
	public Treatment Chloromethine;

	public Texture2D surgeryButton;
	public Texture2D radioButton;
	public Texture2D chemoButton;
	public Texture2D futureButton;
	public Texture2D goatButton;
	public Texture2D buttonImage;


	public Texture2D normalDefault = GUI.skin.button.normal.background;
	public Texture2D hoverDefault = GUI.skin.button.hover.background;
	public Texture2D activeDefault = GUI.skin.button.active.background;

	public GUISkin cooldownBarSkin;

	public AudioClip clickSound;

	private int openedButton = 0;		// 0 for no buttons

	private static Dictionary<TreatmentType, float> Cooldown;
	private static Dictionary<TreatmentType, float> CooldownMax;
	private List<TreatmentType> treatmentTypes = new List<TreatmentType>();

	void Start () {
		Cooldown = new Dictionary<TreatmentType, float>();
		Cooldown.Add(TreatmentType.Surgery, 0);
		Cooldown.Add(TreatmentType.Chemo, 0);
		Cooldown.Add(TreatmentType.Radiation, 0);
		Cooldown.Add(TreatmentType.Targeted, 0);
		Cooldown.Add(TreatmentType.FutureTech, 100);

		CooldownMax = new Dictionary<TreatmentType, float>();
		CooldownMax.Add(TreatmentType.Surgery, 100);
		CooldownMax.Add(TreatmentType.Chemo, 100);
		CooldownMax.Add(TreatmentType.Radiation, 100);
		CooldownMax.Add(TreatmentType.Targeted, 100);
		CooldownMax.Add(TreatmentType.FutureTech, 100);

		treatmentTypes = new List<TreatmentType>();
		foreach (TreatmentType type in Cooldown.Keys) {
			treatmentTypes.Add(type);
		}
	}

	void Update () {
		foreach (TreatmentType type in treatmentTypes) {
			if (Cooldown[type] > 0){
				Cooldown[type] -= Time.deltaTime;
				if (Cooldown[type] < 0)
				{
					Cooldown[type] = 0;
				}
			}
		}
	}

	public static void AddCooldown (TreatmentType type, float cooldown) {
		Cooldown[type] += cooldown;
		CooldownMax[type] = Cooldown[type];
	}

	public void EnableSurgery (Treatment surgery) {
		resection.gameObject.SetActive(false);
		radical.gameObject.SetActive(false);
		laparoscopic.gameObject.SetActive(false);
		laser.gameObject.SetActive(false);
		surgery.gameObject.SetActive(true);
	}

	void OnGUI () {

		var numButtons = 6;
		
		var buttonPad = Screen.width / 100;
		
		var buttonWidth = Screen.width / 10;
		var buttonHeight = ((Screen.height - buttonPad*3) / (numButtons)) - buttonPad;
		
		var popWidth = Screen.width / 1.6;
		var popButtonWidth = ((popWidth - buttonPad) / 4) - buttonPad;
		
		Event e = Event.current;

		var iBut = 0;
		var jBut = 0;

		var openTab = new Rect(0, 0, 0, 0);
		
		// Make a background box
		GUI.Box(new Rect(buttonPad, buttonPad, buttonWidth + buttonPad + buttonPad, Screen.height - buttonPad - buttonPad), "");





		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		buttonImage = surgeryButton;
	//	GUI.skin.button.normal.background = buttonImage;
	//	GUI.skin.button.hover.background = buttonImage;
	//	GUI.skin.button.active.background = buttonImage;
		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), buttonImage)) {
			AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);


			if(openedButton == iBut + 1)
			{
				openedButton = 0;
			}
			else
			{
				openedButton = iBut + 1;
			}
		}


		
		if (openedButton == iBut + 1) {
			GUI.Box(new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight), "");
			openTab = new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight);

			jBut = 0;

			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Resection", "The earliest recorded surgery to remove a tumour was performed in ancient Egypt, more than 4,000 years ago.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// a cancel button appears where the original button was
				// mouse highlights a medium sized hexagon region until the player clicks
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			all cells in the region die immediately
				//			draw surgery effect, play surgery sound
				if (Cooldown[TreatmentType.Surgery] <= 0)
					EnableSurgery(resection);

				openedButton = 0;
			}
			jBut++;

			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Radical Resection", "Radical surgery was pioneered by William Stewart Halsted.  In 1882, he performed the first radical mastectomy, in which a breast cancer tumour was treated by removing the entire breast, underlying muscles and nearby lymph nodes.  This aggressive, disfiguring surgery was the standard treatment for breast cancer until the 1970s.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// a cancel button appears where the original button was
				// mouse highlights a large sized hexagon region until the player clicks
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			all cells in the region die immediately
				//			draw surgery effect, play surgery sound
				if (Cooldown[TreatmentType.Surgery] <= 0)
					EnableSurgery(radical);

				openedButton = 0;
			}
			jBut++;

			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Laparoscopic Resection", "Minimally invasive laparoscopic surgeries to resect portions of the colon have recently become popular due to lower risks and decreased recovery time.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// a cancel button appears where the original button was
				// mouse highlights a small sized hexagon region until the player clicks
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			all cells in the region die immediately
				//			draw surgery effect, play surgery sound
				if (Cooldown[TreatmentType.Surgery] <= 0)
					EnableSurgery(laparoscopic);

				openedButton = 0;
			}
			jBut++;

			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Laser Ablation", "Focal laser ablation is currently in clinical trials as a way to directly target and destroy tumour tissue in prostate cancers.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// a cancel button appears where the original button was
				// first mouse click anchors a line and then moving the mouse changes the direction of the line
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			all cells in the line region die immediately
				//			draw laser effect, play laser sound (pew pew)
				if (Cooldown[TreatmentType.Surgery] <= 0)
					EnableSurgery(laser);

				openedButton = 0;
			}
			jBut++;

			GUI.Label (new Rect ((float)(buttonWidth + buttonPad * 5), (float)((iBut * (buttonHeight + buttonPad)) + buttonHeight) - 10, (float)popWidth, 40), GUI.tooltip);


		}
		iBut++;




		// Make the second button.
		buttonImage = radioButton;
	//	GUI.skin.button.normal.background = buttonImage;
	//	GUI.skin.button.hover.background = buttonImage;
	//	GUI.skin.button.active.background = buttonImage;
		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), buttonImage)) {
			AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
			if(openedButton == iBut + 1)
			{
				openedButton = 0;
				
			}
			else
			{
				openedButton = iBut + 1;
			}
		}
		
		if (openedButton == iBut + 1) {
			GUI.Box(new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight), "");

			openTab = new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight);

			jBut = 0;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("X-Ray Radiotherapy", "X-Rays were discovered by Wilhelm Röntgen in 1895.  Emil Grubbe started treating cancer patients with x-rays just one year later.  Marie Curie discovered two new radioactive elements (polonium and radium) in 1898 which kicked off a new era in medical treatment and research.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// a cancel button appears where the original button was
				// the entire grid is highlighted
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			normal cell death rate is increased (doubled?) for a time (5 seconds?) globally
				//			cancer cell death rate is greatly increased (quadrupled?) for a time (same as above) globally
				//			all cell mutation rate is slightly increased (1.5x? - multiply is no good if normal rate is zero by default) for a time (same as above) globally
				//			draw radiation effect, play radiation sound
				openedButton = 0;
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Brachytherapy", "Brachytherapy, the process of inserting radioactive seeds directly into a tumour, was first attempted in 1901.  Iridium pellets, the most common brachytherapy radiation source used to treat many different cancer types today, were first employed in 1958.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// a cancel button appears where the original button was
				// a medium sized region around the mouse is highlighter
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			the hex cell is replaced by a radioactive seed - cells can no longer occupy the space!
				//			normal cell death rate is increased (doubled?) in the region forever
				//			cancer cell death rate is greatly increased (quadrupled?) in the region forever
				//			all cell mutation rate is slightly increased (1.5x? - multiply is no good if normal rate is zero by default) in the region forever
				//			draw radiation effect, play radiation sound
				openedButton = 0;
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Gamma Knife", "The Gamma Knife administers high-intensity cobalt radiation in short bursts at many different angles centered on the tumour, such that the cancer receives a lethal dose but the surrounding tissue is spared.  It was invented in Sweden in 1967.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// a cancel button appears where the original button was
				// a small region around the mouse is highlighted
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			normal cell death rate is increased (doubled?) for a time (5 seconds?) in the region
				//			cancer cell death rate is greatly increased (quadrupled?) for a time (same as above) in the region
				//			all cell mutation rate is slightly increased (1.5x? - multiply is no good if normal rate is zero by default) for a time (same as above) in the region
				//			draw radiation effect, play radiation sound
				openedButton = 0;
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Proton Radiotherapy", "Higher energy particles deliver higher doses of radiation with increased precision, and are often used to treat cancers such as ocular and skull base tumours.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// a cancel button appears where the original button was
				// the first click anchors the beam on a cell, and then moving the mouse changes the orientation of the beam, which is 3 or 5 hexs wide and goes across the whole grid
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			normal cell death rate is greatly increased (tripled?) for a time (5 seconds?) in the beam
				//			cancer cell death rate is very greatly increased (eight times?) for a time (same as above) in the beam
				//			all cell mutation rate is increased (2.5x? - multiply is no good if normal rate is zero by default) for a time (same as above) in the beam
				//			draw radiation effect, play radiation sound
				openedButton = 0;
			}
			jBut++;

			GUI.Label (new Rect ((float)(buttonWidth + buttonPad * 5), (float)((iBut * (buttonHeight + buttonPad)) + buttonHeight) - 10, (float)popWidth, 40), GUI.tooltip);
		}
		iBut++;




		buttonImage = chemoButton;
	//	GUI.skin.button.normal.background = buttonImage;
	//	GUI.skin.button.hover.background = buttonImage;
	//	GUI.skin.button.active.background = buttonImage;
		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), buttonImage)) {
			AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
			if(openedButton == iBut + 1)
			{
				openedButton = 0;
				
			}
			else
			{
				openedButton = iBut + 1;
			}
		}
		
		if (openedButton == iBut + 1) {
			GUI.Box(new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight), "");

			openTab = new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight);

			jBut = 0;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Chloromethine", "Chlormethine, also known as mustine, is a derivative of mustard gas that was tested on humans with lymphoma and leukemia in 1942 after doctors noticed very low white blood cell counts in mustard gas attack survivors.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// a cancel button appears where the original button was
				// the entire grid is highlighted
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			normal cell death rate is greatly increased (tripled?) for a time (5 seconds?) globally
				//			cancer cell death rate is very greatly increased (six times?) for a time (same as above) globally
				//			draw chemo effect, play chemo sound
				if (Cooldown[TreatmentType.Chemo] <= 0){
					Chloromethine.gameObject.SetActive(true);
				}

				openedButton = 0;
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Methotrexate", "Sidney Farber’s work in the middle of the 20th century treating children suffering from leukemia with antifolates such as aminopterin and methotrexate was instrumental in proving that drugs can cause remission in cancer.  He went on to become a prominent advocate for cancer research, raising millions of dollars in donations and government funding.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// a cancel button appears where the original button was
				// the entire grid is highlighted
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			normal cell division rate is reduced (halved?) for a time (5 seconds?) globally
				//			cancer cell division rate is reduced (thirded?) for a time (5 seconds?) globally
				//			normal cell death rate is increased (doubled times?) for a time (same as above) globally
				//			cancer cell death rate is increased (tripled times?) for a time (same as above) globally
				//			draw chemo effect, play chemo sound
				openedButton = 0;
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Doxorubicin", "In 1950, Italian and French researchers isolated daunorubicin from a red coloured bacteria that lived in the soil around a 13th century castle.  In 1967 researchers learned that daunorubicin could cause fatal cardiac toxicity.  Doxorubicin is a more effective derivative.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// a cancel button appears where the original button was
				// the entire grid is highlighted
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			normal cell division rate is greatly reduced (thirded?) for a time (5 seconds?) globally
				//			cancer cell division rate is greatly reduced (1/5?) for a time (5 seconds?) globally
				//			normal cell death rate is increased (1.5x times?) for a time (same as above) globally
				//			cancer cell death rate is increased (doubled?) for a time (same as above) globally
				//			draw chemo effect, play chemo sound
				openedButton = 0;
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Cisplatin", "Cisplatin was found to interfere with E.Coli cell division in 1965 at Michigan Statue University, and 4 years later it was shown to reduce the size of tumours in rats.  It was approved for use in human cancers in 1978.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// a cancel button appears where the original button was
				// the entire grid is highlighted
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			normal cell division rate is reduced (halved?) for a time (5 seconds?) globally
				//			cancer cell division rate is reduced (halved?) for a time (5 seconds?) globally
				//			normal cell death rate is increased (doubled times?) for a time (same as above) globally
				//			cancer cell death rate is increased proportional to its current mutation rate (higher mutation = higher rate) for a time (same as above) globally
				//			draw chemo effect, play chemo sound
				openedButton = 0;
			}
			jBut++;

			GUI.Label (new Rect ((float)(buttonWidth + buttonPad * 5), (float)((iBut * (buttonHeight + buttonPad)) + buttonHeight) - 10, (float)popWidth, 40), GUI.tooltip);
		}
		iBut++;



//		No time for these
//
//
//		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), "Genetics")) {
//			if(openedButton == iBut + 1)
//			{
//				openedButton = 0;
//				
//			}
//			else
//			{
//				openedButton = iBut + 1;
//			}
//		}
//		
//		if (openedButton == iBut + 1) {
//			GUI.Box(new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight), "");
//
//			openTab = new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight);
//
//			jBut = 0;
//			
//			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Sanger Sequencing"))) {
//			}
//			jBut++;
//			
//			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Exome Sequencing"))) {
//			}
//			jBut++;
//			
//			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Whole Genome Sequencing"))) {
//			}
//			jBut++;
//			
//			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Single Cell Sequencing"))) {
//			}
//			jBut++;
//
//			GUI.Label (new Rect ((float)(buttonWidth + buttonPad * 5), (float)((iBut * (buttonHeight + buttonPad)) + buttonHeight) - 10, (float)popWidth, 40), GUI.tooltip);
//		}
//		iBut++;
//
//
//
//
//
//
//		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), "Targeted Drugs")) {
//			if(openedButton == iBut + 1)
//			{
//				openedButton = 0;
//				
//			}
//			else
//			{
//				openedButton = iBut + 1;
//			}
//		}
//		
//		if (openedButton == iBut + 1) {
//			GUI.Box(new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight), "");
//
//			openTab = new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight);
//
//			jBut = 0;
//			
//				if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Trastuzumab"))) {
//			}
//			jBut++;
//			
//				if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Imatinib"))) {
//			}
//			jBut++;
//			
//				if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Vemurafenib"))) {
//			}
//			jBut++;
//			
//			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Crizotinib"))) {
//			}
//			jBut++;
//
//			GUI.Label (new Rect ((float)(buttonWidth + buttonPad * 5), (float)((iBut * (buttonHeight + buttonPad)) + buttonHeight) - 10, (float)popWidth, 40), GUI.tooltip);
//		}
//		iBut++;
//





		buttonImage = futureButton;
		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), futureButton)) {
			AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
			if(openedButton == iBut + 1)
			{
				openedButton = 0;
				
			}
			else
			{
				openedButton = iBut + 1;
			}
		}

		// starts witha cooldown
		if (openedButton == iBut + 1) {
			GUI.Box(new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight), "");

			openTab = new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight);

			jBut = 0;
			
					if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Immunotherapy"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// cancer death rate goes up
			}
			jBut++;
			
					if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Oncolytic Virus"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// cancer death greatly increases
				// normal death rate slightly increases
			}
			jBut++;
			
					if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent("Nanoparticles"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// cancer division rate greatly reduced
			}
			jBut++;
			
					if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*4)), new GUIContent(goatButton, "No explanation necessary.\n"))) {
				AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				// dumb spammy click a cell to kill it goat lol
			}
			jBut++;

			GUI.Label (new Rect ((float)(buttonWidth + buttonPad * 5), (float)((iBut * (buttonHeight + buttonPad)) + buttonHeight) - 10, (float)popWidth, 40), GUI.tooltip);
		}
		iBut++;


		iBut++;
		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), "Quit")) {
			AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
			Application.Quit();
		}




		// draw cooldown bars
		GUI.skin = cooldownBarSkin;
		GUI.color = new Color (1, 0, 0, 0.3f);
		iBut = 0;

		GUI.Box(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)((buttonWidth * Cooldown[TreatmentType.Surgery]) / CooldownMax[TreatmentType.Surgery]), (float)buttonHeight), "");
		iBut++;
		// ...







		// check for clicks outside the GUI and close any open tab
		if (e.type == EventType.MouseDown && !openTab.Contains(e.mousePosition))
		{
			openedButton = 0;
		}
	}
}