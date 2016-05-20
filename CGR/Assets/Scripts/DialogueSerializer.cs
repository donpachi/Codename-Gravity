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
        LevelCollection existingLevelsData = null;

        if (!File.Exists(Application.dataPath + "/dialoguedata.xml"))
        {
            FileStream filestream = File.Create(Application.dataPath + "/dialoguedata.xml");
            filestream.Close();
            filestream.Dispose();
            SerializeLevelDialogue();
        }
        else
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(Application.dataPath + "/dialoguedata.xml", FileMode.Open);
                using (XmlReader reader = new XmlTextReader(fs))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(LevelCollection));
                        return (LevelCollection)serializer.Deserialize(reader);
                    }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }


        }
        return existingLevelsData;
    }
}