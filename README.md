# VIZWide3D

[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2Fsofthills3d%2FVIZWide3D&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=hits&edge_flat=false)](https://hits.seeyoufarm.com)

## 개요
초대형 3D 모델을 웹 브라우저에서의 가시화 모듈인 VIZWide3D에 대한 기본 사용 예제 및 활용 코드를 공유하기 위한 목적입니다.

상세 API에 대한 설명 및 예제는 Softhills Documentation Center (https://www.softhills.net/SHDC) 를 참고 하시기 바랍니다.

---
## 구성
+  VIZCore : VIZWide3D 배포폴더 ( 배포 받아서 사용 )
    - MODEL : 예제에 활용되는 모델(*.vizw) 파일
    - UIHeaderBar : 예제에 활용되는 상단 Header 버튼을 위한 UI
+  html : 웹 페이지로 하위 Examples 설명 참고

---

## Examples
| 기능            | 샘플 파일 (.html)  | 출력 형태  | 세부 기능           | 콜백 함수                   | 기능 설명                                                      |
| ------------- | ------------------------ | ----------- | --------------- | ----------------------- | ---------------------------------------------------------- |
| Initialize    | index_initialize    |             | 초기화                | onInit                        |초기화                                                           |
| Configuration | index_configuration |             | 초기 환경 설정 변경 | onConfiguration         | 초기 환경에 설정된 값을 변경                                           |
| Toolbar | index_toolbar |View        |버튼 추가  | cbAddButton         | toolbar에 버튼 추가                                           |
|  |  |View        |서브 버튼 추가  | cbAddSubButton         | toolbar에 서브 버튼 추가                                           |
|  |  |View        |버튼 숨기기  | cbHideButton         | toolbar에 버튼 숨기기                                           |
| ContextMenu | index_contextmenu |View        |메뉴 추가  | cbAddMenu         | contextmenu에 메뉴 추가                                           |
|  |  |View        |메뉴 숨기기  | cbHideMenu         | contextmenu에 메뉴 숨기기                                           |
| File          | index_file          | View        | 파일 열기           | cbOpenFile              | 파일 열기                                                      |
|               |                          | View        | 모델 닫기           | cbClose                 | 파일 닫기                                                      |
|               |                          | View        | 파일 추가           | cbAddFiles              | 파일 추가                                                      |
|               |                          | Console log | 이벤트             | cbModelLoadingCompleted | 파일 로딩 완료 시점 로그 출력                                          |
| Object        | index_object        | Console log | 노드 반환           | cbGetNodeByName         | 특정 'keyword'로 검색 반환된 노드 로그 출력                              |
|               |                          | View        | 노드 선택           | cbSelectNodes           | 특정 'keyword'의 노드 선택                                        |
|               |                          | Console log | 모델 선택 이벤트       | cbSelectNodeEvent       | 선택된 object 노드 로그 출력                                        |
| Color         | index_color         | View        | 노드 색상 변경        | cbSetColor              | 특정 노드 색상 변경                                                |
|               |                          | View        | 노드 색상 초기화       | cbClearColor            | 특정 노드 색상 초기화                                               |
|               |                          | View        | 전체 색상 초기화       | cbClearAll              | 전체 색상 초기화                                                  |
| Property      | index_property      | Console log | 전체 속성 맵 반환      | cbGetAllPropertyMap     | 모델 전체 속성 맵 반환                                              |
|               |                          | Console log | 노드 속성 반환        | cbGetProperty           | 특정 노드의 속성 정보 반환                                            |
|               |                          | View        | 속성에 해당해는 개체 선택  | cbSelectNode            | 특정 속성에 해당하는 개체 선택                                          |
| Review        | index_review        | View        | 리뷰 저장           | cbSaveReview            | 리뷰 (노트, 측정, 스냅샷) 저장 후 json 출력                              |
|               |                          | View        | 리뷰 불러오기         | cbOpenReview            | json 리뷰 열기                                                 |
| Camera        | index_camera        | View        | +X              | cbSetCameraPlusX        | +X 카메라 설정                                                  |
|               |                          | View        | \-X             | cbSetCameraMinusX       | \-X 카메라 설정                                                 |
|               |                          | View        | +Y              | cbSetCameraPlusY        | +Y 카메라 설정                                                  |
|               |                          | View        | \-Y             | cbSetCameraMinusY       | \-Y 카메라 설정                                                 |
|               |                          | View        | +Z              | cbSetCameraPlusZ        | +Z 카메라 설정                                                  |
|               |                          | View        | \-Z             | cbSetCameraMinusZ       | \-Z 카메라 설정                                                 |
|               |                          | View        | +ISO            | cbSetCameraPlusISO      | +ISO 카메라 설정                                                |
|               |                          | View        | \-ISO           | cbSetCameraMinusISO     | \-ISO 카메라 설정                                               |
|               |                          | View        | Matrix 카메라 설정   | cbSetCameraData         | Matrix를 구성해 카메라를 설정                                        |
|               |                          | Console log | 카메라 정보 반환       | cbGetCameraData         | 카메라 정보 반환                                                  |
| Search        | index_search        | Console log | 노드 검색           | cbFindNode              | QuickSearch (Full match : false) - 특정 keyword 검색된 노드 로그 출력|
|               |                          | Console log | 검색어 일치          | cbFindNodeKeywordMatch  | QuickSearch (Full match : true)- 특정 keyword 검색된 노드 로그 출력|
|               |                          | Console log | 속성 포함           | cbIncludeProperty       | 특정 Property 검색된 노드 로그 출력              |
| Note          | index_note          | View        | Surface Note 생성 | cbAddSurfaceNote        | Surface Note 생성                                            |
|               |                          | View        | 3D Note 생성      | cbAdd3DNote             | 3D Note 생성                                                 |
|               |                          | View        | 2D Note 생성      | cbAdd2DNote             | 2D Note 생성                                                 |
|               |                          | View        | Surface Note 수정 | cbChangeSurfaceNote     | Surface Note 수정                                            |
|               |                          | View        | 3D Note 수정      | cbChange3DNote          | 3D Note 수정                                                 |
|               |                          | View        | 2D Note 수정      | cbChange2DNote          | 2D Note 수정                                                 |
|               |                          | Console log | Note 선택 이벤트     | cbSelectNote            | 선택된 Note의 정보 출력                                            |
| Clipping      | index_clipping      | View        | X축 단면 생성        | cbClippingXAxis         | X축 단면 생성                                                   |
|               |                          | View        | Y축 단면 생성        | cbClippingYAxis         | Y축 단면 생성                                                   |
|               |                          | View        | Z축 단면 생성        | cbClippingZAxis         | Z축 단면 생성                                                   |
|               |                          | View        | 선택 상자 단면 생성     | cbClippingSelectBox     | 선택한 모델을 기준으로 박스 단면 생성                                      |
|               |                          | View        | 박스 단면 생성        | cbClippingBox           | 보여지는 모델 기준으로 박스 생성                                         |
|               |                          | View        | 단면 방향 전환        | cbClippingInverse       | X축 or Y축 or Z축 단면의 방향을 전환                                    |
|               |                          | View        | 단면 보이기/숨기기      | cbShowSection           | 단면 보이기/숨기기                                                 |
|               |                          | View        | 단면 삭제           | cbClearSection          | 단면 삭제                                                      |

---



