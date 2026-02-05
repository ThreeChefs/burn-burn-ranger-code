# 탄탄특공대
<details>
  <summary>목차</summary>
  <ol>
    <li>
      <a href="#🎮게임-소개">🎮 게임 소개</a>
      <ul>
        <li><a href="#시연-영상">시연 영상</a></li>
      </ul>
    </li>
    <li>
      <a href="#👨‍💻프로젝트-소개">👨‍💻 프로젝트 소개</a>
      <ul>
        <li><a href="#개발-기간">개발 기간</a></li>
        <li><a href="#주요-기능">주요 기능</a></li>
        <li><a href="#기술-목록">기술 목록</a></li>
      </ul>
    </li>
    <li>
      <a href="#📄관련-문서">📄 관련 문서</a>
    </li>
    <li>
      <a href="#👨‍👩‍👧‍👧팀원-소개">👨‍👩‍👧‍👧 팀원 소개</a>
    </li>
  </ol>
</details>
<br/>

# 🎮게임 소개
1. 제목 :  **탄탄특공대** 
2. 장르: 뱀서라이크 / 캐쥬얼 / 2D 
3. 플랫폼: Android

## 시연 영상
<a href="https://www.youtube.com/watch?v=XtuRV5AsIME" target="_blank" rel="noopener noreferrer">
  <img width="287" height="476"
       src="https://github.com/user-attachments/assets/7e7fcb72-a0b7-47f7-919f-d7c9f22c71b8" />
</a>
<br/>


# 👨‍💻프로젝트 소개
<img width="5400" height="3840" alt="image" src="https://github.com/user-attachments/assets/09526532-8b3d-4ee0-ac46-37a82f417aaf" />

## 개발 기간
### 2026/01/03 ~ 2026/02/01
<br/>

## 주요 기능

>#### 스테이지 / 웨이브
- 스테이지의 몬스터 웨이브 관리와 스테이지 내 이벤트를 정의
- `StageData`와 `StageWaveEntry` 로 스테이지의 정보 및 웨이브를 ScriptableObject로 관리

>#### 스테이지 플레이어
- 플레이어 상태(HP, 피격, 사망) 관리 로직 구현
- 스킬·장비 효과가 플레이어에 적용되는 흐름 정리
- 스테이지 전용 플레이어 컨텍스트 분리

>#### 장비 효과
- 장비 효과를 Effect 단위의 ScriptableObject 구조로 분리
- 스테이지 내부에서 인스턴스 생성 방식으로 효과 적용
- 효과 중첩 및 조건부 발동(피격, 처치 등)을 고려한 구조 설계

>#### 몬스터
- 몬스터 공통 로직 모듈화
- 데미지·넉백·상태 처리 통합으로 몬스터 타입 추가 비용 최소화
- SO 데이터 기반 데이터 관리
- 스탯·랭크·공격 타입을 데이터화하여 밸런싱 및 유지보수 효율 개선

>#### 보스몬스터
- 확장 가능한 보스 패턴 설계
- `BossPatternBase` 기반으로 HP 트리거·패턴 분기 가능한 구조 구현

>#### 다양한 스킬 / 투사체
- 스킬 데이터와 투사체 실행 로직 분리
- 투사체 이동, 충돌, 효과 처리를 모듈화
- `HitContext`와 `BaseSkillEffectSO`를 통해 실제 효과 로직 분리
- 오브젝트 풀링 기반 투사체 생성 및 관리

>#### UI
- 사용 가능한 UI 프리팹 로드와 로드된 UI를 편리하게 이용할 수 있도록 설계
- `BaseUI`, `PopupUI` 등 미리 정의된 UI Template 을 이용해 손쉬운 추가 UI 프리팹 생성
- SafeArea가 필요한 RectTransform 을 지정하여 사용할 수 있도록 구성
- 정의된 SortOrder 캔버스 지정과 Sibling 순서를 통한 쉬운 UI SortOrder 관리
- 미리 정의된 애니메이션을 통해 빠른 팝업 애니메이션 추가

>#### 오브젝트 풀링
- `PoolManager` 로 풀의 카테고리화, `BasePool` 로 오브젝트의 종류, `PoolObject` 로 게임오브젝트를 구성
- `PoolManager` 의 제너릭과 추상화로 카테고리의 확장에 용이. ex) `MonsterManager`, `ProjectileManager` 등
- 오브젝트를 순서대로 사용하지 못하는 게임특성을 고려하여 반환 이벤트와 HashSet 자료구조 선택

>#### 캐릭터 스탯
- 플레이어와 몬스터가 공통으로 사용하는 캐릭터 스탯 구조 설계
- 기본 스탯과 보정 스탯 분리 구조 설계
- 장비, 버프, 스킬에 따른 스탯 변동 처리
- 런타임 스탯 계산 결과 캐싱 적용
- 확장성을 고려해 스탯을 entry 단위로 관리

>#### 인벤토리 & 장비 아이템
- 장비 장착 및 해제 기능 구현
- 장비 장착·변경 시 스탯 및 효과 갱신 처리
- 장비 합성 및 강화 시스템 구현
- 장비 뽑기 시스템 구현

>#### 저장시스템

>#### Scene 관리

>#### 사운드매니저
- 사운드의 사용 정보를 `AudioClipEntry`, `AudioClipGroupData` 에 담아 필요한 클립의 네이밍 관리

<br/>

## 기술 목록
<img width="1119" height="299" alt="image" src="https://github.com/user-attachments/assets/4d328f07-9ddf-43df-8420-8b27c15b0906" />
<br/>

# 📄관련 문서
- [프로젝트 발표표 PPT](https://docs.google.com/presentation/d/1DMqh6mgatSqGv7bmpunlaF4nyr540FrEz099Zwh4X2w/edit?usp=sharing)


<br/>

# 👨‍👩‍👧‍👧팀원 소개

| **이름** | **링크** |  **담당** |
|------|------|------|
| 곽호빈 | [Github](https://github.com/hobin1117-source)<br/>[Blog](https://hobin1117.tistory.com/) |몬스터<br/>스테이지 아이템<br/> 저장 시스템 <br/>외 |
| 신주은 | [Github](https://github.com/shin0112)<br/>[Blog](https://velog.io/@shin0112/posts)  |플레이어 <br/> 스킬 시스템 <br/> 프로젝타일 <br/> 장비 시스템 <br/> 외|
| 전수라 | [Github](https://github.com/sooroora)<br/>[Blog](https://sooroora.tistory.com/) | 스테이지/웨이브<br/>UI 시스템<br/>풀링 시스템<br/>성장 시스템<br/>외 |
