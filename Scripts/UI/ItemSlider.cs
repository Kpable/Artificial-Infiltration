using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlider : MonoBehaviour {
    
    public List<string> items;

    public PlayerCustomizationMenuController parentMenu;

    private Button leftArrow, rightArrow;
    private Text itemText;

    private string textToSet = "";

	// Use this for initialization
	void Awake () {
        // A bunch of error reporting while getting the right components
        Transform leftArrowTransform = transform.Find("Left Arrow");
        if (leftArrowTransform)
        {
            leftArrow = transform.Find("Left Arrow").gameObject.GetComponent<Button>();
            if (!leftArrow) Debug.Log(name + ": Unable to find Button Object in Left Arrow child");
        }
        else Debug.LogError(name + ": Unable to find Left Arrow child");

        Transform rightArrowTransform = transform.Find("Left Arrow");
        if (rightArrowTransform)
        {
            rightArrow = transform.Find("Right Arrow").gameObject.GetComponent<Button>();
            if (!rightArrow) Debug.Log(name + ": Unable to find Button Object in Right Arrow child");
        }
        else Debug.LogError(name + ": Unable to find Right Arrow child");

        Transform itemTransform = transform.Find("Item");
        if(itemTransform)
        {
            itemText = transform.Find("Item").gameObject.GetComponent<Text>();
            if(itemText) Debug.Log(name + ": Unable to find Text Object in Item child");
        }

        leftArrow.onClick.AddListener(delegate { CycleLeft(); });
        rightArrow.onClick.AddListener(delegate { CycleRight(); });

        itemText.text = textToSet;
    }

    public void CycleLeft()
    {
        // Only cycle if theres more than one
        if(items.Count > 1)
        {
            // If we've reached left most wrap back to end. 
            if(items.IndexOf(itemText.text) == 0)
            {
                itemText.text = items[items.Count - 1];
            }
            // else go back one
            else
            {
                itemText.text = items[items.IndexOf(itemText.text) - 1];
            }
        }

        if (parentMenu) parentMenu.SliderValueChanged();
    }

    public void CycleRight()
    {
        // Only cycle if theres more than one
        if (items.Count > 1)
        {
            // If we've reached right most, wrap back to the beginning
            if (items.IndexOf(itemText.text) == items.Count - 1)
            {
                itemText.text = items[0];
            }
            //else go forward one
            else
            {
                itemText.text = items[items.IndexOf(itemText.text) + 1];
            }
        }

        if (parentMenu) parentMenu.SliderValueChanged();

    }

    public void AddItem(string item)
    {
        // Add the item to our list
        items.Add(item);
    }

    public string GetText()
    {
        if(itemText)
            return itemText.text;
        return "";
    }

    public void SetText(string text)
    {
        if (itemText)
            itemText.text = text;
        else
            textToSet = text;
    }

}
