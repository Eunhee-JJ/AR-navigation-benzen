# 벤젠고리관 AR 네비게이션
성균관대학교 자연과학캠퍼스 벤젠고리관(제1과학관+제2과학관+기초학문관+생명공학관) 방랑자를 위한 AR 네비게이션 시스템

<br>

# 사용 언어 및 기술 스택
- `C`,`C++`,`C#`
- `Unity` - 2021.3.11f1 LTS for Apple Silicon

<br>

# 동작 과정
<img width="100%" src="https://user-images.githubusercontent.com/62401770/215709845-b743d1c9-f26f-4ec1-8538-04897648fcde.png"/>


<br>

# 시연 영상
<img width="60%" src="https://user-images.githubusercontent.com/62401770/215721910-826f054d-44da-4e3f-a741-81764e77e2ae.gif"/>
<img width="60%" src="https://user-images.githubusercontent.com/62401770/215730847-74d518d6-fa1c-4f81-9b41-e33f10dbc017.gif"/>


<br>

# 개선점
1. 측위 오차로 인한 목적지 방향 설정 오차 발생 및 목적지 도착 여부 판별 오류 → GPS 대신 마커인식, Wifi, 비콘 등 무선 통신 기반의 실내 측위 방법 사용

2. 수동 실내 지리 데이터 수집의 한계로 인해 길찾기 범위가 벤젠고리관으로 한정
→ SLAM과 드론을 이용해 실내 지리 데이터를 자동으로 획득함으로써, 다른 실내 건물에서도 보편적으로 사용할 수 있도록 확장
