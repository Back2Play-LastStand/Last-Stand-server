# Last-Stand-server
Last Stand 서버 저장소입니다.

계정 시스템, 인증, 데이터 관리 등 주요 서버 로직을 담당합니다.

## 🛠 기술 스택

- **ASP.NET Core 8**
- **C#**
- **Dapper**
- **MySqlConnection**
- **BCrypt.Net**
- **MySQL**
- **REST API**
- **Session Authentication**

## 📘 Last Stand API 명세서

### 📂 Account API

### 🔍 플레이어 ID 찾기

- **URL**: `POST /api/account/player-id?email=test@example.com`
- **설명**: 이메일을 통해 플레이어 ID를 조회합니다.
- **쿼리 파라미터**:

```markdown
email (string, required) - 조회할 이메일 주소
```

- **성공 응답 (조회 성공** ``200 OK``**)**

```json
{
  "playerId": "admin"
}
```

- **실패 응답 (존재하지 않는 이메일** ``404 Not Found``**)**

```json
{
  "playerId": null
}
```

### 🔐 비밀번호 재설정

- **URL**: `PATCH /api/account/password`
- **설명**:  playerId와 이메일이 일치하면 비밀번호를 재설정합니다.
- **요청 바디**:

```json
{
  "playerId": "admin",
  "email": "test@example.com",
  "newPassword": "1234"
}
```

- **성공 응답 (비밀번호 재설정 성공** ``200 OK``**)**

```json
{
  "success": true,
  "message": "Password has been reset successfully."
}
```

- **실패 응답 (playerId 또는 이메일 불일치** ``400 Bad Request``**)**

```json
{
  "success": false,
  "message": "PlayerId and email do not match."
}
```

### 📂 Auth API

### 📝 회원가입

- **URL**: `POST /api/auth/register`
- **설명**: 플레이어 ID와 비밀번호로 계정을 생성합니다
- **요청 바디**:

```json
{
  "playerId": "admin",
  "password": "1234",
  "email": "admin@example.com"
}
```

- **성공 응답 (등록 성공** ``200 OK``**)**

```json
{
  "playerId": "admin",
  "message": "Register Success",
  "email": "test@example.com"
}
```

- **실패 응답 (중복된 아이디** ``409 Conflict``**)**

```json
{
  "playerId": "admin",
  "message": "Id already exists",
  "email": "test@example.com"
}
```

### 📝 로그인

- **URL**: `POST /api/auth/register`
- **설명**: 플레이어 ID와 비밀번호로 계정을 생성합니다
- **요청 바디**:

```json
{
  "playerId": "admin",
  "password": "1234"
}
```

- **성공 응답 (로그인 성공** ``200 OK``**)**

```json
{
  "playerId": "admin",
  "isNewAccount": true,
  "sessionId": "session-token-value",
  "message": "Login successful"
}
```
- **실패 응답 1 (로그인 실패** ``401 Unauthorized``**)**

```json
{
  "playerId": "admin",
  "message": "Login Failed"
}
```

- **실패 응답 2 (이미 로그인한 계정** ``409 Conflict``)

```json
{
  "playerId": "admin",
  "isNewAccount": true,
  "message": "Already logged in"
}
```

### 📂 Data API

### 🔍 플레이어 이름 조회

- **URL**: `GET /api/data/name?playerId={playerId}`
- **헤더**: `Session-Id` (필수) - 유효한 세션 토큰
- **설명**: 로그인한 사용자가 플레이어 이름을 최초로 등록합니다.
이미 등록된 계정은 이름을 등록할 수 없습니다.

- **쿼리 파라미터**:

```markdown
playerId (string, required) - 조회할 플레이어 아이
```

- **성공 응답 (조회 성공** ``200 OK``**)**

```json
{
  "playerName": "Admin"
}
```
- **실패 응답 1 (세션 ID 없음** `401 Unauthorized`**)**

```json
{
  "message": "Session Id Is Not Found"
}
```

- **실패 응답 2 (세션 ID 유효하지 않음** `401 Unauthorized`**)**

```json
{
  "message": "Invalid or expired session."
}
```

- **실패 응답 3 (세션의 계정과 playerId 불일치** `401 Unauthorized`**)**

```json
{
  "message": "Session does not match player."
}
```

- **실패 응답 4 (입력값 부족** `400 Bad Request`**)**

```json
{
  "message": "PlayerId and PlayerName are required."
}
```

- **실패 응답 5 (이미 사용 중인 이름** `409 Conflict`**)**

```json
{
  "message": "PlayerName is already taken."
}
```

- **실패 응답 6** **(플레이어 정보 없음** `404 Not Found` **)**

```json
{
  "message": "Player Not Found"
}
```

- **실패 응답 7 (이미 이름을 등록한 계정** ``409 Conflict`` **)
**
```json
{
  "message": "This account is not New"
}
```

### 📝 플레이어 이름 등록

- **URL**:`POST /api/data/name`
- **헤더**: `Session-Id` (필수) - 유효한 세션 토큰
- **설명**: 로그인한 사용자가 플레이어 이름을 최초로 등록합니다.
이미 등록된 계정은 이름을 등록할 수 없습니다.

- **요청 바디**
```json
{
  "playerId": "admin",
  "playerName": "Admin"
}
```

- **성공 응답 (등록  성공** ``200 OK``**)**

```json
{
  "playerId": "admin",
  "playerName": "Admin"
}
```
- **실패 응답 1 (필수 필드 누락 시** `400 Bad Request`**)**

```json
{
  "message": "PlayerId and PlayerName are required."
}
```

- **실패 응답 2 (이미 사용 중인 플레이어 이름일 경우** `409 Conflict`**)**

```json
{
  "message": "PlayerName is already taken."
}
```

- **실패 응답 3 (기존 계정으로 이름 등록을 시도할 경우** `409 Conflict`**)**

```json
{
  "message": "This account is not New"
}
```

- **실패 응답 4 (세션이 없거나 유효하지 않거나, 세션의 계정과 요청한 playerId가 일치하지 않을 경우** `401 Unauthorized`**)**

```json
{
  "message": "PlayerId and PlayerName are required."
}
```

- **실패 응답 5 (플레이어 ID가 존재하지 않거나 잘못된 경우** `404 Not Found`**)**

```json
{
  "message": "Player Not Found"
}
```
