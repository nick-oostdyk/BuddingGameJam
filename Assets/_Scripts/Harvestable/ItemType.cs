﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum ItemType
{
	// resources
	STONE,
	BOULDER,
	METAL_SCRAP,
	METAL,

	STICK,
	WOOD,
	PLANK,

	FIBER,
	ROPE,

	// tools
	BASIC_MULTI_TOOL,
	ADVANCED_MULTI_TOOL,
	FISHING_ROD, // i made the fishing minigame :D

	// structures
	FIREPIT,
	CRAFTING_STATION,
	BED,
	FURNACE,
	WATER_PURIFIER,
	FISH_DISPLAY,

	//
	NUM_ITEMS
}

// hehe look at my fishing minigame :DD
public enum FishType
{
	BLUE_GUPPY,
	SILVER_GUPPY,
	GOLDEN_GUPPY,

	BLACK_TETRA,
	NEON_TETRA,
	GOLDEN_TETRA,

	CATFISH,
	BLUE_CATFISH,
	GOLDEN_CATFISH,

	BLUE_ANGELFISH,
	QUEEN_ANGELFISH,
	GOLDEN_ANGELFISH,

	PUFFERFISH,
	GOLDEN_PUFFERFISH,

	EEL,
	GOLDEN_EEL,

	OCTOPUS,
	GOLDEN_OCTOPUS,
}

public class ItemPool
{
	public static Dictionary<ItemType, ItemObject> ItemDict;

	public static void Init()
	{
		ItemObject[] itemArr = Resources.LoadAll<ItemObject>("ScriptableObjects/Items");

		ItemDict = new Dictionary<ItemType, ItemObject>();
		foreach (var item in itemArr)
			ItemDict[item.Type] = item;
	}
}