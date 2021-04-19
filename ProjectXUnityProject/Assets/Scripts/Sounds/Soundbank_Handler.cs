using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundbankHandler
{
    public enum Sounds
    {
        Play_Ambience_Dungeon,
        Play_Bleed,
        Play_Candles,
        Play_Coins,
        Play_Consumables,
        Play_In_Game_Music,
        Play_Main_Menu,
        Play_Main_Menu_Music,
        Play_Movement_NPC,
        Play_Movement_PC,
        Play_NPC_Damage,
        Play_NPC_Death,
        Play_Portal_1,
        Play_UI_Hover,
        Play_UI_Menu_1,
        Play_Weapons,
        Stop_All,
        Stop_Ambience_Dungeon,
        Stop_In_Game_Music,
        Stop_Main_Menu,
        Stop_Main_Menu_Music
    }
    public enum AttackType
    {
        Ability_Attack,
        Basic_Attack,
        Spellbook_Basic,
        Spellbook_Fire,
        Spellbook_Ice
    }
    public enum PotionType
    {
        Potion_Health,
        Potion_Strength
    }
    public static void WeaponAttackType(GameObject gameObject, AttackType attackType)
    {
        AkSoundEngine.SetSwitch("Weapon_Attack_Selector", attackType.ToString(), gameObject);
    }
    public static void WeaponSelector(GameObject gameObject, Weapon.EquipmentType weaponType)
    {
        AkSoundEngine.SetSwitch("Weapon_Selector", weaponType.ToString(), gameObject);
    }
    public static void PotionSelector(GameObject gameObject, PotionType potionType)
    {
        AkSoundEngine.SetSwitch("Consumables", potionType.ToString(), gameObject);
    }
    public static void SoundEvent(Sounds sound, GameObject gameObject)
    {
        AkSoundEngine.PostEvent(sound.ToString(), gameObject);
    }
}