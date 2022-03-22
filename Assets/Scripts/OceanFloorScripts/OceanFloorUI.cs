using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OceanFloorUI : MonoBehaviour
{
    public GameObject CenterField;
    public GameObject ReturnField;

    [Header("=== Information Settings ===")]
    public GameObject infoField;
    public Text TopText;
    public Text MiddleText;
    public Text ButtomText;

    [Header("=== Task Settings ===")]
    public GameObject taskField;
    public Text Task1;

    public GameObject fishParent;
    private int totalFish;
    private int catchedFish;

    [Header("=== Energy Settings ===")]
    public GameObject energyField;
    public Text energyText;
    public Image energyPool;
    public Boat boat;
    private float energyPersentage;

    [Header("=== Cargo Settings ===")]
    public GameObject cargoField;

    // Start is called before the first frame update
    private void Start()
    {
        CenterField.SetActive(true);
        ReturnField.SetActive(false);

        infoField.SetActive(true);
        TopText.enabled = false;
        MiddleText.enabled = false;
        ButtomText.enabled = false;

        taskField.SetActive(true);

        totalFish = fishParent.GetComponent<Spawn>().currentAmount;

        energyPersentage = 1f;
        energyField.SetActive(true);

        cargoField.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (energyPersentage <= 0)
        {
            HandleFinish();
        }
        HandleInfo();
        HandleTask();
        HandleEnergy();
        HandleCargo();
    }

    private void HandleInfo()
    {
        if (0f <= energyPersentage && energyPersentage <= 0.3f)
        {
            TopText.enabled = true;
            TopText.text = energyPersentage * 100 + "% Energy Remaining";
        }
        else
        {
            TopText.enabled = false;
        }

        if (PauseManager.paused)
        {
            MiddleText.text = "Paused";
            MiddleText.enabled = true;
        }
        else if (energyPersentage <= 0f)
        {
            MiddleText.text = "Low Energy\nReturn to Charge";
            MiddleText.enabled = true;
        }
        else
        {
            MiddleText.enabled = false;
        }

        ButtomText.text = "Use [Q] to enter/exit Driving Mode";
        ButtomText.enabled = true;
    }

    private void HandleTask()
    {
        catchedFish = totalFish - fishParent.GetComponent<Spawn>().currentAmount;
        Task1.text = "Time For Fishing\n    Catch " + catchedFish + "/" + totalFish + "fishes";
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
        CenterField.SetActive(false);
        ReturnField.SetActive(true);
        energyField.SetActive(false);
        cargoField.SetActive(false);
    }
}
