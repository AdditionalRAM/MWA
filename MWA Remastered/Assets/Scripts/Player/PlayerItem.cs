using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    public PlayerMovement altScript;
    public AudioSource pickupSound;

    public Joystick aimStick;
    public Vector3 aimInput;

    public GameObject crosshair, aimStickIndicator;
    public Transform itemParent;
    public Item selectedItem;

    public InventoryObject defInventory, consumeInventory, equipInventory, keyInventory;

    public GameObject acquirePrefab;
    public Transform canvas;
    GameObject currentAc;

    private void Awake()
    {
        altScript = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        GetInput();
        if (selectedItem != null)
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
            selectedItem.player = this;
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
        }  
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<DroppedItem>() != null)
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
                }*/
                Destroy(item.gameObject);
            }
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
    }

    public void UnequipItem()
    {
        GameObject destroyItem = selectedItem.gameObject;
        selectedItem = null;
        Destroy(destroyItem);
    }

    private void OnDestroy()
    {
        defInventory.container.Clear();
        consumeInventory.container.Clear();
        equipInventory.container.Clear();
        keyInventory.container.Clear();
    }
}
