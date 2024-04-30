using UnityEngine;

public class MainScene : BaseScene
{
    [SerializeField] private AudioClip _bgm;
    [SerializeField] private Transform[] _playerSpawnPoints;
    private Transform _player;
    public override void Initizlize()
    {
        base.Initizlize();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        SceneType = Scene.Main;


        LocatePlayer();

        GameManager.Instance.monsterManager.CreatMonsterSpot();
    }
    public override void Clear()
    {

    }
    public void PlayFieldSound()
    {
        if (GameManager.Instance.playerManager.playerData.quest != null)
        {
            if (GameManager.Instance.playerManager.playerData.quest.QuestID > 1032)
            {
                GameManager.Instance.soundManager.PlayBGM(GameManager.Instance.soundManager.GetSoundClip(SoundList.fieldBGMAfter));
            }
        }

        GameManager.Instance.soundManager.PlayBGM(GameManager.Instance.soundManager.GetSoundClip(SoundList.fieldBGM));
    }
    public void LocatePlayer()
    {
        Debug.Log("LocatePlayer");
        if (GameManager.Instance.sceneManager.PrevScene == Scene.Boss)
        {
            _player.position = _playerSpawnPoints[2].position;
            PlayFieldSound();
            Debug.Log("Player Locate At" + _playerSpawnPoints[2].position);
            return;
        }

        if (GameManager.Instance.playerManager.playerData.questIndex < 6)
        {
            _player.position = _playerSpawnPoints[0].position;
            PlayFieldSound();
            Debug.Log("Player Locate At" + _playerSpawnPoints[0].position);
        }
        else
        {
            _player.position = _playerSpawnPoints[1].position;
            Debug.Log("Player Locate At" + _playerSpawnPoints[1].position);
            Debug.Log("Player Locate At" + _player.position);
        }
    }
}
