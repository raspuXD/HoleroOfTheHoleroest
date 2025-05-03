using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPart : MonoBehaviour
{
    [SerializeField] string partName, toolName;
    public Tool theToolNeededToFix;
    public float howFixed = 0;
    public float howMuchIsNeededToFix = 100;

    public bool hasBeenFixed;

    public bool canCurrentlyBeFixed = false;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    UseText text;
    public AudioSource hitSound, finSound;

    private void Start()
    {
        text = FindObjectOfType<UseText>();
        theToolNeededToFix = FindToolEvenIfInactive(toolName);
    }

    private Tool FindToolEvenIfInactive(string name)
    {
        Tool[] allTools = Resources.FindObjectsOfTypeAll<Tool>();
        foreach (Tool tool in allTools)
        {
            if (tool.name == name)
            {
                return tool;
            }
        }
        return null;
    }

    public void Update()
    {
        if(canCurrentlyBeFixed)
        {
            if(!hasBeenFixed)
            {
                if (theToolNeededToFix.isSelected)
                {
                    text.uiText.color = Color.white;
                    text.WriteTheText("Fix with <u><color=green>E </u><color=white>or <u><color=green>Left Click");

                    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
                    {
                        HandleTheFixing(10);
                    }
                }
                else
                {
                    string toolName = theToolNeededToFix.gameObject.name;
                    text.uiText.color = Color.red;
                    text.WriteTheText("<color=white>You need <u><color=red>" + toolName + "</u><color=white> to fix <u><color=red>" + partName + "</color>");
                }
            }
            else
            {
                text.uiText.color = Color.green;
                text.WriteTheText("This part is already fixed");
            }
        }
    }

    void HandleTheFixing(float amount)
    {
        hitSound.Play();
        howFixed += amount;
        howFixed = Mathf.Clamp(howFixed, 0, howMuchIsNeededToFix);

        skinnedMeshRenderer.SetBlendShapeWeight(0, 100 - howFixed);

        if(howFixed == howMuchIsNeededToFix)
        {
            hasBeenFixed = true;
            finSound.Play();
        }
    }

    public void RemoveTheText()
    {
        text.uiText.color = Color.white;
        text.UnWriteTheText();
    }
}
