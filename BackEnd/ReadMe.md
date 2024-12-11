# 프로젝트 소개
이 프로젝트는 C#으로 작성된 서버 애플리케이션으로, HTTP 요청을 처리하고 Oracle 및 MSSQL 데이터베이스와 상호 작용하는 예제 코드입니다. 파일 다운로드, 데이터베이스 조회 등의 기능을 제공합니다.

---

## 주요 기능
- **파일 관리**: 서버를 통해 파일을 다운로드 할 수 있습니다.
- **데이터베이스 연동**: Oracle과 MSSQL 데이터베이스에서 데이터를 조회할 수 있습니다.
- **HTTP 서버**: `Softhills.Net.HttpServerCore`를 사용하여 HTTP 요청을 처리합니다.

---

## 폴더 구조
- dlls
  - Softhills.Framework.dll
  - Oracle.ManagedDataAccess.dll
- Softhills.Server
  - Program.cs
- Softhills.Database
  - OracleManager.cs
  - SqlManager.cs

---

## 설정 방법
### 1. 필요한 DLL 추가
`dlls` 폴더에 포함된 `Softhills.Framework.dll`과 `Oracle.ManagedDataAccess.dll`을 프로젝트에 추가합니다.

1. Visual Studio에서 솔루션 탐색기에서 프로젝트를 마우스 오른쪽 버튼으로 클릭하고 **참조 추가**를 선택합니다.
2. **찾아보기**를 클릭하고 `dlls` 폴더에서 해당 DLL을 선택하여 추가합니다.

### 2. 환경 설정
`App.config` 또는 환경 설정 파일에서 데이터베이스 연결 문자열과 서버 설정을 구성합니다.

#### 예시
- 실제 데이터베이스 정보를 입력해야 합니다.
```xml
<appSettings>
  <!-- Oracle Database Settings -->
  <add key="DATABASE_HOST_TEST" value="your_oracle_host" />
  <add key="DATABASE_PORT_TEST" value="1521" />
  <add key="DATABASE_SERVICE_NAME_TEST" value="your_service_name" />
  <add key="DATABASE_USERID_TEST" value="your_username" />
  <add key="DATABASE_PASSWORD_TEST" value="your_password" />

  <!-- MSSQL Database Settings -->
  <add key="DATABASE_HOST_TEST1" value="your_sql_host" />
  <add key="DATABASE_PORT_TEST1" value="1433" />
  <add key="DATABASE_SERVICE_NAME_TEST1" value="your_database_name" />
  <add key="DATABASE_USERID_TEST1" value="your_username" />
  <add key="DATABASE_PASSWORD_TEST1" value="your_password" />
</appSettings>
```

## 사용 방법
### 1. 서버 실행
**관리자 권한**으로 `Visual Studio`를 실행 후 빌드하여 서버를 시작합니다.

- 서버는 기본적으로 **포트 80**에서 실행되며, 필요에 따라 포트를 변경할 수 있습니다.

### 2. API 요청 형식
서버는 HTTP GET 요청을 통해 다양한 기능을 제공합니다.

- 요청 형식:
```
http://IP_ADDRESS:PORT/SOFTHILLS?CMD=COMMAND&PARAM1=VALUE1&PARAM2=VALUE2
```

#### 예제
- **VIZW 다운로드**  
  - `SHIP`: 호선 정보
  - `BLOCK`: 블록 정보
  - `NAME`: 파일 이름
    - 파일 이름은 `/` 를 반드시 **포함** 하여야 합니다.
```
http://localhost/SOFTHILLS?CMD=VIZW&SHIP=3380&BLOCK=A21P0&NAME=/A21P0_wh.vizw
```

- **Oracle 데이터 조회**  
```
http://localhost/SOFTHILLS?CMD=GetOracleData
```

- **MSSQL 데이터 조회**  
```
http://localhost/SOFTHILLS?CMD=GetSqlData
```

---

## 클래스 설명
### **Program.cs**
- 서버의 진입점이며, HTTP 요청을 수신하고 처리합니다.
- `ProcessVIZW` 등의 메서드를 통해 각 기능을 구현합니다.

### **OracleManager.cs**
- Oracle 데이터베이스와의 통신을 담당합니다.
- `Get` 메서드를 통해 데이터를 조회합니다.

### **SqlManager.cs**
- MSSQL 데이터베이스와의 통신을 담당합니다.
- `Get` 메서드를 통해 데이터를 조회합니다.

---

## 주의 사항
1. **DLL 관리**
 - `dlls` 폴더에 포함된 DLL 파일들을 반드시 프로젝트에 추가하고 참조 설정을 해야 합니다.
 - DLL 파일의 버전이 맞지 않으면 오류가 발생할 수 있으니 주의하세요.

2. **보안**
 - `ACCESSKEY`와 같은 민감한 정보는 코드에 하드코딩하지 않고 안전하게 관리해야 합니다.
 - 데이터베이스 연결 정보도 보안에 유의하여 관리해야 합니다.

3. **에러 처리**
 - 예제 코드에서는 에러 처리가 생략되거나 간단히 되어 있습니다. 실제 운영 시에는 예외 처리를 강화해야 합니다.

4. **포트 사용**
 - 포트 80은 일반적으로 관리자 권한이 필요합니다. 권한이 없으면 다른 포트를 사용하거나 관리자 권한으로 실행하세요.

---

## 라이선스
이 프로젝트는 **MIT 라이선스**를 따릅니다.
