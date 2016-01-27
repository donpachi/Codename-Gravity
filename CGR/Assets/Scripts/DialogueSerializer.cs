using System.Xml.Serialization;
using System.Xml;
using System.IO;
using UnityEngine;
using System.Collections;


public class DialogueSerializer
{
    private string pathToResources;
    private LevelCollection LevelsData;
    public DialogueSerializer()
    {
        pathToResources = Application.dataPath;
        LevelsData = new LevelCollection();
    }

    public void SerializeLevelDialogue()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LevelCollection));
        StreamWriter streamWriter = new StreamWriter(File.Create(pathToResources + "/dialoguedata.xml"));
        serializer.Serialize(streamWriter, LevelsData);
        streamWriter.Close();
        streamWriter.Dispose();
    }

    public LevelCollection DeserializeLevelDialogue()
    {
        FileStream filestream;
        XmlReader reader;
        XmlSerializer serializer = new XmlSerializer(typeof(LevelCollection));

        if (!System.IO.File.Exists(pathToResources + "/dialoguedata.xml"))
        {
            filestream = System.IO.File.Create(pathToResources + "/dialoguedata.xml");
            filestream.Close();
            filestream.Dispose();
            SerializeLevelDialogue();
        }
        else
        {
            filestream = new System.IO.FileStream(pathToResources + "/dialoguedata.xml", System.IO.FileMode.Open);
            reader = new XmlTextReader(filestream);
            try
            {
                if (serializer.CanDeserialize(reader))
                {
                    LevelsData = (LevelCollection) serializer.Deserialize(reader);
                }
            }
            finally
            {
                reader.Close();
                filestream.Close();
                filestream.Dispose();
            }
        }
        return LevelsData;
    }
}