# 静态图片资源目录

此目录存放小程序所需的本地静态图片资源。

## 需要的图片文件

请准备以下图片文件放入此目录：

| 文件名 | 用途 | 建议尺寸 |
|--------|------|----------|
| `default-avatar.png` | 未登录默认头像 | 200x200px |
| `logo.png` | 登录页Logo | 200x200px |
| `center-bg.png` | 个人中心背景 | 750x400px |
| `order-bg.png` | 订单详情/个人资料背景 | 750x300px |
| `locate.png` | 定位图标 | 50x50px |
| `car.png` | 物流/配送图标 | 50x50px |

## 图片来源

你可以从以下途径获取这些图片：

1. **原服务器下载**：
   - 访问 `https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/`
   - 下载对应的图片文件

2. **自行设计**：
   - 使用设计工具（如Figma、Sketch）设计符合品牌风格的图片
   - 导出为PNG格式

3. **使用免费图库**：
   - [Unsplash](https://unsplash.com)
   - [Pexels](https://pexels.com)
   - [Iconfont](https://www.iconfont.cn) (图标)

## 注意事项

1. 图片建议使用 **PNG** 格式，支持透明背景
2. 背景图片建议使用 **JPG** 格式，文件更小
3. 所有图片建议进行压缩，减少小程序包体积
4. 图标类图片建议使用 **SVG** 格式（如小程序支持）

## 替代方案

如果不想使用本地图片，可以将 `http.ts` 中的 baseURL 改回使用远程服务器：

```typescript
const baseURL = 'https://pcapi-xiaotuxian-front-devtest.itheima.net'
```
