using UnityEngine;
using System.Collections;

public class TreatmentGUI : MonoBehaviour {
	
	private int openedButton = 0;		// 0 for no buttons
	
	void OnGUI () {
		
		var numButtons = 6;
		
		var buttonPad = Screen.width / 100;
		
		var buttonWidth = Screen.width / 10;
		var buttonHeight = ((Screen.height - buttonPad*3) / (numButtons)) - buttonPad;
		
		var popWidth = Screen.width / 1.6;
		var popButtonWidth = ((popWidth - buttonPad) / 4) - buttonPad;
		
		
		var iBut = 0;
		var jBut = 0;
		
		// Make a background box
		GUI.Box(new Rect(buttonPad, buttonPad, buttonWidth + buttonPad + buttonPad, Screen.height - buttonPad - buttonPad), "");




		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), new GUIContent("Surgery", "Kills selected cells dead whether they're cancer or not"))) {
			if(openedButton == iBut + 1)
			{
				openedButton = 0;
			}
			else
			{
				openedButton = iBut + 1;
			}
		}

		GUI.Label (new Rect (10,40,100,20), GUI.tooltip);
		
		if (openedButton == iBut + 1) {
			GUI.Box(new Rect(buttonWidth + buttonPad * 4, (float)(iBut * (buttonHeight + buttonPad) + buttonPad * 2), (float)popWidth, (float)buttonHeight), "");
			
			jBut = 0;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Resection")) {
				// a cancel button appears where the original button was
				// mouse highlights a medium sized hexagon region until the player clicks
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			all cells in the region die immediately
				//			draw surgery effect, play surgery sound
						
			}
			jBut++;

			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Radical Resection")) {
				// a cancel button appears where the original button was
				// mouse highlights a large sized hexagon region until the player clicks
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			all cells in the region die immediately
				//			draw surgery effect, play surgery sound
			}
			jBut++;

			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Laparoscopic Resection\t")) {
				// a cancel button appears where the original button was
				// mouse highlights a small sized hexagon region until the player clicks
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			all cells in the region die immediately
				//			draw surgery effect, play surgery sound
			}
			jBut++;

			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Laser Ablation")) {
				// a cancel button appears where the original button was
				// first mouse click anchors a line and then moving the mouse changes the direction of the line
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			all cells in the line region die immediately
				//			draw laser effect, play laser sound (pew pew)
			}
			jBut++;
		}
		iBut++;




		// Make the second button.
		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), "Radiation")) {
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
			
			jBut = 0;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "X-Ray Radiotherapy")) {
				// a cancel button appears where the original button was
				// the entire grid is highlighted
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			normal cell death rate is increased (doubled?) for a time (5 seconds?) globally
				//			cancer cell death rate is greatly increased (quadrupled?) for a time (same as above) globally
				//			all cell mutation rate is slightly increased (1.5x? - multiply is no good if normal rate is zero by default) for a time (same as above) globally
				//			draw radiation effect, play radiation sound
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Brachytherapy")) {
				// a cancel button appears where the original button was
				// a medium sized region around the mouse is highlighter
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			the hex cell is replaced by a radioactive seed - cells can no longer occupy the space!
				//			normal cell death rate is increased (doubled?) in the region forever
				//			cancer cell death rate is greatly increased (quadrupled?) in the region forever
				//			all cell mutation rate is slightly increased (1.5x? - multiply is no good if normal rate is zero by default) in the region forever
				//			draw radiation effect, play radiation sound
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Gamma Knife")) {
				// a cancel button appears where the original button was
				// a small region around the mouse is highlighted
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			normal cell death rate is increased (doubled?) for a time (5 seconds?) in the region
				//			cancer cell death rate is greatly increased (quadrupled?) for a time (same as above) in the region
				//			all cell mutation rate is slightly increased (1.5x? - multiply is no good if normal rate is zero by default) for a time (same as above) in the region
				//			draw radiation effect, play radiation sound
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Proton Radiotherapy")) {
				// a cancel button appears where the original button was
				// the first click anchors the beam on a cell, and then moving the mouse changes the orientation of the beam, which is 3 or 5 hexs wide and goes across the whole grid
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			normal cell death rate is greatly increased (tripled?) for a time (5 seconds?) in the beam
				//			cancer cell death rate is very greatly increased (eight times?) for a time (same as above) in the beam
				//			all cell mutation rate is increased (2.5x? - multiply is no good if normal rate is zero by default) for a time (same as above) in the beam
				//			draw radiation effect, play radiation sound
			}
			jBut++;
		}
		iBut++;




		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), "Chemotherapy")) {
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
			
			jBut = 0;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Chloromethine")) {
				// a cancel button appears where the original button was
				// the entire grid is highlighted
				// 		if the player cancels, nothing happens
				// 		if the player clicks on a hex
				//			normal cell death rate is greatly increased (tripled?) for a time (5 seconds?) globally
				//			cancer cell death rate is very greatly increased (six times?) for a time (same as above) globally
				//			all cell mutation rate is increased (2.5x? - multiply is no good if normal rate is zero by default) for a time (same as above) in the beam
				//			draw radiation effect, play radiation sound
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Methotrexate")) {
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Doxorubicin")) {
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Cisplatin")) {
			}
			jBut++;
		}
		iBut++;
		
		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), "Genetics")) {
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
			
			jBut = 0;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Sanger Sequencing")) {
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Exome Sequencing")) {
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Whole Genome Sequencing")) {
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Single Cell Sequencing")) {
			}
			jBut++;
		}
		iBut++;
		
		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), "Targeted Drugs")) {
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
			
			jBut = 0;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Trastuzumab")) {
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Imatinib")) {
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Vemurafenib")) {
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "???")) {
			}
			jBut++;
		}
		iBut++;
		
		if(GUI.Button(new Rect(buttonPad * 2, (float)(iBut * (buttonHeight + buttonPad) + buttonPad*2), buttonWidth, (float)buttonHeight), "Future Tech")) {
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
			
			jBut = 0;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Immunotherapy")) {
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Oncolytic Virus")) {
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Nanoparticles")) {
			}
			jBut++;
			
			if(GUI.Button(new Rect((float)((buttonWidth + buttonPad * 5) + (popButtonWidth + buttonPad) * jBut), (float)(iBut * (buttonHeight + buttonPad) + buttonPad*3), (float)popButtonWidth, (float)(buttonHeight - buttonPad*2)), "Goat on a Stick")) {
			}
			jBut++;
		}
		iBut++;
	}
}