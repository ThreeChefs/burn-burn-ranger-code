using System;

// 나중에 PlayingStageProgress 가 있을 수 있으니 헷갈리지 않게..
// StageClearProgress 
public class StageClearProgress
{
    int _clearStageNum = 0;             // 현재 클리어한 스테이지 번호

    int _openedStageRewardStep = 0;     // 열린 리워드 수
    int _receivedStageRewardStep = 0;   // 리워드 어디까지 받았는지

    float _lastPlayingStageRecord = 0f; // 클리어하지 못한 마지막 스테이지 기록 (00:00. 08:00 ... 등)

    public int ClearStageNum => _clearStageNum;
    public int OpenedStageRewardStep => _openedStageRewardStep;
    public int ReceivedStageRewardStep => _receivedStageRewardStep; 


    public StageClearProgressSave ExportProgress()
    {
        return new StageClearProgressSave
        {
            clearStageNum = _clearStageNum,
            openedStageRewardStep = _openedStageRewardStep,
            receivedStageRewardStep = _receivedStageRewardStep

        };
    }
    

}

[Serializable]
public class StageClearProgressSave
{
    public int clearStageNum;
    public int openedStageRewardStep;
    public int receivedStageRewardStep;

}


