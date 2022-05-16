using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueBoxManager : MonoBehaviour
{
	// singleton -----
	private static DialogueBoxManager _i;
	public static DialogueBoxManager Instance => _i;
	public void Awake()
	{
		if (_i == null) _i = this;
		else Destroy(this);
	}
	// -----

	private enum TextArea : short
	{
		HEAD,
		BODY,
	}

	public class TextSequence
	{
		public string[] Sequence;
		public int Index;

		public TextSequence(string[] sequence)
		{
			Sequence = sequence;
			Index = 0;
		}
	}

	[Header("Continue / Close input")]
	[SerializeField] private InputAction _action;
	[SerializeField] private GameObject _textboxPanel;

	private TextMeshProUGUI[] _text;

	private Queue<(string, TextSequence)> _textQueue;
	private TextSequence _currentSequence => _textQueue.Peek().Item2;

	public System.Action OnCurrentSequenceFinish;

	private void Start()
	{
		_text = _textboxPanel.GetComponentsInChildren<TextMeshProUGUI>();
		_textboxPanel.SetActive(false);

		_textQueue = new Queue<(string, TextSequence)>();
		//_pushExampleDialogue();
	}

	private void Update()
	{
		if (_action.WasPerformedThisFrame())
			_advanceOrEnd();
	}

	private void _advanceOrEnd()
	{
		if (!_textboxPanel.activeInHierarchy) return;

		_currentSequence.Index++;
		// if current sequence is finished
		if (_currentSequence.Index == _currentSequence.Sequence.Length)
		{
			_textQueue.Dequeue();
			_beginNewSequence();
		}

		// continue current sequence
		else
			_setText(_currentSequence.Sequence[_currentSequence.Index]);
	}

	public void PushText(string head, string text) => PushSequence(head, new TextSequence(new string[] { text }));
	public void PushText(string text) => PushSequence("", new TextSequence(new string[] { text }));
	public void PushSequence(TextSequence sequence) => PushSequence("", sequence);
	public void PushSequence(string head, TextSequence sequence)
	{
		_textQueue.Enqueue((head, sequence));
		_pushLive();
	}

	private void _pushLive()
	{
		if (_textboxPanel.activeInHierarchy) return;

		_textboxPanel.SetActive(true);
		_beginNewSequence();
	}

	private void _beginNewSequence()
	{
		// if there's no more text to display
		if (_textQueue.Count == 0)
		{
			_invokeSequenceFinished();
			_textboxPanel.SetActive(false);
			return;
		}

		var (head, sequence) = _textQueue.Peek();
		_setText(head, sequence.Sequence[sequence.Index]);
	}

	private void _setText(string head, string body)
	{
		_text[(short)TextArea.HEAD].text = head;
		_text[(short)TextArea.BODY].text = body;
	}

	private void _setText(string body)
	{
		_text[(short)TextArea.BODY].text = body;
	}

	private void _invokeSequenceFinished()
	{
		OnCurrentSequenceFinish?.Invoke();

		var delegates = OnCurrentSequenceFinish?.GetInvocationList();
		if (delegates == null) return;
		foreach (var action in delegates)
			OnCurrentSequenceFinish -= (action as System.Action);
	}

	private void _pushExampleDialogue()
	{
		DialogueBoxManager.Instance.PushText("Cass", "You can do it! Let's get it done, together!");
		DialogueBoxManager.Instance.PushSequence("Nick", new TextSequence(new string[] {
			"Man.",
			"What are you talking about.",
			"I'm all for the positivity, but let's just focus on the task."
			}));
		DialogueBoxManager.Instance.PushText("Cass", "-m-");
	}

	private void OnEnable() => _action.Enable();
	private void OnDisable() => _action.Disable();
}
