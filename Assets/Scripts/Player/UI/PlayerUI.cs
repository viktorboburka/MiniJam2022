using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerAbilitySystem AbilitySys;
    [SerializeField] private GameObject itemIcons;

    [SerializeField] private Image expirienceFillBar;
    [SerializeField] private Image HpFillBar;


    [SerializeField] private Inventory playerStats;

    [SerializeField] private TMP_Text timeText;


    private Image[] iconImage;
    private RectTransform[] barImage;
    private TMP_Text[] texts;

    public void NewSlotWasAdded() {
        var slotCount = AbilitySys.Slots.Count;

        for(int i = 0; i < slotCount; i++)
        {
            iconImage[i].color = Color.white;
            iconImage[i].sprite = AbilitySys.Slots[i].item.itemSprite;
        }

    }

    private void Awake(){
        gameObject.SetActive(true);
    }

    private void Start()
    {
        StopTime.resetTimer();

        var count = itemIcons.transform.childCount;
        iconImage = new Image[count];
        texts = new TMP_Text[count];
        barImage = new RectTransform[count];
        for (int i = 0; i < count; ++i)
        {
            iconImage[i] = itemIcons.transform.GetChild(i).GetComponent<Image>();
            texts[i] = itemIcons.transform.GetChild(i).GetComponentInChildren<TMP_Text>();
            barImage[i] = itemIcons.transform.GetChild(i).Find("Coldown_bar").GetComponent<RectTransform>();
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
                barImage[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100 * (1-completionPercantage));
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
