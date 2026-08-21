# 멀티플레이어 게임 UI/UX 기획 및 화면 구조 명세서

## 1. 전체 화면 전환 구조 (Screen Flow)

```text
BootScene (로딩) 
  └─ MainMenuScene
       ├─ Create Room Panel ──► (성공) ─┐
       ├─ Find Room Panel   ──► (입장) ─┼─► LobbyScene ──► InGameScene
       ├─ Settings Panel                │                    ├─ HUD (기본)
       └─ Quit Panel                    │                    ├─ Scoreboard (Tab)
                                        │                    ├─ InGame Settings (Esc)
                                        │                    └─ ResultScreen (종료)
                                        └─ (퇴장) ──► MainMenuScene
```

---

## 2. 씬별 UI 상세 구성 및 계층 구조

### 2.1 BootScene
* 스팀에서 로그인 하고 게임 실행 시 로딩하는 동안 해당 씬 재생
* **구성 요소:** 배경화면, 로딩바(Slider), 로딩 상태 텍스트, 진척도(%), 로딩 완료 시 자동 메인메뉴 진입

### 2.2 MainMenuScene
* **패널 전환 규칙:** `MainMenuPanel`은 항상 배경에 유지되며, 팝업 패널 활성화 시 메인 메뉴 버튼 레이어를 비활성화(Click Lock) 처리합니다.
* **Create Room Panel:** 방 제목, 비밀번호, 태그 설정, [생성] / [취소]
* **Find Room Panel:**
  * 검색 필드 & 태그 필터
  * 방 리스트 View (비밀번호 여부, 방 번호, 제목, 방장명, 인원 현황, Ping)
  * [입장] (선택 시 활성화), [새로고침] (1~3초 쿨타임 적용), [취소]
* **Settings Panel:**
  * **화면:** 해상도, 디스플레이 모드(전체/창), 프레임 제한
  * **음향:** Master, BGM, SFX, Voice, Mic Input Level/Mute
  * **조작:** Key Binding (기본값 복원 버튼 포함)
  * **하단:** [저장] (변경 사항 있을 때만 활성화), [취소]
* **Quit Panel:** "게임을 종료하시겠습니까?" 모달 [종료] / [취소]

### 2.3 LobbyScene
* **권한별 UI 제어 (Host vs Client):**
  * **Host (방장):** `Start` 버튼 활성화, 맵/모드 변경 UI, 플레이어 제어(강퇴, 팀 이동, 순서 변경, AI 봇 추가)
  * **Client (참가자):** `Ready` 버튼 활성화 (`Start` 비활성화), 방 설정 Read-Only, 본인 팀 이동 선택 가능

#### 화면 배치 구조
```text
┌──────────────────────────────────────────────────────────┐
│ [🔒] Room Name                             3 / 8 Players │
├──────────────────────────────────────────────────────────┤
│ ┌──────────────────────────┐  ┌────────────────────────┐ │
│ │       Police Team        │  │     ROOM SETTINGS      │ │
│ │ 🔴 Player A  [HOST]  🔊 │  │ Map      : < City >    │ │
│ │ 🔵 Player B  [READY] 🔇 │  │ Mode     : < Classic > │ │
│ │ ⚪ (Empty / Add Bot) 🔊 │  └────────────────────────┘ │
│ └──────────────────────────┘  ┌────────────────────────┐ │
│ ┌──────────────────────────┐  │ Chat Window            │ │
│ │       Robber Team        │  │ [System] Player B join │ │
│ │ 🟢 Player C  [READY] 🔇 │  │ Player A : Hi          │ │
│ │ 🟡 Player D          🔊 │  └────────────────────────┘ │
│ └──────────────────────────┘                             │
├──────────────────────────────────────────────────────────┤
│    [ Ready / Start ]     [ Leave ]     [ Settings ]      │
└──────────────────────────────────────────────────────────┘
```

### 2.4 InGameScene(포트폴리오에서는 수업과 관련된 내용으로 우선 작성하고 나중에 대체)

#### 1) Play HUD
* **상단:** 매치 남은 시간, RoleChange 리스폰 대기 시간, 팀별 체포 현황 (도둑 체포 시 인원 아이콘 비활성화)
* **조준/상호작용:** 테이저 조준점, 상호작용 프로그레스바 (E키: 구출/석방, R키: RoleChange)
* **유틸리티:** 미니맵, 아이템 슬롯 (노템전 시 X 표시), 상점 창, 이동 가능한 채팅창

#### 2) 정보 메뉴 (Tab키 - Scoreboard)
* **플레이어별 정보:** 색상, 팀, 체포 횟수, 피체포 횟수, Rescue 횟수, RoleChange 횟수
* **소셜/사운드 제어:** 개별 음성 조절 슬라이더 및 음성/채팅 차단(Mute) 버튼

#### 3) 설정 메뉴 (Esc키)
* [게임으로 돌아가기], [환경설정], [방 나가기] (확인 팝업 연결)

#### 4) Result Screen
* 승리 팀 연출, 최종 팀 스코어, MVP 및 플레이어별 상세 기여도 리포트

---

## 3. UI Prefab 규격 목록

| 프리팹명 | 구성 요소 | 용도 |
| :--- | :--- | :--- |
| **RoomItem** | 방 번호, 비번 아이콘, 방 제목, 방장명, 인원 현황, Ping | FindRoom 패널의 ScrollView 리스트 항목 |
| **PlayerItem** | 색상 식별자, 닉네임, 방장 아이콘, Ready 상태, 마이크/음소거 아이콘 | 로비 및 인게임 스코어보드의 유저 단위 UI |
| **InteractionPrompt** | 키 인풋 단축키(E/R), 텍스트(Rescue/Release 등), Progress Circle | 월드 공간 또는 HUD 상의 상호작용 팝업 |
| **RoleChangeBar** | 로딩바(Slider), 타이머 텍스트 | RoleChange 구조물 활성화 진척도 표시 |
| **ToastMessage** | 메세지 텍스트, Fade In/Out 애니메이션 | 팀 변경, 유저 입퇴장 등 즉시 알림 |
| **PasswordModal** | InputField, [확인], [취소] | 비번방 입장 시 출력되는 팝업 |

---

## 4. 입력 인터페이스 명세

* **이동 (Move):** `WASD`
* **화면 방향 전환:** `Mouse`
* **정보 메뉴 (Scoreboard):** `Tab`
* **설정 메뉴 / 취소:** `Esc`
* **테이저 발사:** `Mouse Left Button`
* **구출 / 석방 상호작용:** `E`
* **RoleChange 상호작용:** `R`
* **상점 열기:** `B`
* **음성 채팅 (Push-to-Talk):** `V`

---

## 5. 기획 보완 및 예외 처리 가이드

### 1) 세션 및 네트워크 예외 처리
* **핑(Ping) 표시:** `RoomItem` 및 로비 내 플레이어 슬롯에 핑 상태 표시 추가
* **비밀번호 입력 팝업:** 비번방 클릭 시 비밀번호 입력 모달창 연결
* **방장 위임 (Host Migration):** 방장 퇴장 시 다음 순번 플레이어에게 권한 위임 및 ToastMessage 알림
* **세션 끊김 처리:** 네트워크 오류 시 메인메뉴 이동 및 사유 안내 팝업 출력

### 2) 로비 & 인게임 UX 보완
* **시작 제약 조건:** 팀 인원 불균형 또는 전원 Ready 미완료 시 `Start` 버튼 비활성화
* **AI 봇 난이도 설정:** 봇 추가 시 난이도(Easy/Normal/Hard) Dropdown 제공
* **보이스 채널 연출:** 음성 출력 중인 유저의 마이크 아이콘에 파동 애니메이션 적용
* **관전 (Spectator) UI:** 체포/사망 시 생존 팀원 시점 관전 카메라 HUD 제공
* **승리 조건 안내:** HUD 상단에 매치 목표(예: "도둑 전원 체포", "보석 탈취 후 탈출") 상시 표시




# 버그픽스

## Case 1

### 메인메뉴 네비게이션 기능

* 메인메뉴 버튼 네비게이션 추가 후 Sub Panel 열린 상태에서 클릭은 불가능하지만 이동키로 다른 Sub Panel 활성화 가능한 버그 발견

* 