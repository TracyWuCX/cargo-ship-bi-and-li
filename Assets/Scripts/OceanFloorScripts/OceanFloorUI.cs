using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OceanFloorUI : MonoBehaviour
{
    [Header("=== Information Settings ===")]
    public GameObject infoField;
    public Text depthText;
    public int currentDepth;

    [Header("=== Task Settings ===")]
    public GameObject taskField;

    [Header("=== Cargo Settings ===")]
    public GameObject cargoField;

    [Header("=== Energy Settings ===")]
    public GameObject energyField;
    public Text energyText;
    public Image energyPool;
    public float totalEnergy;
    [Range(0.111f, 1.999f)] public float energyDeductionRate;
    public float currentEnergy;

    [Header("=== Finish Settings ===")]
    public GameObject fishParent;
    public GameObject finishField;
    public Text creditText;
    public int credits = 0;

    // Start is called before the first frame update
    private void Start()
    {
        // Information Field
        infoField.SetActive(true);
        // Energy Field
        currentEnergy = totalEnergy;
        energyField.SetActive(true);
        // Finishi Field
        finishField.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInfo();
        HandleEnergy();
        HandleFinish();
    }

    private void HandleInfo()
    {
        if (depthText == null)
        {
            return;
        }

        depthText.text = "Depth: " + (int)currentDepth + "m";

    }

    private void HandleEnergy()
    {
        if (energyText == null) {
            return;
        }

        energyText.text = "Energy\n" + (int)currentEnergy;
        energyPool.fillAmount = currentEnergy / totalEnergy;

        currentEnergy -= Time.deltaTime * energyDeductionRate;

        if (currentEnergy <= 0)
        {
            Time.timeScale = 0; // Pause = 0, Start = 1
            energyField.SetActive(false);
            finishField.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HandleFinish()
    {
        if (creditText == null)
        {
            return;
        }
        credits = fishParent.GetComponent<Spawn>().pointAmount - fishParent.GetComponent<Spawn>().currentAmount;
        creditText.text = "Credits : " + (int)credits;
    }

}
