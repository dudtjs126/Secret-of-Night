using System.Text;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public class SoundTool : EditorWindow
{
    public int uiWidthLarget = 450;
    public int uiWidthMiddle = 300;
    public int uiWidthSmall = 200;

    private int selection = 0;
    private Vector2 ScrollPos_1 = Vector2.zero;
    private Vector2 ScrollPos_2 = Vector2.zero;
    private AudioClip soundSource;
    private static SoundData soundData;

    [MenuItem("Tools/Sound Tool")]
    static void Init()
    {
        soundData = CreateInstance<SoundData>();
        soundData.LoadData();

        SoundTool window = GetWindow<SoundTool>(false, "Sound Tool");
        window.Show();
    }

    private void OnGUI()
    {
        if (soundData == null)
        {
            return;
        }
        EditorGUILayout.BeginVertical();
        {
            // 상단, add, remove, copy 버튼
            UnityObject source = soundSource;
            EditorHelper.EditorToolTopLayer(soundData, ref selection, ref source, this.uiWidthMiddle);
            soundSource = (AudioClip)source;

            EditorGUILayout.BeginHorizontal();
            {
                //중간 데이터 목록
                EditorHelper.EditorToolListLayer(ref ScrollPos_1, soundData, ref selection, ref source, this.uiWidthMiddle);
                SoundClip soundClip = soundData.soundClips[selection];
                soundSource = (AudioClip)source;

                //설정
                EditorGUILayout.BeginVertical();
                {
                    this.ScrollPos_2 = EditorGUILayout.BeginScrollView(this.ScrollPos_2);
                    {
                        if (soundData.GetDataCount() > 0)
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.Separator();
                                EditorGUILayout.LabelField("ID", selection.ToString(), GUILayout.Width(this.uiWidthLarget));
                                soundData.names[selection] = EditorGUILayout.TextField("Name", soundData.names[selection],
                                    GUILayout.Width(this.uiWidthLarget));
                                soundClip.playType = (SoundPlayType)EditorGUILayout.EnumPopup("Play Type", soundClip.playType,
                                    GUILayout.Width(this.uiWidthLarget));
                                soundClip.maxVolume = EditorGUILayout.FloatField("Max Volume", soundClip.maxVolume,
                                    GUILayout.Width(this.uiWidthLarget));
                                soundClip.isLoop = EditorGUILayout.Toggle("LoopClip", soundClip.isLoop, GUILayout.Width(this.uiWidthLarget));
                                EditorGUILayout.Separator();
                                if (this.soundSource == null && soundClip.clipName != string.Empty)
                                {
                                    soundClip.PreLoad();
                                    this.soundSource = Resources.Load<AudioClip>(soundClip.clipfullPath);
                                }
                                this.soundSource = EditorGUILayout.ObjectField("Audio Clip", this.soundSource, typeof(AudioClip), false,
                                    GUILayout.Width(this.uiWidthLarget)) as AudioClip;
                                if (this.soundSource != null)
                                {
                                    soundClip.clipPath = EditorHelper.GetPath(this.soundSource);
                                    soundClip.clipName = this.soundSource.name;
                                    soundClip.pitch = EditorGUILayout.Slider("Pitch", soundClip.pitch, -3.0f, 3.0f,
                                        GUILayout.Width(this.uiWidthLarget));
                                    soundClip.dopplerLevel = EditorGUILayout.Slider("Doppler Level", soundClip.dopplerLevel, 0.0f, 5.0f,
                                        GUILayout.Width(this.uiWidthLarget));
                                    soundClip.rolloffMode = (AudioRolloffMode)EditorGUILayout.EnumPopup("Rolloff Mode", soundClip.rolloffMode,
                                        GUILayout.Width(this.uiWidthLarget));
                                    soundClip.minDistance = EditorGUILayout.FloatField("Min Distance", soundClip.minDistance,
                                        GUILayout.Width(this.uiWidthLarget));
                                    soundClip.maxDistance = EditorGUILayout.FloatField("Max Distance", soundClip.maxDistance,
                                        GUILayout.Width(this.uiWidthLarget));
                                    soundClip.sparialBlend = EditorGUILayout.Slider("Sparial Blend", soundClip.sparialBlend, 0.0f, 1.0f,
                                        GUILayout.Width(this.uiWidthLarget));
                                }
                                else
                                {
                                    soundClip.clipName = string.Empty;
                                    soundClip.clipPath = string.Empty;
                                }
                                EditorGUILayout.Separator();
                                if (GUILayout.Button("Add Loop", GUILayout.Width(this.uiWidthMiddle)))
                                {
                                    soundClip.AddLoop();
                                }
                                for (int i = 0; i < soundClip.checkTime.Length; i++)
                                {
                                    EditorGUILayout.BeginVertical("box");
                                    {
                                        GUILayout.Label("Loop Step " + i, EditorStyles.boldLabel);
                                        if (GUILayout.Button("Remove", GUILayout.Width(this.uiWidthMiddle)))
                                        {
                                            soundClip.RemoveLoop(i);
                                            return;
                                        }

                                        soundClip.checkTime[i] = EditorGUILayout.FloatField("Check Time", soundClip.checkTime[i],
                                            GUILayout.Width(this.uiWidthMiddle));
                                        soundClip.setTime[i] = EditorGUILayout.FloatField("Set Time", soundClip.setTime[i],
                                            GUILayout.Width(this.uiWidthMiddle));
                                    }
                                    EditorGUILayout.EndVertical();
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Reload Settings"))
            {
                soundData = CreateInstance<SoundData>();
                soundData.LoadData();
                selection = 0;
                this.soundSource = null;
            }
            if (GUILayout.Button("Save"))
            {
                soundData.SaveData();
                CreateEnumStructure();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    public void CreateEnumStructure()
    {
        string enumName = "SoundList";
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < soundData.GetDataCount(); i++)
        {
            if (soundData.names[i] != string.Empty)
            {
                sb.AppendLine("     " + soundData.names[i] + " = " + i + ",");
            }
        }
        EditorHelper.CreateEnumStructure(enumName, sb);
    }
}
