using System;

// 나중에 PlayingStageProgress 가 있을 수 있으니 헷갈리지 않게..
// StageClearProgress 
public class StageClearProgress
{
    int _clearStageNum = 0;             // 현재 클리어한 스테이지 번호 / 1스테이지 클리어 한 상태라면 1로 저장된 상태

    int _openedStageRewardStep = 0;     // 열린 리워드 수
    int _receivedStageRewardStep = 0;   // 리워드 어디까지 받았는지

    int _lastPlayingStageRecord = 0; // 클리어하지 못한 마지막 스테이지 기록 (00:00. 08:00 ... 등)

    public int ClearStageNum => _clearStageNum;
    public int OpenedStageRewardStep => _openedStageRewardStep;
    public int ReceivedStageRewardStep => _receivedStageRewardStep; 
    public int LastPlayingStageRecord => _lastPlayingStageRecord;


    public StageClearProgressSave ExportProgress()
    {
        return new StageClearProgressSave
        {
            clearStageNum = _clearStageNum,
            openedStageRewardStep = _openedStageRewardStep,
            receivedStageRewardStep = _receivedStageRewardStep,
            lastPlayingStageRecord = _lastPlayingStageRecord,
        };
    }

    public void SaveStagePrgress(int stageNum, int lastPlayingStageRecord)
    {
        _clearStageNum = stageNum;
        _lastPlayingStageRecord= lastPlayingStageRecord;
    }
    
}

[Serializable]
public class StageClearProgressSave
{
    public int clearStageNum;
    public int openedStageRewardStep;
    public int receivedStageRewardStep;
    public int lastPlayingStageRecord;

}


