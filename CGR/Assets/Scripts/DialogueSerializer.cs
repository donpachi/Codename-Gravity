using System.Xml.Serialization;
using System.Xml;
using System.Collections;


public class DialogueSerializer
{
    private string path = @"Resources";
    private string pathToResources;
    private DialogueXML dialogueData;
    public DialogueSerializer()
    {                   
        pathToResources = System.IO.Path.GetPathRoot(path);
        dialogueData = new DialogueXML();
    }

    public void SerializeDialogueXML()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DialogueXML));
        System.IO.StreamWriter streamWriter = System.IO.File.CreateText(pathToResources + "/data.xml");
        serializer.Serialize(streamWriter, dialogueData);
        streamWriter.Close();
        streamWriter.Dispose();
    }

    public void DeserializeDialogueXML()
    {
        System.IO.FileStream filestream;
        XmlReader reader;
        XmlSerializer serializer = new XmlSerializer(typeof(DialogueXML));

        if (!System.IO.File.Exists(pathToResources + "/data.xml"))
        {
            filestream = System.IO.File.Create(pathToResources + "/dialogue.xml");
            filestream.Close();
            filestream.Dispose();
            SerializeDialogueXML();
        }
        else
        {
            filestream = new System.IO.FileStream(pathToResources + "/dialogue.xml", System.IO.FileMode.Open);
            reader = new XmlTextReader(filestream);
            try
            {
                if (serializer.CanDeserialize(reader))
                {
                    dialogueData = serializer.Deserialize(reader) as DialogueXML;
                }
            }
            finally
            {
                reader.Close();
                filestream.Close();
                filestream.Dispose();
            }
        }
    }
}