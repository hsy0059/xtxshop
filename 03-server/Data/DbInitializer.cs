using XtxServer.Entities;

namespace XtxServer.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        // 初始化管理员账号
        if (!context.AdminUsers.Any())
        {
            var admin = new AdminUser
            {
                Id = Guid.NewGuid().ToString("N"),
                Username = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Nickname = "超级管理员",
                Avatar = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/avatar_1.jpg",
                Email = "admin@xtx.com",
                Status = 1,
                Role = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            context.AdminUsers.Add(admin);
        }

        // 初始化分类
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Id = "1001", Name = "生鲜果蔬", Icon = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/nav_icon_1.png", Picture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/category_1.png", Sort = 1, Level = 1 },
                new Category { Id = "1002", Name = "粮油速食", Icon = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/nav_icon_2.png", Picture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/category_2.png", Sort = 2, Level = 1 },
                new Category { Id = "1003", Name = "休闲零食", Icon = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/nav_icon_3.png", Picture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/category_3.png", Sort = 3, Level = 1 },
                new Category { Id = "1004", Name = "肉禽蛋品", Icon = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/nav_icon_4.png", Picture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/category_4.png", Sort = 4, Level = 1 },
                new Category { Id = "1005", Name = "酒水饮料", Icon = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/nav_icon_5.png", Picture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/category_5.png", Sort = 5, Level = 1 },
                new Category { Id = "1006", Name = "家居日用", Icon = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/nav_icon_6.png", Picture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/category_6.png", Sort = 6, Level = 1 },
                new Category { Id = "1007", Name = "美妆护肤", Icon = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/nav_icon_7.png", Picture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/category_7.png", Sort = 7, Level = 1 },
                new Category { Id = "1008", Name = "母婴用品", Icon = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/nav_icon_8.png", Picture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/category_8.png", Sort = 8, Level = 1 },
                new Category { Id = "1009", Name = "数码家电", Icon = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/nav_icon_9.png", Picture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/category_9.png", Sort = 9, Level = 1 },
                new Category { Id = "1010", Name = "运动户外", Icon = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/nav_icon_10.png", Picture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/images/category_10.png", Sort = 10, Level = 1 }
            };
            context.Categories.AddRange(categories);
        }

        // 初始化轮播图
        if (!context.Banners.Any())
        {
            var banners = new List<Banner>
            {
                new Banner { Id = "1", ImgUrl = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/banner_1.jpg", HrefUrl = "/pages/goods/goods?id=1", Type = 1, Sort = 1, Status = 1 },
                new Banner { Id = "2", ImgUrl = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/banner_2.jpg", HrefUrl = "/pages/goods/goods?id=2", Type = 1, Sort = 2, Status = 1 },
                new Banner { Id = "3", ImgUrl = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/banner_3.jpg", HrefUrl = "/pages/goods/goods?id=3", Type = 1, Sort = 3, Status = 1 }
            };
            context.Banners.AddRange(banners);
        }

        // 初始化热门推荐
        if (!context.HotRecommends.Any())
        {
            var hots = new List<HotRecommend>
            {
                new HotRecommend { Id = "1", Title = "特惠推荐", Alt = "精选好物 限时特惠", Target = "/pages/hot/hot?type=1", Pictures = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/hot_1.jpg", Type = "1", Sort = 1, Status = 1 },
                new HotRecommend { Id = "2", Title = "爆款推荐", Alt = "热门爆款 不容错过", Target = "/pages/hot/hot?type=2", Pictures = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/hot_2.jpg", Type = "2", Sort = 2, Status = 1 },
                new HotRecommend { Id = "3", Title = "一站买全", Alt = "一站式购物 省心省力", Target = "/pages/hot/hot?type=3", Pictures = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/hot_3.jpg", Type = "3", Sort = 3, Status = 1 },
                new HotRecommend { Id = "4", Title = "新鲜好物", Alt = "新鲜直达 品质保证", Target = "/pages/hot/hot?type=4", Pictures = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/hot_4.jpg", Type = "4", Sort = 4, Status = 1 }
            };
            context.HotRecommends.AddRange(hots);
        }

        // 初始化商品
        if (!context.Goods.Any())
        {
            var goods = new List<Goods>
            {
                new Goods { Id = "1", Name = "智利进口车厘子", Desc = "JJ级大果 新鲜直达", Price = 99, OldPrice = 129, MainPicture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/goods_1.jpg", Inventory = 100, SalesCount = 50, Status = 1, CategoryId = "1001" },
                new Goods { Id = "2", Name = "新西兰阳光金奇异果", Desc = "12个装 单果约100g", Price = 69, OldPrice = 89, MainPicture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/goods_2.jpg", Inventory = 200, SalesCount = 80, Status = 1, CategoryId = "1001" },
                new Goods { Id = "3", Name = "五常大米", Desc = "东北五常 稻花香2号", Price = 59, OldPrice = 79, MainPicture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/goods_3.jpg", Inventory = 150, SalesCount = 120, Status = 1, CategoryId = "1002" },
                new Goods { Id = "4", Name = "三只松鼠坚果礼盒", Desc = "每日坚果 健康零食", Price = 88, OldPrice = 128, MainPicture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/goods_4.jpg", Inventory = 80, SalesCount = 200, Status = 1, CategoryId = "1003" },
                new Goods { Id = "5", Name = "澳洲进口牛排", Desc = "原切牛排 肉质鲜嫩", Price = 158, OldPrice = 198, MainPicture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/goods_5.jpg", Inventory = 60, SalesCount = 30, Status = 1, CategoryId = "1004" },
                new Goods { Id = "6", Name = "法国红酒", Desc = "波尔多产区 干红葡萄酒", Price = 199, OldPrice = 299, MainPicture = "https://pcapi-xiaotuxian-front-devtest.itheima.net/miniapp/uploads/goods_6.jpg", Inventory = 40, SalesCount = 15, Status = 1, CategoryId = "1005" }
            };
            context.Goods.AddRange(goods);
        }

        await context.SaveChangesAsync();
    }
}
