using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerAbilitySystem AbilitySys;
    [SerializeField] private GameObject itemIcons;

    [SerializeField] private Image expirienceFillBar;
    [SerializeField] private Image HpFillBar;


    [SerializeField] private InputActionAsset actions;

    [SerializeField] private Inventory playerStats;

    [SerializeField] private TMP_Text timeText;


    [SerializeField] private GameObject menu;

    private bool isMenuOpen = false;

    private Image[] iconImage;
    private RectTransform[] barImage;
    private TMP_Text[] texts;
    private TMP_Text[] bindTexts;

    public void NewSlotWasAdded() {
        var slotCount = AbilitySys.Slots.Count;

        var bindNumber = 1;

        for(int i = 0; i < slotCount; i++)
        {
            iconImage[i].color = Color.white;
            iconImage[i].sprite = AbilitySys.Slots[i].item.itemSprite;

            if(AbilitySys.Slots[i].item.itemType == ItemType.ManualWeapon)
            {
                var bindKey = actions["Player/Abillity" + bindNumber];

                var displayString = bindKey.GetBindingDisplayString();
                bindTexts[i].text = displayString;
                bindNumber++;
            }
            
        }

    }

    private void Start()
    {

        Debug.Log("setting open menu open menu");

        var openMenu = actions["Player/OpenMenu"];
        openMenu.Enable();
        openMenu.performed += (obj) => OpenMenu_performed();

        StopTime.resetTimer();

        var count = itemIcons.transform.childCount;
        iconImage = new Image[count];
        texts = new TMP_Text[count];
        barImage = new RectTransform[count];
        bindTexts = new TMP_Text[count];
        for (int i = 0; i < count; ++i)
        {
            iconImage[i] = itemIcons.transform.GetChild(i).GetComponent<Image>();
            texts[i] = itemIcons.transform.GetChild(i).GetComponentInChildren<TMP_Text>();
            barImage[i] = itemIcons.transform.GetChild(i).Find("Coldown_bar").GetComponent<RectTransform>();
            bindTexts[i] = itemIcons.transform.GetChild(i).Find("BindBox").GetComponentInChildren<TMP_Text>();
        }

    }

    private void OpenMenu_performed()
    {

        if (isMenuOpen)
        {
            foreach(InputAction action in actions)
            {
                action.Enable();
            }

            GameObject.Find("OptionsPanel").SetActive(false);

            isMenuOpen = false;
            Time.timeScale = 1;
            menu.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            NewSlotWasAdded();

        }
        else
        {
            foreach (InputAction action in actions)
            {
                action.Disable();
            }

            var openMenu = actions["Player/OpenMenu"];
            openMenu.Enable();

            isMenuOpen = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            menu.SetActive(true);

            NewSlotWasAdded();

        }

    }


    // Update is called once per frame
    private void Update()
    {
        var slots = AbilitySys.Slots;
        var slotCount = slots.Count;
        for (int i = 0; i < slotCount; i++)
        { 
            if(slots[i].CooldownTime > 0)
            {
                texts[i].text = slots[i].CooldownTime.ToString("F1");
                var completionPercantage = 1 - Mathf.Clamp01(slots[i].CooldownTime / slots[i].item.cooldown);
                barImage[i].localScale = new Vector3 (1f, slots[i].CooldownTime / playerStats.GetCooldown(slots[i].item), 1f);
                iconImage[i].color = new Color(1, completionPercantage, completionPercantage);
            }
            else
            {
                texts[i].text = "";
            }
        }

        //time
        timeText.text = StopTime.GetTime<string>();


        //hp
        float expirience = 1f - ((float)playerStats.GetExperience() / (float)playerStats.GetExperienceNeeded());
        float hp = 1f - ((float)playerStats.GetHP() / (float)playerStats.GetMaxHP());


        HpFillBar.fillAmount = hp;
        expirienceFillBar.fillAmount = expirience;



    }
}
