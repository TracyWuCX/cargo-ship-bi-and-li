using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OceanFloorUI : MonoBehaviour
{
    public GameObject CenterField;

    [Header("=== Information Settings ===")]
    public GameObject infoField;
    public Text EnergyWarning;

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

    [Header("=== Finish Settings ===")]
    public GameObject finishField;
    public Text countDownText;
    public string sceneName;
    private int countDown = 3;

    // Start is called before the first frame update
    private void Start()
    {
        infoField.SetActive(true);
        EnergyWarning.enabled = false;

        taskField.SetActive(true);

        totalFish = fishParent.GetComponent<Spawn>().currentAmount;

        energyPersentage = 1f;
        energyField.SetActive(true);

        cargoField.SetActive(true);

        finishField.SetActive(false);
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
            EnergyWarning.enabled = true;
            EnergyWarning.text = energyPersentage * 100 + "% Energy Remaining";
        }
        else
        {
            EnergyWarning.enabled = false;
        }
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
        energyField.SetActive(false);
        cargoField.SetActive(false);
        finishField.SetActive(true);
        countDownText.text = "in " + countDown + " seconds";
        Invoke(nameof(HandleScene), 3f);
    }

    private void HandleScene()
    {
        //LoadScene.Instance.Loading(sceneName);
    }

}
