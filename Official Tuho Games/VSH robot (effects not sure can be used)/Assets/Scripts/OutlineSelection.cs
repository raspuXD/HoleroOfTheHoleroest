using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    public BrokenPart currentPart;
    RobotButton theButton;
    public float distance = 4f;
    UseText text;
    RobotHandler handler;
    CollectPart collect;
    private void Start()
    {
        text = FindObjectOfType<UseText>();
        handler = FindObjectOfType<RobotHandler>();
    }

    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit, distance)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;

            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                    currentPart = highlight.gameObject.GetComponent<BrokenPart>();
                    currentPart.canCurrentlyBeFixed = true;
                }
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                    highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                }
            }
            else if(highlight.CompareTag("Button") && highlight != selection)
            {
                theButton = highlight.gameObject.GetComponent<RobotButton>();

                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                    theButton.isSelected = true;

                    if(theButton.moveForwardButton)
                    {
                        if (handler.currentState == RobotHandler.RobotState.MovingToMiddle)
                        {
                            text.WriteTheText("The Robot is coming");
                        }
                        else if (handler.currentState == RobotHandler.RobotState.None || handler.currentRobot == null)
                        {
                            text.WriteTheText("Bring a new Robot <u><color=green>E");
                        }
                        else if (handler.currentState == RobotHandler.RobotState.MovingToPortal || handler.currentState == RobotHandler.RobotState.MovingToEnd)
                        {
                            text.WriteTheText("The Robot is going for its destiny");
                        }
                        else if (handler.currentState == RobotHandler.RobotState.AtMiddle && handler.IsFixedFully() && handler.DoesPartMiss.IsThePartPut())
                        {
                            text.WriteTheText("Send it towards battle field!");
                        }

                        else if(handler.currentState == RobotHandler.RobotState.AtMiddle && !handler.IsFixedFully() || !handler.DoesPartMiss.IsThePartPut())
                        {
                            //make a checker for to check if can send yet
                            text.WriteTheText("Fix the robot first");
                        }
                        
                    }
                    else
                    {
                        if (handler.currentState == RobotHandler.RobotState.MovingToMiddle)
                        {
                            text.WriteTheText("The Robot is coming");
                        }
                        else if (handler.currentState == RobotHandler.RobotState.None || handler.currentRobot == null)
                        {
                            text.WriteTheText("Bring a new Robot with the <color=green>green <color=white>button");
                        }
                        else if (handler.currentState == RobotHandler.RobotState.MovingToPortal || handler.currentState == RobotHandler.RobotState.MovingToEnd)
                        {
                            text.WriteTheText("The Robot is going for its destiny");
                        }
                        else if (handler.currentState == RobotHandler.RobotState.AtMiddle && handler.IsFixedFully() && handler.DoesPartMiss.IsThePartPut())
                        {
                            text.WriteTheText("Send it towards the unknown");
                        }
                        else if (handler.currentState == RobotHandler.RobotState.AtMiddle && !handler.IsFixedFully() || !handler.DoesPartMiss.IsThePartPut())
                        {
                            //make a checker for to check if can send yet
                            text.WriteTheText("Fix the robot first");
                        }
                    }
                }
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                    highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                }
            }
            else if(highlight.CompareTag("CollectablePart") && highlight != selection)
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    collect = highlight.gameObject.GetComponent<CollectPart>();
                    if(collect != null && !collect.thePartInCamera.activeSelf && collect.inventory.IsOpenHanded)
                    {
                        highlight.gameObject.GetComponent<Outline>().enabled = true;
                        collect.canCollect = true;
                        text.WriteTheText("Equip the robot head");
                    }
                    else if(collect != null && !collect.thePartInCamera.activeSelf && !collect.inventory.IsOpenHanded)
                    {
                        text.WriteTheText("You need both hands for this");
                    }
                    else
                    {
                        text.UnWriteTheText();
                    }
                }
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                    highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                }
            }
            else
            {
                highlight = null;
                text.UnWriteTheText();
                if (theButton != null)
                {
                    theButton.isSelected = false;
                    theButton = null;
                }
            }
        }
        else
        {
            if (theButton != null)
            {
                theButton.isSelected = false;
                theButton = null;
                text.UnWriteTheText();
            }

            if (currentPart != null)
            {
                currentPart.RemoveTheText();
                currentPart.canCurrentlyBeFixed = false;
                currentPart = null;
            }

            if(collect != null)
            {
                collect.canCollect = false;
                collect = null;
                text.UnWriteTheText();
            }
        }
    }

}
