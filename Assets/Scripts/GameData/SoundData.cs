using System;
using System.IO;
using System.Reflection;
using System.Xml;
using UnityEngine;

/// <summary>
/// 사운드 클립을 배열로 소지, 사운드 데이터 저장 및 로드
/// </summary>
public class SoundData : BaseData
{
    public SoundClip[] soundClips = new SoundClip[0];

    public string clipPath = "Sounds/";
    private string xmlFilePath = string.Empty;
    private string xmlFileName = "soundData.xml";
    private string dataPath = "Json/soundData";
    //XML 구분자
    private const string SOUND = "sound"; //저장 키.
    private const string CLIP = "clip"; //저장 키.

    public SoundData()
    {

    }
    public void SaveData()
    {
        using (XmlTextWriter writer = new XmlTextWriter(this.xmlFilePath + this.xmlFileName, System.Text.Encoding.Unicode))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(SOUND);
            writer.WriteElementString("length", GetDataCount().ToString());
            writer.WriteWhitespace("\n");

            for (int i = 0; i < GetDataCount(); i++)
            {
                writer.WriteStartElement(CLIP);
                writer.WriteElementString("id", i.ToString());
                writer.WriteElementString("name", this.names[i]);
                writer.WriteElementString("loops", this.soundClips[i].checkTime.Length.ToString());
                writer.WriteElementString("maxvol", this.soundClips[i].maxVolume.ToString());
                writer.WriteElementString("pitch", this.soundClips[i].pitch.ToString());
                writer.WriteElementString("dopplerlevel", this.soundClips[i].dopplerLevel.ToString());
                writer.WriteElementString("rolloffmode", this.soundClips[i].rolloffMode.ToString());
                writer.WriteElementString("mindistance", this.soundClips[i].minDistance.ToString());
                writer.WriteElementString("maxdistance", this.soundClips[i].maxDistance.ToString());
                writer.WriteElementString("spatialblend", this.soundClips[i].sparialBlend.ToString());
                if (this.soundClips[i].isLoop)
                {
                    writer.WriteElementString("loop", "true");
                }
                writer.WriteElementString("clippath", this.soundClips[i].clipPath);
                writer.WriteElementString("clipname", this.soundClips[i].clipName);
                writer.WriteElementString("checktimecount", this.soundClips[i].checkTime.Length.ToString());

                string str = "";
                foreach (float t in this.soundClips[i].checkTime)
                {
                    str += t.ToString() + "/";
                }
                writer.WriteElementString("checktime", str);

                str = "";
                writer.WriteElementString("settimecount", this.soundClips[i].setTime.Length.ToString());
                foreach (float t in this.soundClips[i].setTime)
                {
                    str += t.ToString() + "/";
                }
                writer.WriteElementString("settime", str);
                writer.WriteElementString("type", this.soundClips[i].playType.ToString());

                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }
    public void LoadData()
    {
        this.xmlFilePath = Application.dataPath + dataDirectory;
        TextAsset textAsset = ResourceManager.Load<TextAsset>(this.dataPath);
        if (textAsset == null || textAsset.text == null)
        {
            this.AddData("New Sound");
            return;
        }
        using (XmlTextReader reader = new XmlTextReader(new StringReader(textAsset.text)))
        {
            int currentID = 0;
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "length":
                            int length = int.Parse(reader.ReadString());
                            this.names = new string[length];
                            this.soundClips = new SoundClip[length];
                            break;
                        case "id":
                            currentID = int.Parse(reader.ReadString());
                            this.soundClips[currentID] = new SoundClip();
                            this.soundClips[currentID].realId = currentID;
                            break;
                        case "name":
                            this.names[currentID] = reader.ReadString();
                            break;
                        case "loops":
                            int loopCount = int.Parse(reader.ReadString());
                            soundClips[currentID].checkTime = new float[loopCount];
                            soundClips[currentID].setTime = new float[loopCount];
                            break;
                        case "maxvol":
                            soundClips[currentID].maxVolume = float.Parse(reader.ReadString());
                            break;
                        case "pitch":
                            soundClips[currentID].pitch = float.Parse(reader.ReadString());
                            break;
                        case "dopplerlevel":
                            soundClips[currentID].dopplerLevel = float.Parse(reader.ReadString());
                            break;
                        case "rolloffmode":
                            soundClips[currentID].rolloffMode = (AudioRolloffMode)System.Enum.Parse(typeof(AudioRolloffMode), reader.ReadString());
                            break;
                        case "mindistance":
                            soundClips[currentID].minDistance = float.Parse(reader.ReadString());
                            break;
                        case "maxdistance":
                            soundClips[currentID].maxDistance = float.Parse(reader.ReadString());
                            break;
                        case "spatialblend":
                            soundClips[currentID].sparialBlend = float.Parse(reader.ReadString());
                            break;
                        case "loop":
                            soundClips[currentID].isLoop = true;
                            break;
                        case "clippath":
                            soundClips[currentID].clipPath = reader.ReadString();
                            break;
                        case "clipname":
                            soundClips[currentID].clipName = reader.ReadString();
                            break;
                        case "checktimecount":
                            int checkTimeCount = int.Parse(reader.ReadString());
                            soundClips[currentID].checkTime = new float[checkTimeCount];
                            break;
                        case "checktime":
                            SetLoopTime(true, soundClips[currentID], reader.ReadString());
                            break;
                        case "settimecount":
                            int setTimeCount = int.Parse(reader.ReadString());
                            soundClips[currentID].setTime = new float[setTimeCount];
                            break;
                        case "settime":
                            SetLoopTime(false, soundClips[currentID], reader.ReadString());
                            break;
                        case "type":
                            soundClips[currentID].playType = (SoundPlayType)Enum.Parse(typeof(SoundPlayType), reader.ReadString());
                            break;
                    }
                }
            }
        }
        //풀링 기법을 사용하거나 너무 게임이 무거워 지면 이를 삭제하고 풀링을 적용한다
        foreach (SoundClip clip in soundClips)
        {
            clip.PreLoad();
        }
    }

    void SetLoopTime(bool isCheck, SoundClip clip, string timeString)
    {
        string[] time = timeString.Split('/');
        for (int i = 0; i < time.Length; i++)
        {
            if (time[i] != string.Empty)
            {
                if (isCheck)
                {
                    clip.checkTime[i] = float.Parse(time[i]);
                }
                else
                {
                    clip.setTime[i] = float.Parse(time[i]);
                }
            }
        }
    }

    public override int AddData(string newName)
    {
        if (this.names == null)
        {
            this.names = new string[] { newName };
            this.soundClips = new SoundClip[] { new SoundClip() };
        }
        else
        {
            this.names = ArrayHelper.Add(newName, this.names);
            this.soundClips = ArrayHelper.Add(new SoundClip(), this.soundClips);
        }
        return GetDataCount();
    }
    public override void RemoveData(int index)
    {
        this.names = ArrayHelper.Remove(index, this.names);
        if (names.Length == 0)
        {
            this.names = null;
        }
        this.soundClips = ArrayHelper.Remove(index, this.soundClips);
    }
    public SoundClip GetCopy(int index)
    {
        if (index < 0 || index >= soundClips.Length)
        {
            return null;
        }
        SoundClip original = this.soundClips[index];
        SoundClip copy = new SoundClip();

        //리플랙션 기능을 이용한 자동 데이터 카피
        foreach (FieldInfo field in typeof(SoundClip).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            object value = field.GetValue(original);
            field.SetValue(copy, value);
        }
        return copy;
    }
    public SoundClip GetClip(int index)
    {
        if (index < 0 || index >= soundClips.Length)
        {
            return null;
        }
        soundClips[index].PreLoad();
        return soundClips[index];
    }
    public override void CopyData(int index)
    {
        this.names = ArrayHelper.Add(this.names[index], this.names);
        this.soundClips = ArrayHelper.Add(GetCopy(index), this.soundClips);
    }
}
