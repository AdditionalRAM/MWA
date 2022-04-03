using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum ControlStyle{
    Touch,
    KM,
    Controller
}
public class PlayerMovement : MonoBehaviour, IDamage, IUseSaveGame, IUseOnSave
{
    UIReferences ui;

    public ControlStyle cstyle;
    public PlayerItem altScript;
    public AudioSource hurtSound;
    public GameObject torch;

    public GameObject placeholderGameOver;

    public Animator an;
    Rigidbody2D rb;
    SpriteRenderer sRenderer;

    Vector3 dirInput;

    public Joystick movestick;
    public GameObject movestickIndicator;
    public Color damageColor;
    public UIBar healthBar, armorBar;
    Color normalColor;

    public string currentPlaceName;
    public AudioClip currentBGMusic;
    public Text placeText;

    public float movementSpeed;
    public float maxHealth;
    public float health;
    public float maxArmor, armor;
    public float armorRegenDelay, armorRegenRate, armorRegenTime;
    public bool regenerateArmor = true, armorRegenerating = false;
    public float sunglassesMultiplier, currentRoomLight;
    public bool wearingSunglasses;
    public int fps;

    public bool freeze;
    public bool iFrames;
    public bool takenKB;

    IEnumerator armorRegenCo;

    private void Awake()
    {      
        ui = FindObjectOfType<UIReferences>();
        altScript = GetComponent<PlayerItem>();
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        sRenderer = GetComponent<SpriteRenderer>();
        normalColor = sRenderer.color;
        health = maxHealth;
        UpdateHealth();
        healthBar.SetMaxValue(maxHealth, false);
        iFrames = false;
    }

    public void OnAfterGameLoad()
    {
        transform.position = new Vector3(SaveGame.playerPos[0], SaveGame.playerPos[1], 0);
    }

    public void OnBeforeGameSave()
    {
        SaveGame.playerPos[0] = transform.position.x;
        SaveGame.playerPos[1] = transform.position.y;
        SaveGame.placeName = currentPlaceName;
    }

    private void Update()
    {
        if (!freeze) { GetInput(); }
        else { dirInput = Vector3.zero; iFrames = true; }
        if (fps > 0 && fps <= 240)
        {
            Application.targetFrameRate = fps;
            QualitySettings.vSyncCount = 0;
        }
        else if(fps <= 0)
        {
            QualitySettings.vSyncCount = 1;
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void GetInput()
    {
        /*if(cstyle == ControlStyle.KM)
        {
            dirInput.x = Input.GetAxisRaw("Horizontal");
            dirInput.y = Input.GetAxisRaw("Vertical");
        }else if(cstyle == ControlStyle.Touch)
        {
            dirInput.x = movestick.Horizontal;
            dirInput.y = movestick.Vertical;
        }*/
        dirInput.x = Input.GetAxisRaw("Horizontal");
        dirInput.y = Input.GetAxisRaw("Vertical");
        if (dirInput.magnitude == 0)
        {
            dirInput.x = movestick.Horizontal;
            dirInput.y = movestick.Vertical;
        }
        movestickIndicator.SetActive(!movestick.background.gameObject.activeInHierarchy);
    }

    void Movement()
    {
        rb.velocity = dirInput.normalized * movementSpeed * Time.deltaTime;  
        if (dirInput.x > 0) sRenderer.flipX = false;
        else if (dirInput.x < 0) sRenderer.flipX = true;
        if (dirInput != Vector3.zero) an.SetBool("walking", true);
        else an.SetBool("walking", false);
    }

    public void Damaged(float dmg)
    {
        if (!iFrames)
        {
            hurtSound.Play();
            if(maxArmor > 0)
            {
                if(armor > 0)
                {
                    armor -= dmg;
                    armorBar.an.SetTrigger("shake");
                    if (armor < 0) armor = 0;
                    if (regenerateArmor && !armorRegenerating) {
                        StartArmorRegen();
                    }
                    else if (armorRegenerating)
                    {
                        StartArmorRegen();
                    }
                }
                else
                {
                    healthBar.an.SetTrigger("shake");
                    health -= dmg;
                }
            }
            else
            {
                health -= dmg;
                healthBar.an.SetTrigger("shake");
            }
            UpdateHealth();
            StartCoroutine(DamageTint());
        }    
    }

    public void TakeKB(System.Single time, float thrust, Vector3 otherPos)
    {
        if (!takenKB)
        { 
            Vector3 diff = transform.position - otherPos;
            diff = diff.normalized * thrust;
            Vector2 targetPos = new Vector2(transform.position.x + diff.x, transform.position.y + diff.y);
            rb.DOMove(targetPos, time, false);
            takenKB = false;
        }       
    }

    public void UpdateHealth()
    {
        if (health > maxHealth) health = maxHealth;
        if(maxArmor > 0)
        {
            if (armor > maxArmor) armor = maxArmor;
            armorBar.gameObject.SetActive(true);
            armorBar.SetMaxValue(maxArmor, true);
            armorBar.SetValue(armor);
        }
        else
        {
            armorBar.gameObject.SetActive(false);
            armor = 0;
        }
        healthBar.SetValue(health);
        if (health <= 0)
        {
            freeze = true;
            ui.gameOverManger.GameOver(transform);
            maxArmor = 5000;
            armor = 5000;
        }
    }

    IEnumerator DamageTint()
    {
        iFrames = true;
        sRenderer.color = damageColor;
        yield return new WaitForSecondsRealtime(.2f);
        sRenderer.color = normalColor;
        iFrames = false;
    }

    IEnumerator RegenerateArmor()
    {
        armorRegenerating = true;
        yield return new WaitForSeconds(armorRegenDelay);
        for (int i = 0; i < maxArmor; i++)
        {
            armor += armorRegenRate;
            UpdateHealth();
            if (armor >= maxArmor) break;
            yield return new WaitForSeconds(armorRegenTime);
        }
        armorRegenerating = false;
    }

    public void StartArmorRegen()
    {
        if(armorRegenCo != null) StopCoroutine(armorRegenCo);
        armorRegenCo = RegenerateArmor();
        StopCoroutine(armorRegenCo);
        StartCoroutine(armorRegenCo);
    }

    public void RefreshLighting()
    {
        SetLighting(currentRoomLight, 0.5f);
    }

    public void SetLighting(float lightIntensity, float lightTweenDuration)
    {
        torch.SetActive(lightIntensity < 1);
        currentRoomLight = lightIntensity;
        if (wearingSunglasses)
        {
            lightIntensity *= sunglassesMultiplier;
        }
        if (ui.globalLight.intensity != lightIntensity)
            DOTween.To(() => ui.globalLight.intensity, x => ui.globalLight.intensity = x, lightIntensity, lightTweenDuration);
    }
}
