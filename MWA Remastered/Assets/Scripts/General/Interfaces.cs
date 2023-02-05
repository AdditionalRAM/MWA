using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IUsableItem{
    void OnUse();
}

public interface IOnRotateItem
{
    void OnRotate();
}

public interface IDamage
{
    void Damaged(float dmg);
    void TakeKB(System.Single kbtime, float thrust, Vector3 otherPos);
}

public interface ISelectiveDamage
{
    void Damaged(float dmg, ItemObject item);
}

public interface IUseSaveGame
{
    void OnAfterGameLoad();
}

public interface IUseOnSave
{
    void OnBeforeGameSave();
}

public interface ILocalization
{
    void OnLocalize();
}

public interface IOnPlayerGetItem
{
    void OnPlayerGetItem(ItemObject item, int itemCount);
}

public interface IOnPlayerEquipItem
{
    void OnPlayerEquipItem(ItemObject item);
}

