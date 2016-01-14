using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Dialogue")]
public class DialogueXML
{
    [XmlAttribute]
    public int ID
    {
        get;
        private set;
    }

    public string speaker
    {
        get;
        set;
    }

    public string speech
    {
        get;
        set;
    }

    public DialogueXML()
    {

    }

    public DialogueXML(int id)
    {
        ID = id;
    }
}
