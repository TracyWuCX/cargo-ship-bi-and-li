using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class OceanFloorUI : MonoBehaviour
{
    [Header("=== Center Settings ===")]
    public GameObject CenterField;

    [Header("=== Exit Settings ===")]
    public GameObject ExitField;

    [Header("=== Menu Settings ===")]
    public GameObject MenuField;

    [Header("=== Information Settings ===")]
    public GameObject infoField;
    public Text EnergyWarning;

    [Header("=== Task Settings ===")]
    public GameObject taskField;
    public Text Task1;

    [Header("=== Credit Settings ===")]
    public GameObject creditField;
    public GameObject fishParent;
    public Text creditText;
    public Text fantasticText;
    private int totalFish;
    private int credit;

    [Header("=== Energy Settings ===")]
    public GameObject energyField;
    public Text energyText;
    public Image energyPool;
    public Boat boat;
    private float energyPersentage;

    [Header("=== Cargo Settings ===")]
    public GameObject cargoField;

    [Header("=== Finish Settings ===")]
    public GameObject finishField;

    // Awake is called on all objects in the scene before any object's Start function is called.
    private void Awake()
    {

    }

    // Start is called before the first frame update
    private void Start()
    {
        // Center Field

        // Exit Field

        // Menu Field

        // Information Field
        infoField.SetActive(true);
        EnergyWarning.enabled = false;

        // Task Field
        taskField.SetActive(true);

        // Credit Field
        totalFish = fishParent.GetComponent<Spawn>().currentAmount;
        creditField.SetActive(true);
        fantasticText.enabled = false;

        // Energy Field
        energyPersentage = 1f;
        energyField.SetActive(true);

        // Cargo Field
        cargoField.SetActive(true);

        // Finish Field
        finishField.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInfo();
        HandleTask();
        HandleCredit();
        HandleEnergy();
        HandleCargo();
        if (energyPersentage <= 0)
        {
            HandleFinish();
        }
    }

    private void HandleInfo()
    {
        if (0.2f <= energyPersentage && energyPersentage <= 0.3f)
        {
            EnergyWarning.enabled = true;
            EnergyWarning.text = "30% Energy Remaining";
        }
        else
        {
            EnergyWarning.enabled = false;
        }
    }

    private void HandleTask()
    {
        Task1.text = "Time For Fishing\n    Catch " + credit + "/" + totalFish + "fishes";
    }

    private void HandleCredit()
    {
        if (creditText == null)
        {
            return;
        }
        credit = totalFish - fishParent.GetComponent<Spawn>().currentAmount;
        creditText.text = "Credits : " + (int)credit;

        if (credit >= totalFish - 2)
        {
            fantasticText.enabled = true;
        }
    }

    private void HandleEnergy()
    {
        if (energyText == null) {
            return;
        }
        energyPersentage = boat.energyPersentage;
        energyText.text = "Energy\n" + (int)(energyPersentage * 100);
        energyPool.fillAmount = energyPersentage;

    }

    private void HandleCargo()
    {

    }

    private void HandleFinish()
    {
        finishField.SetActive(true);
        energyField.SetActive(false);
        cargoField.SetActive(false);
    }

}
