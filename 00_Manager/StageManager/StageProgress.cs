using System;
// 나중에 PlayingStageProgress 가 있을 수 있으니 헷갈리지 않게..
// StageClearProgress 
public class StageProgress
{
    int _clearStageNum = 0;             // 현재 클리어한 스테이지 번호 / 1스테이지 클리어 한 상태라면 1로 저장된 상태

    int _openedStageRewardStep = 0;     // 열린 리워드 수
    int _receivedStageRewardStep = 0;   // 리워드 어디까지 받았는지

    int _lastPlayingStageRecord = 0; // 클리어하지 못한 마지막 스테이지 기록 (00:00. 08:00 ... 등)

    int _lastSelectedStage = 1;

    public int ClearStageNum => _clearStageNum;
    public int OpenedStageRewardStep => _openedStageRewardStep;
    public int ReceivedStageRewardStep => _receivedStageRewardStep;
    public int LastPlayingStageRecord => _lastPlayingStageRecord;
    public int LastSelectedStage => _lastSelectedStage;


    public StageProgressSaveInfo ExportProgress()
    {
        return new StageProgressSaveInfo
        {
            clearStageNum = _clearStageNum,
            openedStageRewardStep = _openedStageRewardStep,
            receivedStageRewardStep = _receivedStageRewardStep,
            lastPlayingStageRecord = _lastPlayingStageRecord,
            lastSelectedStage = _lastSelectedStage,
        };
    }


    public void ImportStageProgress(StageProgressSaveInfo stageClearProgressSave)
    {
        _clearStageNum = stageClearProgressSave.clearStageNum;
        _openedStageRewardStep = stageClearProgressSave.openedStageRewardStep;
        _receivedStageRewardStep = stageClearProgressSave.receivedStageRewardStep;
        _lastPlayingStageRecord = stageClearProgressSave.lastPlayingStageRecord;
        _lastSelectedStage = stageClearProgressSave.lastSelectedStage;
    }


    public void SaveStagePrgress(int stageNum, int lastPlayingStageRecord)
    {
        if(_clearStageNum <= stageNum)  // 진행단계보다 낮은스테이지를 플레이 했을 때, 진행도를 덮으면 안됨.
            _clearStageNum = stageNum;

        _lastSelectedStage = stageNum;
        _lastPlayingStageRecord = lastPlayingStageRecord;
    }

    public void SaveLastSelectedStage(int selectedStageNum)
    {
        _lastSelectedStage = selectedStageNum;
    }

}

[Serializable]
public class StageProgressSaveInfo
{
    public int clearStageNum;
    public int openedStageRewardStep;
    public int receivedStageRewardStep;
    public int lastPlayingStageRecord;
    public int lastSelectedStage;
}


