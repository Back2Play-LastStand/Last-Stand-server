# Last-Stand-server
Last Stand 서버 저장소입니다.

계정 시스템, 인증, 데이터 관리 등 주요 서버 로직을 담당합니다.

## 🛠 기술 스택

- **ASP.NET Core 8**
- **C#**
- **Dapper**
- **BCrypt.Net**
- **MySQL**
- **Redis**
- **REST API**

## 📘 Last Stand API 명세서

### 📂 Account API

### 🔍 플레이어 ID 찾기

- **URL**: `POST /api/account/find-playerid`
- **설명**: 이메일을 통해 플레이어 ID를 조회합니다.
- **요청 바디**:

```json
{
  "email": "test@example.com"
}
```

- **성공 응답**

```json
{
  "playerId": "admin"
}
```

- **실패 응답**

```json
{
  "playerId": null
}
```

- Status Codes:
  - ``200 OK`` - 조회 성공
  - ``404 Not Found`` - 존재하지 않는 이메일

### 🔐 비밀번호 재설정

- **URL**: `PATCH /api/account/reset-password`
- **설명**:  playerId와 이메일이 일치하면 비밀번호를 재설정합니다.
- **요청 바디**:

```json
{
  "playerId": "admin",
  "email": "test@example.com",
  "newPassword": "1234"
}
```

- **성공 응답**

```json
{
  "success": true,
  "message": "Password has been reset successfully."
}
```

- **실패 응답**

```json
{
  "success": false,
  "message": "PlayerId and email do not match."
}
```

- Status Codes:
  - ``200 OK`` - 비밀번호 재설정 성공
  - ``400 Bad Request`` - playerId 또는 이메일 불일치

### 📂 Auth API

### 📝 회원가입

- **URL**: `POST /api/auth/register`
- **설명**: 플레이어 ID와 비밀번호로 계정을 생성합니다
- **요청 바디**:

```json
{
  "playerId": "admin",
  "password": "1234"
}
```

- **성공 응답**

```json
{
  "playerId": "admin",
  "message": "Register Success"
}
```

- **실패 응답**

```json
{
  "playerId": "admin",
  "message": "Id already exists"
}
```

- Status Codes:
  - ``200 OK`` - 등록 성공
  - ``409 Conflict`` - 중복된 아이디

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

- **성공 응답**

```json
{
  "playerId": "admin",
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIs...",
  "message": "Login successful"
}
```
- **실패 응답**

```json
{
  "playerId": "admin",
  "message": "Login Failed"
}
```

- Status Codes:
  - ``200 OK`` - 로그인 성공
  - ``401 Unauthorized`` - 로그인 실패


