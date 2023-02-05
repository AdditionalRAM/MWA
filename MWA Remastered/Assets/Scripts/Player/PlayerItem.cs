using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    public PlayerMovement altScript;
    public AudioSource pickupSound, healSound;

    public Joystick aimStick;
    public Vector3 aimInput;

    public GameObject crosshair, aimStickIndicator;
    public Transform itemParent;
    public Item selectedItem;
    public ArmorObject equippedArmor, sunglasses;

    public InventoryObject defInventory, consumeInventory, equipInventory, keyInventory, armorInventory;

    public GameObject acquirePrefab;
    public Transform canvas;
    GameObject currentAc;

    public ItemObject healthPot;
    public bool quickHealUsable;
    public int quickHealCooldown;

    public float quickHealTimer;
    public ItemObject emptyEquipment;

    private void Awake()
    {
        altScript = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        GetInput();
        if (selectedItem != null)
        {
            if (selectedItem.myItem != emptyEquipment)
            {
                MoveCrosshair();
                if (!selectedItem.useDuringAnim && !selectedItem.animating)
                {
                    RotateItem();
                }
                else if (selectedItem.useDuringAnim)
                {
                    RotateItem();
                }
                selectedItem.owner = gameObject;
                selectedItem.crosshair = crosshair.transform;
            }
        }
        if(!quickHealUsable) UpdateQuickHealTimer();
        if (consumeInventory.HasItem(healthPot, 1))
        {
            UIReferences.instance.quickHealButton.SetActive(true);
            if (quickHealUsable)
            {
                UIReferences.instance.quickHealButton.GetComponent<Button>().interactable = true;
                UIReferences.instance.quickHealCountdown.gameObject.SetActive(false);
                UIReferences.instance.quickHealImage.gameObject.SetActive(true);
                UIReferences.instance.quickHealItemCount.text = 
                    consumeInventory.CountItem(healthPot).ToString();
            }
            else
            {
                UIReferences.instance.quickHealCountdown.gameObject.SetActive(true);
                UIReferences.instance.quickHealImage.gameObject.SetActive(false);
                UIReferences.instance.quickHealButton.GetComponent<Button>().interactable = false;             
            }
        }
        else
        {
            UIReferences.instance.quickHealButton.SetActive(false);
        }
    }

    public void UpdateQuickHealTimer()
    {
        if(quickHealTimer > 0)
        {
            quickHealTimer -= Time.deltaTime;
        }
        else
        {
            quickHealUsable = true;
            quickHealTimer = quickHealCooldown;
        }
        UIReferences.instance.quickHealCountdown.text = ((int)quickHealTimer).ToString();
    }

    public void UseQuickHeal()
    {
        if (quickHealUsable && consumeInventory.HasItem(healthPot, 1) && Healable())
        {
            Heal(healthPot.consumableItem.restoredHealth);
            consumeInventory.RemoveItem(healthPot, 1);
            healSound.Play();
            quickHealUsable = false;
        }
    }

    public void MoveCrosshair()
    {
        Vector3 aim = aimInput;
        if (aim.magnitude > 0.1f)
        {
            crosshair.SetActive(true);
            aim.Normalize();
            aim *= selectedItem.range;
            crosshair.transform.localPosition = aim;
        }
        else
        {
            crosshair.SetActive(false);
        }
    }

    public void GetInput()
    {
        if(aimStick != null)
        {
            aimInput.x = aimStick.Horizontal;
            aimInput.y = aimStick.Vertical;
            aimStickIndicator.SetActive(!aimStick.background.gameObject.activeInHierarchy);
        }
        else
        {
            aimInput = Vector3.zero;
        }
        if(selectedItem == null || selectedItem.myItem == emptyEquipment)
        {
            aimStick.gameObject.SetActive(false);
            aimStickIndicator.SetActive(false);
        }
        else
        {
            aimStick.background.parent.gameObject.SetActive(true);
        }
    }

    public void RotateItem()
    {
        if (crosshair != null && selectedItem != null)
        {
            Vector3 dir = crosshair.transform.position - selectedItem.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            selectedItem.transform.rotation = Quaternion.AngleAxis(angle + selectedItem.rotateOffset, selectedItem.transform.forward);
            SpriteRenderer susRenderer;
            if (selectedItem.GetComponent<SpriteRenderer>() != null) susRenderer = selectedItem.GetComponent<SpriteRenderer>();
            else susRenderer = selectedItem.GetComponentInChildren<SpriteRenderer>();
            if (angle < 90 && angle > -90 && selectedItem.flipSprite)
            {
                susRenderer.flipX = false;
            }
            else if(selectedItem.flipSprite)
            {
                susRenderer.flipX = true;
            }
            if (selectedItem.GetComponent<IOnRotateItem>() != null && aimInput.magnitude > 0.1)
                selectedItem.GetComponent<IOnRotateItem>().OnRotate();
        }
    }

    public void UseItem()
    {
        if (selectedItem != null && !altScript.freeze)
        {
            if (!selectedItem.useDuringAnim && !selectedItem.animating)
            {
                selectedItem.Use();
            }
            else if (selectedItem.useDuringAnim)
            {
                selectedItem.Use();
            }
            if(GetComponent<TutorialMessages>() != null)
            {
                GetComponent<TutorialMessages>().PlayerAttacked();
            }
        }  
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        /*if (other.gameObject.GetComponent<DroppedItem>() != null)
        {
            DroppedItem item = other.gameObject.GetComponent<DroppedItem>();
            if (!other.collider.isTrigger && !other.otherCollider.isTrigger && !item.obtained)
            {
                item.obtained = true;
                if(item.item.type == ItemType.Default)
                {
                    defInventory.AddItem(item.item, item.itemAmount);
                }
                else if(item.item.type == ItemType.Consumable)
                {
                    consumeInventory.AddItem(item.item, item.itemAmount);
                }
                else if (item.item.type == ItemType.Equipment)
                {
                    equipInventory.AddItem(item.item, item.itemAmount);
                }
                else if (item.item.type == ItemType.Key)
                {
                    keyInventory.AddItem(item.item, item.itemAmount);
                }else if (item.item.type == ItemType.Armor)
                {
                    armorInventory.AddItem(item.item, item.itemAmount);
                }
                pickupSound.Play();
                StartCoroutine(AnimateAcquiring(item.item, item.itemAmount, item.itemName));
                /*if(item.item.type == ItemType.Default)
                {
                    if (defInventory.container.Count < defInventory.limit) defInventory.AddItem(item.item, item.itemAmount);
                    else return;
                }
                else if(item.item.type == ItemType.Consumable)
                {
                    if (consumeInventory.container.Count < consumeInventory.limit) consumeInventory.AddItem(item.item, item.itemAmount);
                    else return;
                }
                else if (item.item.type == ItemType.Equipment)
                {
                    if (equipInventory.container.Count < equipInventory.limit) equipInventory.AddItem(item.item, item.itemAmount);
                    else return;
                }
                else if (item.item.type == ItemType.Key)
                {
                    if (keyInventory.container.Count < keyInventory.limit) keyInventory.AddItem(item.item, item.itemAmount);
                    else return;
                }
                Destroy(item.gameObject);
            }
        }*/
        if(other.gameObject.GetComponent<IDamage>() != null)
        {
            other.gameObject.GetComponent<IDamage>().TakeKB(.2f, 1.2f, transform.position);
        }
    }

    public void GrabItem(ItemObject item, int itemAmount, string itemName)
    {
        if (item.type == ItemType.Default)
        {
            defInventory.AddItem(item, itemAmount);
        }
        else if (item.type == ItemType.Consumable)
        {
            consumeInventory.AddItem(item, itemAmount);
        }
        else if (item.type == ItemType.Equipment)
        {
            equipInventory.AddItem(item, itemAmount);
        }
        else if (item.type == ItemType.Key)
        {
            keyInventory.AddItem(item, itemAmount);
        }
        else if (item.type == ItemType.Armor)
        {
            armorInventory.AddItem(item, itemAmount);
        }
        pickupSound.Play();
        StartCoroutine(AnimateAcquiring(item, itemAmount, itemName));
        var ss = FindObjectsOfType<MonoBehaviour>().OfType<IOnPlayerGetItem>();
        foreach (IOnPlayerGetItem s in ss)
        {
            s.OnPlayerGetItem(item, itemAmount);
        }
    }

    IEnumerator AnimateAcquiring(ItemObject item, int amount, string nameText)
    {
        if(currentAc != null)
        {
            GameObject objToDestroy = currentAc;
            currentAc = null;
            objToDestroy.GetComponent<Animator>().SetBool("visible", false);
            Destroy(objToDestroy, .5f);
        }
        currentAc = Instantiate(acquirePrefab, canvas);
        Transform objParent = currentAc.GetComponent<AcquireObject>().itemParent;
        Text itemText = currentAc.GetComponent<AcquireObject>().itemText;
        GameObject currentItemPrefab = Instantiate(item.prefab, objParent);
        currentItemPrefab.transform.localPosition = Vector3.zero;
        Destroy(currentItemPrefab.GetComponentInChildren<Text>().gameObject);
        if(amount > 1)
        {
            itemText.text = amount.ToString() + "x " + nameText;
        }
        else
        {
            itemText.text = nameText;
        }
        currentAc.GetComponent<Animator>().SetBool("visible", true);
        yield return new WaitForSeconds(2f);
        GameObject pooToDestroy = currentAc;
        currentAc = null;
        if(pooToDestroy != null)
        pooToDestroy.GetComponent<Animator>().SetBool("visible", false);
        Destroy(pooToDestroy, .5f);
    }
    public bool Healable()
    {
        if(altScript.health < altScript.maxHealth)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Heal(float amount)
    {
        if (Healable())
        {
            altScript.health += amount;
            altScript.UpdateHealth();
        }
    }

    public void EquipItem(GameObject itemToEquip)
    {
        selectedItem = Instantiate(itemToEquip, itemParent).GetComponent<Item>();
        var ss = FindObjectsOfType<MonoBehaviour>().OfType<IOnPlayerGetItem>();
        foreach (IOnPlayerEquipItem s in ss)
        {
            s.OnPlayerEquipItem(selectedItem.myItem);
        }
    }

    public void UnequipItem()
    {
        GameObject destroyItem = selectedItem.gameObject;
        selectedItem = null;
        Destroy(destroyItem);
    }

    public void EquipArmor(ArmorObject armor)
    {
        if (armor == sunglasses)
        {
            altScript.wearingSunglasses = true;
            altScript.RefreshLighting();
        }
        else if(equippedArmor == sunglasses)
        {
            altScript.wearingSunglasses = false;
            altScript.RefreshLighting();
        }
        equippedArmor = armor;
        altScript.an.SetInteger("armor", armor.armorID);
        //do protection stuff
        altScript.maxArmor = armor.extraHealth;
        altScript.StartArmorRegen();
        altScript.UpdateHealth();    
    }

    private void OnDestroy()
    {
        defInventory.container.Clear();
        consumeInventory.container.Clear();
        equipInventory.container.Clear();
        keyInventory.container.Clear();
        armorInventory.container.Clear();
    }
}
