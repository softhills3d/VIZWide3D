# VIZWide3D

[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2Fsofthills3d%2FVIZWide3D&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=hits&edge_flat=false)](https://hits.seeyoufarm.com)

## 개요
초대형 3D 모델을 웹 브라우저에서 가시화하는 모듈인 VIZWide3D 의 사용 예제와 활용 코드를 공유하는 저장소입니다.

VIZWide3D 는 버전마다 API 사용 방식이 다릅니다. 예제는 **버전별 폴더**로 나뉘어 있으니, 사용하는 VIZWide3D 버전의 폴더만 참고하세요.
서로 다른 버전의 예제 코드를 섞어 쓰면 실행 시점 오류가 납니다.

---
## 버전별 폴더

| 폴더 | 적용 버전 | 상태 | 문서 (시작하기) |
|---|---|---|---|
| [4.0](./4.0) | VIZWide3D 4.0 (최신) | 예제 준비 중 | https://docs.softhills.net/VIZWide3D/Getting-Started/4.0 |
| [3.0](./3.0) | VIZWide3D 3.0 | 예제 준비 중 | https://docs.softhills.net/VIZWide3D/Getting-Started/3.0 |
| [2.1](./2.1) | VIZWide3D 2.1 | 기존 예제 (`index_*.html`) | https://docs.softhills.net/VIZWide3D/Getting-Started/2.1 |

- 2026-11 이전 저장소 루트에 있던 예제는 모두 `2.1/` 로 옮겼습니다. 상대 경로는 그대로라 폴더 안에서 기존과 같이 실행됩니다.
- API 상세 설명은 VIZWide3D Document ( https://docs.softhills.net/VIZWide3D/ ) 를 참고하세요.

---
## 버전 간 주요 차이 (3.0 → 4.0)

| 구분 | 3.0 | 4.0 |
|---|---|---|
| import | `import VIZCore3DModule from "./VIZCore3D/VIZCore3D.js"` | `import { VIZCore3D } from './VIZCore3D/VIZCore3D.js'` |
| 인스턴스 생성 | DOM 요소를 만들어 전달 | `new VIZCore3D('view')` (요소 id) |
| API 호출 | 동기 호출 위주 | 대부분 `Promise` 반환 → `async/await` |
| 로딩 진행 이벤트 | `Model.OnLoadingProgress` | `Model.OnStreamProgressChangedEvent` |
| 선택 이벤트 | `Object3D.OnObject3DSelected` | `Object3D.OnSelectedEvent` |

---
## 폴더 규칙 (기여자용)
- 버전 폴더 이름은 `4.0`, `3.0`, `2.1` 형식(`major.minor`)을 지킵니다. Softhills 문서 검색(MCP)이 이 이름으로 예제 버전을 구분합니다.
- 4.x 예제는 모두 `4.0/` 에 둡니다(4.1·4.2 폴더를 따로 만들지 않음).
- 각 버전 폴더는 그 안에서 독립 실행되도록 `VIZCore3D/`, `Resource/` 를 포함합니다.
