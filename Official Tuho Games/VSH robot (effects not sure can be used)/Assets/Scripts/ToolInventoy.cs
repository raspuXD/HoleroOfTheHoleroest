using System.Collections;
using UnityEngine;

public class ToolInventory : MonoBehaviour
{
    public Tool[] tools;
    private Tool currentTool;
    public bool IsOpenHanded => currentTool == null;
    public GameObject theHotbar;
    public Animator animator;
    private bool canSwitch = true;
    public bool holdsPart = false;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (!canSwitch) return;
        if (holdsPart) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(SwitchToolCooldown(null));
        }
        else
        {
            for (int i = 1; i < tools.Length + 1; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    StartCoroutine(SwitchToolCooldown(tools[i - 1]));
                }
            }
        }
    }

    IEnumerator SwitchToolCooldown(Tool newTool)
    {
        canSwitch = false;
        DeactivateAllTools();
        currentTool = newTool;

        if (currentTool != null)
        {
            currentTool.gameObject.SetActive(true);
            currentTool.isSelected = true;
        }

        theHotbar.SetActive(true);
        animator.SetTrigger("Show");

        yield return new WaitForSeconds(2f);

        theHotbar.SetActive(false);
        canSwitch = true;
    }

    void DeactivateAllTools()
    {
        foreach (Tool tool in tools)
        {
            tool.isSelected = false;
            tool.gameObject.SetActive(false);
        }
    }
}