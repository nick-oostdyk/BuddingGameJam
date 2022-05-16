using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveDialogueTriggerScript : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (GameManager.Instance.GameFlags.HasFlag(GameFlag.CAVE_PROMPT)) return;
		GameManager.Instance.AddGameFlag(GameFlag.CAVE_PROMPT);

		DialogueBoxManager.Instance.PushSequence("", new DialogueBoxManager.TextSequence(new string[] {
			"Looks like some sort of cave!",
			"I wonder what it's like inside...",
			"I should check it out."
			}));
	}
}
