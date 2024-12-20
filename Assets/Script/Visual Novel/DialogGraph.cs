using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace SR
{
    public class DialogGraphController : MonoBehaviour
    {
        public DialogueGraph dialogData;
        public UnityEvent onEndedDialog;
    }
}

[System.Serializable]
public class DialogueGraph
{
    public DialogueNode startingNode;

    private List<DialogueNode> nodes;

    public List<DialogueNode> GetAllNodes()
    {
        return nodes;
    }

    public DialogueGraph()
    {
        nodes = new List<DialogueNode>();
    }

    public DialogueNode CreateNode(List<string> text)
    {
        var newNode = new DialogueNode(text);
        nodes.Add(newNode);
        return newNode;
    }

    public void AddChoice(DialogueNode fromNode, string choiceText, DialogueNode toNode)
    {
        var choice = new Choice
        {
            choiceText = choiceText,
            nextNode = toNode
        };

        fromNode.choices.Add(choice);
    }
}

[System.Serializable]
public class DialogueNode
{
    private DialogType type;
    public List<string> dialogueText;
    public List<Choice> choices;

    public DialogueNode(List<string> text)
    {
        type = DialogType.None;
        dialogueText = text;
        choices = new List<Choice>();
    }
}

[System.Serializable]
public class Choice
{
    public string choiceText;
    public DialogueNode nextNode;
}

public enum DialogType
{
    None,
    UnityEvent,
    MoodEffect,
    Custom
}
