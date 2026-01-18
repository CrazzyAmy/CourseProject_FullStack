# 📚 CourseProject - 課程管理系統

這是一個完整的課程管理系統，採用前後端分離架構，使用 ASP.NET Core 8.0 開發，提供課程查詢、預約、會員管理等功能。

## 📁 專案架構

```
CourseProject/
├── CourseCore/                 # 核心商業邏輯層
│   ├── Interface/             # 服務介面定義
│   ├── Service/               # 商業邏輯實作
│   └── Models/                # 資料模型
│
├── CourseDataAccess/          # 資料存取層
│   ├── Repository/            # 資料庫存取實作
│   └── Models/                # Entity Framework 資料模型
│
├── CourseWebAPI/              # RESTful API 後端
│   ├── Controllers/           # API 控制器
│   ├── Models/                # API 請求/回應模型
│   └── output_dir/            
│       └── syft.spdx.json    # SBOM (軟體物料清單)
│
├── CourseWebJSApp/            # MVC 前端應用
│   ├── Controllers/           # MVC 控制器
│   ├── Views/                 # Razor 視圖
│   └── wwwroot/               # 靜態資源
│
├── WebMiniAPI/                # Minimal API 範例專案
│   ├── Interface/             # 服務介面
│   └── Service/               # 服務實作
│
└── CourseConsoleApp/          # 控制台測試應用
    └── Models/                # 測試用資料模型
```

---

## 🏗️ 技術架構

### **後端技術棧**
- **框架**：ASP.NET Core 8.0
- **架構模式**：三層式架構（API → Service → Repository）
- **資料庫**：SQL Server + Entity Framework Core 8.0
- **身份驗證**：JWT (JSON Web Token)
- **API 文件**：Swagger/OpenAPI
- **跨域支援**：CORS

### **前端技術棧**
- **框架**：ASP.NET Core MVC
- **UI 框架**：Bootstrap 5.3.2
- **JavaScript 函式庫**：jQuery 3.7.1
- **通訊方式**：AJAX + RESTful API

### **開發工具**
- **.NET SDK**：8.0.404+
- **IDE**：Visual Studio 2022 / Visual Studio Code
- **套件管理**：NuGet

---

## 🚀 快速開始

### **1. 環境需求**

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) 或更高版本
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB / Express / Developer Edition)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) 或 [VS Code](https://code.visualstudio.com/)

**檢查 .NET 版本：**
```bash
dotnet --version
# 應顯示 8.0.xxx 或更高
```

---

### **2. 複製專案**

```bash
git clone <repository-url>
cd CourseProject
```

---

### **3. 設定資料庫連線**

#### **方式一：使用 User Secrets（推薦，開發環境）**

```bash
# 切換到 API 專案目錄
cd CourseWebAPI

# 初始化 User Secrets
dotnet user-secrets init

# 設定資料庫連線字串（請修改為你的實際資訊）
dotnet user-secrets set "ConnectionStrings:KhNetCourseDB" "Server=localhost;Initial Catalog=KhNetCourse;User ID=courseapp;Password=YourPassword123;Encrypt=True;TrustServerCertificate=True;"

# 設定 JWT 簽名金鑰（建議重新產生）
dotnet user-secrets set "JwtTokenSettings:IssuerSigningKey" "your-new-secret-key-here-at-least-32-characters"

# 驗證設定
dotnet user-secrets list
```

#### **方式二：修改 appsettings.Development.json（不推薦）**

⚠️ **注意：此檔案不應上傳到 Git！**

複製 `appsettings.template.json` 為 `appsettings.Development.json`，並填入實際資訊：

```json
{
  "ConnectionStrings": {
    "KhNetCourseDB": "Server=localhost;Initial Catalog=KhNetCourse;User ID=courseapp;Password=YourPassword;..."
  },
  "JwtTokenSettings": {
    "IssuerSigningKey": "your-secret-key-here"
  }
}
```

---

### **4. 建立資料庫**

```bash
# 切換到 DataAccess 專案（如果有 Migrations）
cd CourseDataAccess

# 建立資料庫遷移
dotnet ef migrations add InitialCreate

# 更新資料庫
dotnet ef database update
```

**或執行 SQL 腳本：**
- 使用 SQL Server Management Studio (SSMS) 執行 `database_script.sql`（如果有提供）

---

### **5. 安裝相依套件**

```bash
# 回到方案根目錄
cd ..

# 還原所有專案的 NuGet 套件
dotnet restore
```

---

### **6. 建置專案**

```bash
dotnet clean
dotnet build
```

**如果遇到 `CreateAppHost` 錯誤（SDK 8.0.404 bug）：**

在以下專案的 `.csproj` 檔案中加入 `<UseAppHost>false</UseAppHost>`：
- CourseWebAPI
- CourseWebJSApp
- WebMiniAPI
- CourseConsoleApp

```xml
<PropertyGroup>
  <TargetFramework>net8.0</TargetFramework>
  <UseAppHost>false</UseAppHost>  <!-- 加入這行 -->
</PropertyGroup>
```

---

### **7. 執行專案**

#### **啟動後端 API**
```bash
cd CourseWebAPI
dotnet run
# API 將運行在 https://localhost:7096
```

#### **啟動前端 MVC 應用**
```bash
cd CourseWebJSApp
dotnet run
# 應用將運行在 https://localhost:7034
```

#### **同時執行多個專案（Visual Studio）**
1. 在方案總管中右鍵點擊方案
2. 選擇「設定啟始專案」
3. 選擇「多個啟始專案」
4. 將 `CourseWebAPI` 和 `CourseWebJSApp` 設定為「啟動」

---

## 📖 專案詳細說明

### **1. CourseWebAPI - RESTful API 後端**

提供完整的 RESTful API 服務，實作 JWT 身份驗證機制。

#### **主要功能模組**

##### **登入驗證 (LoginController)**
```http
POST /api/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}

# 回應
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

##### **課程查詢 (CourseScheduleController)**
```http
# 取得所有課程（公開）
GET /api/CourseSchedule

# 取得特定課程（公開）
GET /api/CourseSchedule/{id}
```

##### **課程預約 (BookingController)**
```http
# 預約課程（需要驗證）
POST /api/Booking
Authorization: Bearer {token}
Content-Type: application/json

{
  "stuId": "guid-student-id",
  "scheduleId": "guid-schedule-id"
}

# 取消預約（需要驗證）
DELETE /api/Booking/{studentScheduleId}
Authorization: Bearer {token}
```

##### **學生資料管理 (StudentController)**
```http
# 更新學生資料（需要驗證）
PUT /api/Student/{stuId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "id": "guid-student-id",
  "name": "張三",
  "phone": "0912345678"
}
```

#### **JWT 設定**

JWT Token 包含以下資訊（Claims）：
- **Sid**: 使用者 ID
- **Name**: 使用者名稱
- **Jti**: Token 唯一識別碼
- **Issuer**: CourseWebApi
- **Audience**: CourseApp
- **Expire**: 30 分鐘

#### **CORS 設定**

允許以下來源的跨域請求：
- `https://localhost:7034` (CourseWebJSApp)
- `https://localhost:9999` (其他前端應用)

---

### **2. CourseWebJSApp - MVC 前端應用**

使用 jQuery + AJAX 與後端 API 互動的單頁應用。

#### **主要頁面**

##### **課程應用頁面 (/Home/CourseApp)**
- 使用者登入
- 查看課程清單
- 預約課程

#### **使用流程**

1. **登入取得 Token**
   - 輸入 Email 和 Password
   - 點擊「登入」按鈕
   - 系統顯示 JWT Token

2. **查看課程清單**
   - 點擊「調用公開 API」
   - 顯示所有可預約課程

3. **預約課程**
   - 輸入學生 ID
   - 點擊課程旁的「報名課程」按鈕
   - 系統自動帶入 Token 完成預約

#### **前端技術實作**

**AJAX 呼叫範例：**
```javascript
// 登入
$.ajax({
    url: 'https://localhost:7096/api/login',
    type: 'POST',
    contentType: 'application/json',
    data: JSON.stringify({ email, password }),
    success: function(response) {
        jwtToken = response.token;
    }
});

// 呼叫受保護的 API
$.ajax({
    url: 'https://localhost:7096/api/booking',
    type: 'POST',
    contentType: 'application/json',
    headers: {
        'Authorization': `Bearer ${jwtToken}`
    },
    data: JSON.stringify(bookingData),
    success: function(response) {
        alert('預約成功');
    }
});
```

---

### **3. WebMiniAPI - Minimal API 範例**

使用 ASP.NET Core Minimal API 實作的簡化版書籍管理系統。

#### **API 端點**
```http
GET    /api/books          # 取得所有書籍
GET    /api/books/{id}     # 取得特定書籍
POST   /api/books          # 新增書籍
PUT    /api/books/{id}     # 更新書籍
DELETE /api/books/{id}     # 刪除書籍
```

#### **架構特色**
- 使用 `IBookService` 介面抽離商業邏輯
- 依賴注入 (Dependency Injection)
- 非同步處理 (async/await)
- Swagger 文件自動產生

---

### **4. CourseCore - 商業邏輯層**

#### **主要服務**

**ICourseScheduleService**
- 課程排程查詢
- 課程資訊管理

**IMemberService**
- 會員登入驗證
- 會員資料管理
- 密碼更新

**IShopService**
- 課程預約
- 預約取消

#### **資料模型**

**MemberModel**
- 會員基本資訊
- 包含密碼加密處理

**CourseScheduleViewModel**
- 課程詳細資訊
- 包含教師、時數、地點等

**StuCourseScheduleModel**
- 學生課程預約記錄

---

### **5. CourseDataAccess - 資料存取層**

使用 **Repository Pattern** 封裝資料庫操作。

#### **主要 Repository**

**IStudentRepository**
- 學生資料 CRUD
- 密碼驗證

**ICourseScheduleRepository**
- 課程排程查詢

**IStuCourseScheduleRepository**
- 課程預約記錄管理

#### **資料庫 Context**
```csharp
public class KhNetCourseContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<CourseSchedule> CourseSchedules { get; set; }
    public DbSet<StuCourseSchedule> StuCourseSchedules { get; set; }
}
```

---

## 🔒 安全性設定

### **1. 敏感資訊管理**

⚠️ **請勿將以下內容提交到 Git：**
- 資料庫連線字串（包含密碼）
- JWT 簽名金鑰
- API Keys

#### **.gitignore 設定**
```gitignore
# 敏感設定檔
appsettings.Development.json
appsettings.Production.json
appsettings.Local.json
appsettings.Secrets.json

# User Secrets
secrets.json

# 環境變數
.env
.env.local
```

### **2. JWT 金鑰建議**

**產生強度高的金鑰：**
```csharp
// 至少 256 位元 (32 字元)
var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
```

**或使用 GUID：**
```bash
# PowerShell
New-Guid

# Linux/macOS
uuidgen
```

### **3. 資料庫帳號權限**

⚠️ **生產環境建議：**
- 不要使用 `sa` 或管理員帳號
- 建立專用帳號並僅授予必要權限

```sql
CREATE LOGIN courseapp WITH PASSWORD = 'StrongPassword123!';
CREATE USER courseapp FOR LOGIN courseapp;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO courseapp;
```

### **4. HTTPS 設定**

- 開發環境：`RequireHttpsMetadata = false`
- **生產環境：必須設為 `true`**

---

## 📦 SBOM (軟體物料清單)

專案包含 SBOM 檔案，記錄所有相依套件：
```
CourseWebAPI/output_dir/syft.spdx.json
```

此檔案可用於：
- 安全性漏洞掃描
- 授權合規檢查
- 供應鏈安全管理

---

## 🛠️ 常見問題排除

### **問題 1：CreateAppHost 錯誤**

**錯誤訊息：**
```
error MSB4018: "CreateAppHost" 工作發生未預期的失敗
Method not found: 'Boolean Microsoft.NET.HostModel.ResourceUpdater.IsSupportedOS()'
```

**解決方法：**
在 `.csproj` 加入：
```xml
<UseAppHost>false</UseAppHost>
```

或更新到最新的 .NET 8 SDK。

---

### **問題 2：套件還原失敗**

**解決方法：**
```bash
# 清除快取
dotnet nuget locals all --clear

# 重新還原
dotnet restore --force
```

---

### **問題 3：JWT Token 驗證失敗**

**檢查項目：**
1. Token 是否正確帶入 `Authorization` Header
2. Token 格式：`Bearer {token}`
3. Token 是否過期（預設 30 分鐘）
4. JWT 設定（Issuer、Audience、SigningKey）是否一致

---

### **問題 4：CORS 錯誤**

**錯誤訊息：**
```
Access to XMLHttpRequest has been blocked by CORS policy
```

**解決方法：**
確認前端 URL 已加入 CORS 白名單：
```csharp
builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigin", builder => {
        builder.WithOrigins("https://localhost:7034")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
```

---

### **問題 5：Nullable 警告 (CS8618)**

**快速修正：**

**方式一：使屬性可為 Null**
```csharp
public string? Email { get; set; }
```

**方式二：使用 required 修飾元**
```csharp
public required string Email { get; set; }
```

**方式三：停用 Nullable 檢查**
```xml
<Nullable>disable</Nullable>
```

---

## 📚 學習資源

### **官方文件**
- [ASP.NET Core 文件](https://learn.microsoft.com/aspnet/core)
- [Entity Framework Core](https://learn.microsoft.com/ef/core)
- [JWT 驗證](https://jwt.io)

### **相關技術**
- [Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- [Repository Pattern](https://learn.microsoft.com/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
- [Dependency Injection](https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection)

---

## 🤝 貢獻指南

1. Fork 本專案
2. 建立功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交變更 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 開啟 Pull Request

---

## 📄 授權

本專案採用 MIT 授權條款 - 詳見 [LICENSE](LICENSE) 檔案

---

## 👥 作者

**課程專案開發團隊**
- ASP.NET Core MVC 網站與雲端開發實作班

---

## 📞 聯絡資訊

如有問題或建議，請透過以下方式聯繫：
- 建立 Issue
- Email: [your-email@example.com]

---

## 🔄 更新日誌

### v1.0.0 (2024-01-19)
- ✨ 初始版本發布
- 🔐 實作 JWT 身份驗證
- 📚 完成課程管理功能
- 🎨 前端 UI 整合
- 📖 完整文件撰寫

---

**祝開發順利！** 🚀
