using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storyteller : MonoBehaviour
{
    public bool localize, localizeD;
    UIReferences ui;
    public Text storyText;
    public GameObject[] storyImages;
    [TextArea(3, 10)]
    public string[] storyTexts;
    int currentDialog, currentImg;
    public float textDelay, storyDelay;
    public AudioSource dialogSound;
    public bool makeSound;
    private void Awake()
    {
        ui = FindObjectOfType<UIReferences>();
    }

    public void TellStory()
    {
        if (localize && !localizeD) Localize();
        currentDialog = 0;
        currentImg = 0;
        SetImage(currentImg);
        StartCoroutine(Dialog(storyTexts[currentDialog]));
    }

    void UpdateStory()
    {
        currentDialog++;
        if(storyTexts[currentDialog] == "<imgupdate>")
        {
            currentImg++; SetImage(currentImg); currentDialog++;
        }else if(storyTexts[currentDialog] == "<endstory>")
        {
            EndStory();
            return;
        }
        StartCoroutine(Dialog(storyTexts[currentDialog]));
    }

    public void EndStory()
    {
        ui.interactButton.pressed = true;
        gameObject.SetActive(false);
    }

    void SetImage(int i)
    {
        foreach(GameObject img in storyImages)
        {
            img.SetActive(false);
        }
        storyImages[i].SetActive(true);
    }

    IEnumerator Dialog(string textToTell)
    {
        storyText.text = "";
        foreach (char character in textToTell.ToCharArray())
        {
            if (makeSound && character != ' ')
            {
                dialogSound.Play();
            }
            yield return new WaitForSeconds(textDelay);
            storyText.text += character;
        }
        Invoke("UpdateStory", storyDelay);
    }

    public void Localize()
    {
        for (int i = 0; i < storyTexts.Length; i++)
        {
            if (storyTexts[i] != "<imgupdate>" && storyTexts[i] != "<endstory>")
            {
                storyTexts[i] = LocalManager.Localize(storyTexts[i]);
            }
        }
        localizeD = true;
    }
}
