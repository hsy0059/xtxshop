# 小兔鲜儿电商项目

本项目是一个完整的电商系统，包括小程序端、后端 API 和管理后台。

## 项目结构

```
xtx/
├── 02-uniapp/          # 小程序端 (UniApp + Vue3 + TypeScript)
├── 03-server/          # 后端 API (.NET Core 8 + MySQL + Redis)
└── 04-admin/           # 管理后台 (Vue3 + TypeScript + Element Plus)
```

## 后端 API (03-server)

### 技术栈
- .NET Core 8
- Entity Framework Core + Pomelo.MySql
- JWT 认证
- Redis 缓存
- Swagger API 文档

### 项目结构
```
03-server/
├── Controllers/        # API 控制器
│   ├── Admin/         # 管理端 API
│   └── ...            # 小程序端 API
├── Entities/          # 数据库实体
├── DTOs/              # 数据传输对象
├── Services/          # 业务服务层
├── Data/              # 数据库上下文
├── Middleware/        # 中间件
└── Mappings/          # 对象映射
```

### 主要功能模块
1. **用户模块**: 登录、注册、个人信息管理
2. **商品模块**: 商品列表、详情、分类
3. **购物车模块**: 添加、删除、修改购物车
4. **订单模块**: 创建订单、订单列表、订单详情、物流查询
5. **地址模块**: 收货地址管理
6. **支付模块**: 微信支付、模拟支付
7. **首页模块**: Banner、分类、热门推荐、猜你喜欢

### 运行方式

1. 确保已安装 .NET 8 SDK
2. 配置数据库连接字符串 (appsettings.json)
3. 运行迁移命令创建数据库:
```bash
cd 03-server
dotnet ef migrations add InitialCreate
dotnet ef database update
```
4. 运行项目:
```bash
dotnet run
```

API 地址: `http://localhost:5000`
Swagger 文档: `http://localhost:5000/swagger`

## 管理后台 (04-admin)

### 技术栈
- Vue 3
- TypeScript
- Element Plus
- Pinia (状态管理)
- Vue Router
- ECharts (图表)

### 项目结构
```
04-admin/
├── src/
│   ├── api/           # API 接口
│   ├── components/    # 公共组件
│   ├── views/         # 页面视图
│   ├── router/        # 路由配置
│   ├── stores/        # Pinia 状态管理
│   ├── utils/         # 工具函数
│   └── types/         # TypeScript 类型定义
```

### 主要功能
1. **数据概览**: 统计数据、订单分布、销售趋势图表
2. **商品管理**: 商品列表、分类管理
3. **订单管理**: 订单列表、订单详情、发货处理
4. **用户管理**: 用户列表、用户详情
5. **营销管理**: 轮播图管理、热门推荐

### 运行方式

1. 确保已安装 Node.js 16+
2. 安装依赖:
```bash
cd 04-admin
npm install
```
3. 运行开发服务器:
```bash
npm run dev
```

管理后台地址: `http://localhost:5174`

## API 接口说明

### 请求头要求
小程序端调用需要在 Header 中添加:
```
source-client: miniapp
```

需要认证的接口需要在 Header 中添加:
```
Authorization: Bearer {token}
```

### 主要接口列表

#### 登录模块
- POST `/login/wxMin` - 小程序登录
- POST `/login/wxMin/simple` - 小程序登录(内测版)

#### 首页模块
- GET `/home/banner` - 获取轮播图
- GET `/home/category/mutli` - 获取分类
- GET `/home/hot/mutli` - 获取热门推荐
- GET `/home/goods/guessLike` - 猜你喜欢

#### 商品模块
- GET `/goods?id={id}` - 商品详情
- GET `/category/top` - 分类列表

#### 用户模块
- GET `/member/profile` - 获取个人信息
- PUT `/member/profile` - 修改个人信息

#### 购物车模块
- GET `/member/cart` - 购物车列表
- POST `/member/cart` - 添加购物车
- PUT `/member/cart/{skuId}` - 修改购物车
- DELETE `/member/cart` - 删除购物车

#### 订单模块
- GET `/member/order/pre` - 获取预付订单
- POST `/member/order` - 创建订单
- GET `/member/order` - 订单列表
- GET `/member/order/{id}` - 订单详情

#### 地址模块
- GET `/member/address` - 地址列表
- POST `/member/address` - 添加地址
- PUT `/member/address/{id}` - 修改地址
- DELETE `/member/address/{id}` - 删除地址

## 数据库配置

### MySQL 连接字符串
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=xtx_shop;User=root;Password=your_password;"
}
```

### Redis 连接字符串
```json
"ConnectionStrings": {
  "Redis": "localhost:6379"
}
```

## 部署说明

### 后端部署
1. 发布项目:
```bash
dotnet publish -c Release
```
2. 配置反向代理 (Nginx/IIS)
3. 配置环境变量

### 管理后台部署
1. 构建项目:
```bash
npm run build
```
2. 将 `dist` 目录部署到 Web 服务器

## 开发团队

- 前端: Vue3 + UniApp
- 后端: .NET Core
- 数据库: MySQL + Redis
