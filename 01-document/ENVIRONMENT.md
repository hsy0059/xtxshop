# 小兔鲜儿项目本地运行环境要求

## 一、后端环境 (.NET Core)

### 1. 必需软件
| 软件 | 版本 | 下载地址 |
|------|------|----------|
| .NET SDK | 8.0+ | https://dotnet.microsoft.com/download/dotnet/8.0 |
| MySQL | 8.0+ | https://dev.mysql.com/downloads/mysql/ |
| Redis | 可选 | https://redis.io/download (Windows可用Memurai) |

### 2. 安装步骤

#### 安装 .NET 8 SDK
```bash
# 验证安装
dotnet --version
# 应显示 8.0.x
```

#### 安装 MySQL
1. 下载 MySQL Installer
2. 选择 "Server only" 安装
3. 设置 root 密码（建议：root）
4. 记住端口号（默认：3306）

#### 创建数据库
```sql
-- 使用 MySQL 命令行或图形工具执行
CREATE DATABASE xtx_shop CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 3. 配置后端

编辑 `03-server/appsettings.Development.json`：
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=xtx_shop;User=root;Password=你的密码;Charset=utf8mb4;",
    "Redis": "localhost:6379"
  }
}
```

### 4. 启动后端
```bash
cd 03-server
dotnet run

# 访问地址
# API: http://localhost:5000
# Swagger文档: http://localhost:5000/swagger
# 健康检查: http://localhost:5000/health
```

---

## 二、管理端环境 (Vue3)

### 1. 必需软件
| 软件 | 版本 | 下载地址 |
|------|------|----------|
| Node.js | 16+ | https://nodejs.org/ |
| npm | 8+ | 随 Node.js 安装 |

### 2. 安装步骤

#### 验证安装
```bash
node --version  # 应显示 v16.x 或更高
npm --version   # 应显示 8.x 或更高
```

### 3. 安装依赖
```bash
cd 04-admin
npm install
```

### 4. 启动管理端
```bash
npm run dev

# 访问地址
# http://localhost:5174

# 默认账号
# 用户名: admin
# 密码: admin123
```

---

## 三、小程序端环境 (UniApp)

### 1. 必需软件
| 软件 | 版本 | 下载地址 |
|------|------|----------|
| Node.js | 16+ | https://nodejs.org/ |
| HBuilderX | 最新 | https://www.dcloud.io/hbuilderx.html |
| 微信开发者工具 | 最新 | https://developers.weixin.qq.com/miniprogram/dev/devtools/download.html |

### 2. 安装步骤

#### 安装依赖
```bash
cd 02-uniapp
npm install --legacy-peer-deps
```

#### 修复 vue-demi (重要)
如果运行时报错 `hasInjectionContext`，需要手动修复：

编辑 `02-uniapp/node_modules/vue-demi/lib/index.mjs`，在文件末尾添加：
```javascript
// Polyfill hasInjectionContext for Vue < 3.3
if (typeof Vue.hasInjectionContext === 'undefined') {
  Vue.hasInjectionContext = function() { return false }
}
export const hasInjectionContext = Vue.hasInjectionContext
```

### 3. 配置小程序

编辑 `02-uniapp/src/utils/http.ts`，确保 baseURL 指向本地后端：
```typescript
const baseURL = 'http://localhost:5000'
```

### 4. 启动小程序

#### 方式一：命令行
```bash
npm run dev:mp-weixin
```

#### 方式二：HBuilderX
1. 打开 HBuilderX
2. 导入 `02-uniapp` 项目
3. 点击 "运行" → "运行到小程序模拟器" → "微信开发者工具"

### 5. 微信开发者工具配置
1. 打开微信开发者工具
2. 导入 `02-uniapp/dist/dev/mp-weixin` 目录
3. 开启 "不校验合法域名"（开发环境）

---

## 四、完整启动流程

### 第一步：启动 MySQL
确保 MySQL 服务正在运行

### 第二步：启动后端
```bash
cd 03-server
dotnet run
```
等待显示 "✅ 数据库初始化成功"

### 第三步：启动管理端（可选）
```bash
cd 04-admin
npm run dev
```

### 第四步：启动小程序
```bash
cd 02-uniapp
npm run dev:mp-weixin
```
然后在微信开发者工具中打开项目

---

## 五、常见问题

### 1. 后端启动失败
**问题**：数据库连接失败  
**解决**：
- 检查 MySQL 是否启动
- 检查连接字符串密码是否正确
- 检查数据库 `xtx_shop` 是否已创建

### 2. 小程序编译失败
**问题**：`hasInjectionContext` 报错  
**解决**：按上文修复 `vue-demi/lib/index.mjs`

### 3. 小程序无法连接后端
**问题**：请求失败或跨域错误  
**解决**：
- 检查后端是否运行在 `http://localhost:5000`
- 检查小程序 `http.ts` 中的 baseURL 配置
- 微信开发者工具开启 "不校验合法域名"
- 确保手机和电脑在同一网络（真机调试）

### 4. 管理端无法登录
**问题**：登录接口 404 或 500  
**解决**：
- 检查后端是否已启动
- 检查 `vite.config.ts` 中的代理配置
- 查看后端日志错误信息

---

## 六、端口占用检查

如果启动失败，可能是端口被占用：

```bash
# 检查端口占用 (Windows)
netstat -ano | findstr :5000
netstat -ano | findstr :5174
netstat -ano | findstr :3306

# 结束进程 (根据 PID)
taskkill /PID <PID> /F
```

---

## 七、推荐开发工具

| 用途 | 推荐工具 |
|------|----------|
| 后端开发 | Visual Studio 2022 / VS Code + C# 扩展 |
| 数据库管理 | Navicat / DBeaver / MySQL Workbench |
| 前端开发 | VS Code + Volar 扩展 |
| 小程序开发 | HBuilderX + 微信开发者工具 |
| API 测试 | Postman / Swagger UI |
