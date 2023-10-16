# 게임 개발 공부
C#, Visual Studio, unity


## 서버 스터디
목표 : C# 기반 소켓프로그래밍 및 비동기 서버 구현, 세션을 통한 패킷 전달, 패킷 생성 자동화, JobQueue를 통해 처리 쓰레드 이용, JobTimer로 일정 시간마다 job 처리.

```
- 멀티쓰레드 및 Lock
- Socket
- Session
- Packet
- JobQueue
- JobTimer
```

## Chess 게임 만들기 클라이언트&서버
목표 : Unity를 통한 체스 클라이언트 개발 및 서버 개발  

### 패킷 생성
```
    <packet name="C_RequestMatching">
    </packet>
    <packet name="S_ResponseMatching">
        <int name="otherPlayerId"/>
        <bool name="amIWhite"/>
    </packet>
    <packet name="C_LeaveGame">
    </packet>
    <packet name="S_BroadcastLeaveGame">
    </packet>
    <packet name="S_PlayerList">
        <list name="player">
            <bool name="isSelf"/>
            <bool name="isInGame"/>
            <int name="playerId"/>
        </list>
    </packet>
    <packet name="C_Move">
        <int name="prevX"/>
        <int name="prevY"/>
        <int name="nextX"/>
        <int name="nextY"/>
        <int name="promotion"/>
    </packet>
    <packet name="S_BroadcastMove">
        <int name="playerId"/>
        <int name="prevX"/>
        <int name="prevY"/>
        <int name="nextX"/>
        <int name="nextY"/>
        <int name="promotion"/>
    </packet>
    <packet name="S_GameOver">
        <bool name="Draw"/>
        <bool name="youWin"/>
        <bool name="youLose"/>
    </packet>

```

### Client
현재 접속한 인원 표시 (나, 게임중)  

체스 규칙 : 이동, 공격, 선택시 경로 표시, 캐슬링, 프로모션, 앙파상, 이동시 내가 체크라면 되돌리기, 체크메이트시 게임오버 구현.  

싱글플레이 : 체스판 리셋, 판 돌리기 기능  

멀티플레이 : match Request를 통해 게임 참가 의사 표시, 매칭 성사시 게임판 리셋 및 시작, 상대의 이동 표시, 내 이동 패킷 보내기, 나갈 시 패배  

### Server
WaitingRoom : 서버에 접속해 있는 모든 인원  

GameRoom : 게임중인 곳 (각 최대 2인)  

JobTimer를 활용해 주기적으로 GameRoom과 WaitingRoom의 job 실행.  

GameManager을 통해 MatchRequest를 보낸 사람을 저장하고, 2명이 되면 GameRoom 생성, 종료시 GameRoom 삭제  

WaitingRoom은 서버에 접속하거나 나갔을 경우 모든 인원들 broadcast.  

GameRoom은 서로의 정보 및 체스말의 이동을 전달. 연결에 따른 승패 전달.  

AWS EC2에 서버 업로드 되어있음.

### 개선 예정
#### 클라이언트
디잔인적 요소 개선 예정



#### 서버
체스말 이동의 검증 로직을 서버에도 두어야한다. 클라이언트가 임의로 패킷을 조작해서 보내고, 검증하지 않으면 이동할 수 없는 곳으로 말이 이동.  
무승부 로직 구현 필요(합의, 50수룰, 3 same position, 기물부족 등)  


#### 싱글플레이 이미지
<img width="1677" alt="스크린샷 2023-10-17 오전 1 42 54" src="https://github.com/thatslifebro/Elice_Web_project1/assets/75624064/9cbe07ea-e0c6-4757-8e9e-2f5809ebe42d">

#### 멀티플레이 이미지
<img width="1673" alt="스크린샷 2023-10-17 오전 1 00 42" src="https://github.com/thatslifebro/Elice_Web_project1/assets/75624064/3745dd14-6c25-4488-aec1-228175491635">
