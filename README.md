# ColorDetector
ColorDetector

## 요구사항
### Data Extract	
1.	원하는 동영상을 불러오는 기능
2.	동영상 내 채널 추가 기능
3.	채널 그룹화(시퀀스)
      - 채널 그룹화(시퀀스 그룹 생성), 그룹 해제(시퀀스 그룹 해제) 기능
      - 시퀀스 그룹 내 채널을 네비게이션 바에 순서대로 나열하기 위한 개별 넘버링(자동) 기능
      - 시퀀스 그룹 내 채널의 넘버링 변경(수동) 기능 - Drag & Drop을 이용하여 네비게이션 바에서의 정렬 순서 변경
      - 시퀀스 그룹 해제 시 자동 채널 넘버링 기능 - 그룹에 속하지 않은 채널들을 모아 네비게이션 바에서 정렬되어 보일 수 있도록 하는 기능
      - 시퀀스 그룹 외의 채널 넘버링 변경 기능 - Drag & Drop을 이용하여 네비게이션 바에서의 정렬 순서 변경
      - 시퀀스 그룹명 변경 기능
      - 네비게이션 바에서 채널 정보 선택 시 동영상 위의 실제 채널이 선택된 것처럼 처리되도록 구현
4.	동영상 내 채널을 선택하여 마우스를 드래그하거나 선택된 상태에서 키보드 방향키를 눌러 채널의 위치를 이동할 수 있도록 구현
5.	여러 개의 채널 좌표 정보를 파일로 저장한 후 불러와 좌표를 즉시 보여줄 수 있도록 구현
6.	동영상을 마우스 포인터의 위치를 기준으로 x{0}배 확대/축소할 수 있도록 구현
7.	데이터 저장
      - 데이터 저장 주기는 50ms 단위로 저장
      - 데이터 저장 - 각 시퀀스 그룹에 포함된 채널 정보
      - 데이터 저장 - 각 채널의 좌표
      - 데이터 저장 - 각 채널의 Step별 R, G, B 정보(각각 시트 분할하여 저장)
      - 데이터 저장 - 각 채널의 Step별 Duty 정보
      - 데이터 저장 - 각 채널의 Step별 Duty, R, G, B값을 계산하여 나온 Out 시트 정보(R, G, B 시트 분할하여 저장)
### Scenario Verification	
1.	저장된 데이터(채널의 좌표 및 Step별 RGB, Duty, Out_RGB)를 불러오는 기능
2.	저장된 데이터를 Step 순에 따라 채널의 좌표 위치에서 재생(원 RGB 재생, Duty 재생, Out_RGB재생을 따로 분리하여 선택 재생)
3.	이미지를 불러와 이미지 위에 시나리오 데이터(Scenario Verification 2.의 데이터)를 재생할 수 있는 기능
### Check & Modify	
1.	저장된 데이터를 불러오는 기능
2.	저장된 데이터를 시퀀스 그룹에 포함된 채널 중 선택한 채널을 동시에 볼 수 있도록 제공
3.	시퀀스 그룹에 포함된 채널의 Step별 RGB 정보를 편집할 수 있도록 제공
4.	현대, 기아, 제네시스등 정해진 양식의 Color 서식을 Palette에 띄울 수 있는 기능
5.	한 채널 안에서 Step을 여러개 선택하여 Palette의 색상으로 변경할 수 있는 기능
6.	Palette의 색상에 맞춰 Step별 R, G, B값을 각각 저장할 수 있는 기능
7.	Duty 값을 정해진 규칙에 의거한 등차수열 형태로 설정할 수 있는 기능
8.	6와 7항의 값을 바탕으로 Out_R, Out_G, Out_B값을 각각 저장할 수 있는 기능

---
## Business logic / UseCase 
1. 재생된 동영상 특정 픽셀의 색상과 밝기를 추출
2. 추출된 픽셀이 색상표의 어떤 색상과 근접한지 판단하기
   
------
## 시작전 명확히할 사항

### Sequence Group
1.  Sequence Group이란, 하나 이상의 Channel을 가지고, 특정 방향성으로 재정렬된 채널들의 그룹을 말함.
2.  Channel은 하나의 Sequence Group에만 종속될 수 있음.
     - Channel A가 group a에 종속이었다가, group b로 그룹을 묶으려고할 때는 어떻게 처리될 것인가?
     - 그룹의 생성/삭제, 채널의 생성/삭제의 Input Event는 어떤것이 있을지. (마우스로, 아니면 하이어라키로.)
3.  Sequence Group를 지정하게 되면, 어떤 방향으로 채널들을 정렬할 것인지를 판단해야한다.
4.  Sequence Group의 이름을 지정할수 있음.(중요도 하)
----
### 선택과 이동
1. 선택시, 그룹이 있으면 그룹 선택, 그룹이 없으면 그냥 채널 선택
2. 선택된 상태에서 움직이기 시작하면 그룹이동 (그룹 이동시, GroupSelet는 계속 활성화 = 이동이 끝나면 Bound 재계산)
3. 선택한 것을 또 선택하면, 무조건 단일 선택 (그룹Select해제)
4. 단일 선택된 상태에서 움직이면 단일 이동
5. 이전에 선택한것과 다른 것인데, 같은 그룹이면 그냥 그룹 선택 유지, 단일 채널이면 다른 채널로 변경
6. 


-----

### Channel/Group Data와 Hierarchy & VideoViewer UI들의 관계

1차 구조
![SequenceViewSync](https://github.com/user-attachments/assets/0c2e683c-51be-47d6-b03f-619c56265786)

2차 구조 (25.04.03 변경)

![SequenceViewSync drawio](https://github.com/user-attachments/assets/22a1f66e-684e-42dc-bb3c-84567a05910c)

