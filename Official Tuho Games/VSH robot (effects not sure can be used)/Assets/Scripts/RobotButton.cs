using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotButton : MonoBehaviour
{
    public bool moveForwardButton = true;
    public bool isSelected;
    public RobotHandler handler;

    private void Start()
    {
        handler = FindObjectOfType<RobotHandler>();
    }

    private void Update()
    {
        if(isSelected)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(moveForwardButton)
                {
                    handler.MoveRobotForward();
                }
                else
                {
                    handler.MoveRobotToPortal();
                }
            }
        }
    }
}
