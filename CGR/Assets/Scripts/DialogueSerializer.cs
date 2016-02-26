using System.Xml.Serialization;
using System.Xml;
using System.IO;
using UnityEngine;
using System.Collections;


public static class DialogueSerializer
{

    public static void SerializeLevelDialogue()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LevelCollection));
        StreamWriter streamWriter = new StreamWriter(File.Create(Application.dataPath + "/dialoguedata.xml"));
        LevelCollection LevelsData = new LevelCollection();
        serializer.Serialize(streamWriter, LevelsData);
        streamWriter.Close();
        streamWriter.Dispose();
    }

    public static LevelCollection DeserializeLevelDialogue()
    {
        FileStream filestream;
        XmlReader reader;
        LevelCollection existingLevelsData = new LevelCollection();
        XmlSerializer serializer = new XmlSerializer(typeof(LevelCollection));

        if (!System.IO.File.Exists(Application.dataPath + "/dialoguedata.xml"))
        {
            filestream = System.IO.File.Create(Application.dataPath + "/dialoguedata.xml");
            filestream.Close();
            filestream.Dispose();
            SerializeLevelDialogue();
        }
        else
        {
            filestream = new System.IO.FileStream(Application.dataPath + "/dialoguedata.xml", System.IO.FileMode.Open);
            reader = new XmlTextReader(filestream);
            if (serializer.CanDeserialize(reader))
                existingLevelsData = (LevelCollection) serializer.Deserialize(reader);
            reader.Close();
            filestream.Close();
            filestream.Dispose();
        }
        return existingLevelsData;
    }
}