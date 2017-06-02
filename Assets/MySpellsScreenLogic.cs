using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class MySpellsScreenLogic : MonoBehaviour {
    public RectTransform SpellButtonTemplate, CastAmountPanelRef;
    public Text DropdownTemplate;
    Text activeDropdown;
    Transform activeButtons;
    List<Transform> uiList = new List<Transform>();

    void OnEnable()
    {
        DrawSpellButtons(PlayerManager.CurrentPlayer.SpellList);
    }

    void DrawSpellButtons(List<Spell> spells)
    {
        ClearUi();

        SpellButtonTemplate.gameObject.SetActive(true);
        DropdownTemplate.gameObject.SetActive(true);

        for (int i = 0; i < spells.Count; i++)
        {
            var newButton = (RectTransform)Instantiate(SpellButtonTemplate, SpellButtonTemplate.position, SpellButtonTemplate.rotation, SpellButtonTemplate.parent);
            newButton.GetComponentInChildren<Text>().text = spells[i].Name + " | " + spells[i].School + " | " + spells[i].RollAmount;
            newButton.SetAsLastSibling();

            var spell = spells[i];
            newButton.Find("Button").GetComponentInChildren<Button>().onClick.AddListener(delegate () { RemoveSpellFromPlayer(spell); });

            var dropdown = (Text)Instantiate(DropdownTemplate, DropdownTemplate.transform.position, DropdownTemplate.transform.rotation, DropdownTemplate.transform.parent);
            dropdown.text = spells[i].Description;
            dropdown.transform.SetAsLastSibling();
            dropdown.gameObject.SetActive(false);

            var castPanel = (RectTransform)Instantiate(CastAmountPanelRef, CastAmountPanelRef.position, CastAmountPanelRef.rotation, CastAmountPanelRef.parent);

            castPanel.Find("Text").GetComponent<Text>().text = spell.RollAmount + "";
            castPanel.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate () { DecrementSpellCast(spell); });
            castPanel.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate () { IncrementSpellCast(spell); });
            castPanel.gameObject.SetActive(false);
            castPanel.SetAsLastSibling();

            newButton.GetComponent<Button>().onClick.AddListener(delegate () { ToggleText(dropdown, castPanel); });

            uiList.Add(newButton.transform);
            uiList.Add(dropdown.transform);
            uiList.Add(castPanel.transform);
        }
        SpellButtonTemplate.gameObject.SetActive(false);
        DropdownTemplate.gameObject.SetActive(false);
        CastAmountPanelRef.gameObject.SetActive(false);
    }

    private void IncrementSpellCast(Spell spell)
    {
        spell.RollAmount++;
        PlayerManager.SavePlayer();
        DrawSpellButtons(PlayerManager.CurrentPlayer.SpellList);
    }
    private void DecrementSpellCast(Spell spell)
    {
        spell.RollAmount--;
        PlayerManager.SavePlayer();
        DrawSpellButtons(PlayerManager.CurrentPlayer.SpellList);
    }

    private void RemoveSpellFromPlayer(Spell spell)
    {
        foreach (var playerSpell in PlayerManager.CurrentPlayer.SpellList)
        {
            if(playerSpell.Name == spell.Name)
            {
                PlayerManager.CurrentPlayer.SpellList.Remove(playerSpell);
                break;
            }
        }

        DrawSpellButtons(PlayerManager.CurrentPlayer.SpellList);
    }

    void ClearUi()
    {
        foreach (var ui in uiList)
        {
            Destroy(ui.gameObject);
        }

        uiList.Clear();
    }

    void ToggleText(Text text, Transform castPanel)
    {
        text.gameObject.SetActive(!text.gameObject.activeSelf);
        castPanel.gameObject.SetActive(text.gameObject.activeSelf);

        if (activeDropdown != text && text.gameObject.activeSelf)
        {
            if (activeDropdown)
            {
                activeDropdown.gameObject.SetActive(false);
                activeButtons.gameObject.SetActive(false);
            }
            activeDropdown = text;
            activeButtons = castPanel;
        }
    }
}
