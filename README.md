# 🎨 ColorDetector

> 동영상에서 색상 정보를 추출하고 분석하는 시각화 툴입니다.

---

## 📌 요구사항

<details>
<summary><strong>📁 Data Extract</strong></summary>

1. 동영상 불러오기  
2. 동영상 내 채널 추가  
3. 채널 그룹화 (Sequence)  
   - 시퀀스 그룹 생성/해제  
   - 네비게이션 바에 넘버링 자동 부여  
   - 넘버링 수동 변경 (Drag & Drop)  
   - 그룹 해제 시 자동 정렬  
   - 그룹 외 채널의 정렬 순서 변경  
   - 시퀀스 그룹명 변경  
   - 채널 선택 시 동영상 상 채널 강조 표시  
4. 채널 위치 이동 (마우스 드래그 / 키보드 방향키)  
5. 채널 좌표 정보 저장 및 불러오기  
6. 마우스 포인터 기준 동영상 확대/축소  
7. 데이터 저장 (50ms 주기)  
   - 시퀀스 그룹 정보  
   - 채널 좌표  
   - Step별 RGB 정보 (시트 분할)  
   - Step별 Duty 정보  
   - Out_RGB 정보 (시트 분할 저장)  

</details>

<details>
<summary><strong>🎞️ Scenario Verification</strong></summary>

1. 저장된 데이터 불러오기 (좌표, RGB, Duty, Out_RGB)  
2. Step 순서에 따른 재생 기능  
   - RGB / Duty / Out_RGB 각각 선택 재생  
3. 이미지 위에 시나리오 데이터를 재생  

</details>

<details>
<summary><strong>🛠️ Check & Modify</strong></summary>

1. 저장된 데이터 불러오기  
2. 특정 시퀀스 그룹의 채널 동시 보기  
3. Step별 RGB 정보 편집  
4. 현대/기아/제네시스 색상 서식 불러오기 (Palette)  
5. 복수 Step 선택 후 색상 변경  
6. RGB값 저장  
7. Duty 값을 등차수열로 설정  
8. RGB + Duty 기반 Out_RGB 계산 및 저장  

</details>

---

## ⚙️ Business Logic / Use Case

- 재생된 영상에서 특정 픽셀의 색상과 밝기 추출  
- 추출된 색상이 미리 정의된 색상표와 얼마나 유사한지 비교  

---

## 🧭 명확히 할 사항

<details>
<summary><strong>📌 Sequence Group</strong></summary>

- 하나 이상의 Channel을 특정 순서로 정렬한 그룹  
- 각 Channel은 하나의 Sequence Group에만 소속 가능  

#### 예시 고려 사항

- Channel A가 group a에서 b로 이동할 때 처리 방식  
- 그룹 생성/삭제 시 입력 방식 (마우스, UI 등)  
- 정렬 방향의 판단 기준  
- 시퀀스 그룹 이름 설정 가능  

</details>

<details>
<summary><strong>✋ 선택과 이동</strong></summary>

1. 선택 시 그룹이 존재하면 그룹 선택, 아니면 단일 채널 선택  
2. 선택 상태에서 이동 시 → 그룹 이동 (이동 종료 후 Bound 재계산)  
3. 선택 항목을 다시 선택하면 → 무조건 단일 선택  
4. 단일 선택 상태에서 이동 시 → 단일 이동  
5. 같은 그룹 내 다른 항목 선택 시 → 그룹 선택 유지  

</details>

---

## 🧩 Channel/Group Data와 UI의 관계

### 1차 구조도
![SequenceViewSync](https://github.com/user-attachments/assets/0c2e683c-51be-47d6-b03f-619c56265786)

### 2차 구조도 (2025-04-03 변경)
![SequenceViewSync drawio](https://github.com/user-attachments/assets/22a1f66e-684e-42dc-bb3c-84567a05910c)
