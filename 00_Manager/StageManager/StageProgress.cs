using System;
// 나중에 PlayingStageProgress 가 있을 수 있으니 헷갈리지 않게..
// StageClearProgress 
public class StageProgress
{
    int _clearStageNum = 0;             // 현재 클리어한 스테이지 번호 / 1스테이지 클리어 한 상태라면 1로 저장된 상태
    int _lastSelectedStageNum = 1;         // 마지막에 선택되어있던 스테이지 번호

    int _openedStageRewardStep = 0;     // 열린 리워드 수
    int _receivedStageRewardStep = 0;   // 리워드 어디까지 받았는지

    int _lastPlayingStageRecord = 0; // 클리어하지 못한 마지막 스테이지 기록 (00:00. 08:00 ... 등)


    public int ClearStageNum => _clearStageNum;
    public int OpenedStageRewardStep => _openedStageRewardStep;
    public int ReceivedStageRewardStep => _receivedStageRewardStep;
    public int LastPlayingStageRecord => _lastPlayingStageRecord;
    public int LastSelectedStageNum => _lastSelectedStageNum;


    public StageProgressSaveInfo ExportProgress()
    {
        return new StageProgressSaveInfo
        {
            clearStageNum = _clearStageNum,
            openedStageRewardStep = _openedStageRewardStep,
            receivedStageRewardStep = _receivedStageRewardStep,
            lastPlayingStageRecord = _lastPlayingStageRecord,
            lastSelectedStageNum = _lastSelectedStageNum,
        };
    }


    public void ImportStageProgress(StageProgressSaveInfo stageClearProgressSave)
    {
        _clearStageNum = stageClearProgressSave.clearStageNum;
        _openedStageRewardStep = stageClearProgressSave.openedStageRewardStep;
        _receivedStageRewardStep = stageClearProgressSave.receivedStageRewardStep;
        _lastPlayingStageRecord = stageClearProgressSave.lastPlayingStageRecord;
        _lastSelectedStageNum = stageClearProgressSave.lastSelectedStageNum;
    }


    public void SaveStagePrgressNum(int stageNum, int lastPlayingStageRecord)
    {
        if(_clearStageNum <= stageNum)  // 진행단계보다 낮은스테이지를 플레이 했을 때, 진행도를 덮으면 안됨.
            _clearStageNum = stageNum;

        if (GameManager.Instance.StageDatabase.Count < stageNum)
        {
            _lastSelectedStageNum = GameManager.Instance.StageDatabase.Count;   // 최대 스테이지 넘은 이후
        }
        else _lastSelectedStageNum = stageNum;

        _lastPlayingStageRecord = lastPlayingStageRecord;

    }

    public void SaveLastSelectedStage(int selectedStageNum)
    {
        _lastSelectedStageNum = selectedStageNum;
    }

}

[Serializable]
public class StageProgressSaveInfo
{
    public int clearStageNum;
    public int openedStageRewardStep;
    public int receivedStageRewardStep;
    public int lastPlayingStageRecord;
    public int lastSelectedStageNum;
}


