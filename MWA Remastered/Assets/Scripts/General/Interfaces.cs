using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IUsableItem{
    void OnUse();
}

public interface IDamage
{
    void Damaged(float dmg);
    void TakeKB(System.Single kbtime, float thrust, Vector3 otherPos);
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
