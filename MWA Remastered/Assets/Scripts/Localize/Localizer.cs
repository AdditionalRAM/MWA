using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Localizer : MonoBehaviour
{
    public Transform canvas;
    public GameObject langMenu, backButton, popup;
    public int currentLang, tempLang;
    public string langsDir;
    public string[] langFileNames;
    public TextAsset[] langTexts;
    public float tweenDur;
    [TextArea(3, 30)]
    public List<string> langs = new List<string>();

    private void Awake()
    {
        currentLang = PlayerPrefs.GetInt("language", 0);
        if (currentLang == 0)
        {
            StartLangMenu(false);
        }
        else GetReadyToLocalize(currentLang, false);
    }

    public void StartLangMenu(bool wBackButton)
    {
        backButton.SetActive(wBackButton);
        langMenu.transform.localScale = Vector3.zero;
        langMenu.transform.DOScale(Vector3.one, tweenDur).SetUpdate(true);
    }

    public void CloseLangMenu()
    {
        langMenu.transform.DOScale(Vector3.zero, tweenDur).SetUpdate(true);
    }

    public void StartPopup(int langToLoad)
    {
        tempLang = langToLoad;
        popup.transform.DOScale(1.5f, tweenDur).SetUpdate(true);
    }

    public void PopupConfirm()
    {
        GetReadyToLocalize(tempLang, true);
    }

    public void ClosePopup()
    {
        popup.transform.DOScale(0, tweenDur).SetUpdate(true);
    }

    public void NoPopupLocalize(int langToLoad)
    {
        GetReadyToLocalize(langToLoad, false);
    }

    public void GetReadyToLocalize(int langToLoad, bool reload)
    {
        Time.timeScale = 1f;
        CloseLangMenu();
        currentLang = langToLoad;
        PlayerPrefs.SetInt("language", currentLang);
        /*for (int i = 0; i < langFileNames.Length; i++)
        {
            string nowPath = Application.persistentDataPath + langsDir + langFileNames[i];
            if (!File.Exists(nowPath))
            {
                if (!Directory.Exists(Application.persistentDataPath + langsDir)) Directory.CreateDirectory(Application.persistentDataPath + langsDir);

            }

            StreamReader reader = new StreamReader(nowPath);
            langs.Add(reader.ReadToEnd());
            reader.Close();
        }*/
        foreach (TextAsset leng in langTexts)
        {
            langs.Add(leng.text);
        }
        LocalManager.LoadKeys(langs[0]);
        LocalManager.LoadLanguage(langs[currentLang]);
        LocalManager.WriteToDic();
        var ss = FindObjectsOfType<MonoBehaviour>().OfType<ILocalization>();
        foreach (ILocalization s in ss)
        {
            s.OnLocalize();
        }
        if (reload) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
