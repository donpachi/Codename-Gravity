using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

//container for raw speech data as well as speaker name for each section of dialogue
[Serializable]
public class Dialogue
{
    [XmlElement("Speaker")]
    public string speaker
    {
        get;
        set;
    }
    [XmlElement("Speech")]
    public string speech
    {
        get;
        set;
    }
}
//this is a sub container that contains all the given speech for a given node in a level
[Serializable]
public class DialogueNode
{
    [XmlAttribute("index")]
    public int nodeIndex { get; set; }

    [XmlArray("Dialogues")]
    [XmlArrayItem("Dialogue", typeof(Dialogue))]
    public Dialogue[] dialogueArray { get; set; }
}

//this is a container that holds all the blocks of dialogue for a given level
[Serializable]
public class Level
{
    [XmlAttribute("levelName")]
    public string levelname
    {
        get;
        set;
    }
    [XmlArray("Level")]
    [XmlArrayItem("Dialogues", typeof(DialogueNode))]
    public DialogueNode[] levelDialogueNodes { get; set; }
}

//this is the base array element that encompasses each of the levels in an array
[Serializable, XmlRoot("LevelCollection")]
public class LevelCollection
{
    [XmlArray("LevelCollection")]
    [XmlArrayItem("Levels", typeof(Level))]
    public Level[] levelCollection { get; set; }
}