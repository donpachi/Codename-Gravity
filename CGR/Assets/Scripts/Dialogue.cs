using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

//container for raw speech data as well as speaker name for each section of dialogue
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
public class Dialogues
{
    [XmlArray("Dialogues")]
    [XmlArrayItem("Dialogue", typeof(Dialogue))]
    public Dialogue[] Dialogue { get; set; }
}

//this is a container that holds all the blocks of dialogue for a given level
public class Level
{
    [XmlArray("Level")]
    [XmlArrayItem("Dialogues", typeof(Dialogues))]
    public Dialogues[] Dialogues { get; set; }
}

//this is the base array element that encompasses each of the levels in an array
[XmlRoot("LevelDialogueCollection")]
public class LevelCollection
{
    [XmlArray("LevelCollection")]
    [XmlArrayItem("Levels", typeof(Level))]
    public Level[] Level { get; set; }
}