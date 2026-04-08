

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Seeds;

// Seed dữ liệu banner/item gacha cho PostgreSQL.
public static class GachaSeed
{
    /// <summary>
    /// Seed banner gacha nếu chưa tồn tại.
    /// Luồng xử lý: lấy mã banner hiện có, duyệt danh sách seed, bỏ qua banner đã có và chỉ save khi có bản ghi mới.
    /// </summary>
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        var existingBannerCodes = await context.GachaBanners
            .Select(b => b.Code)
            .ToListAsync();

        var bannersToSeed = GetBanners();
        var addedCount = 0;

        foreach (var banner in bannersToSeed)
        {
            if (existingBannerCodes.Contains(banner.Code))
            {
                Console.WriteLine($"[GachaSeed] Banner {banner.Code} already exists. Skipping.");
                continue;
                // Không seed lại banner đã có để tránh vi phạm unique code.
            }

            Console.WriteLine($"[GachaSeed] Adding new banner: {banner.Code}");
            context.GachaBanners.Add(banner);
            var items = GetBannerItems(banner);
            context.GachaBannerItems.AddRange(items);
            addedCount++;
        }

        if (addedCount > 0)
        {
            Console.WriteLine($"[GachaSeed] Saving {addedCount} new banners to database...");
            await context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("[GachaSeed] No new banners to add.");
        }
    }

    /// <summary>
    /// Tạo danh sách banner seed mặc định.
    /// Luồng xử lý: khởi tạo banner tiêu chuẩn với cấu hình pity.
    /// </summary>
    public static List<GachaBanner> GetBanners()
    {
        var banners = new List<GachaBanner>();

        var standardBanner = new GachaBanner(
            code: "standard_banner",
            nameVi: "Vòng Quay Tiêu Chuẩn",
            nameEn: "Standard Banner",
            costDiamond: 5,
            oddsVersion: "v1.0",
            effectiveFrom: DateTime.UtcNow.AddDays(-1),
            effectiveTo: null,
            pityEnabled: true,
            hardPityCount: 90
        );
        // effectiveFrom lùi 1 ngày để banner active ngay sau khi seed.

        banners.Add(standardBanner);

        return banners;
    }

    /// <summary>
    /// Tạo danh sách item cho banner tiêu chuẩn.
    /// Luồng xử lý: khai báo reward theo từng rarity với trọng số drop tương ứng.
    /// </summary>
    public static List<GachaBannerItem> GetBannerItems(GachaBanner standardBanner)
    {
        var items = new List<GachaBannerItem>();

        // Nhóm Common.
        items.Add(new GachaBannerItem(standardBanner.Id, GachaRarity.Common, GachaRewardType.Gold, "50", 3000, "50 Vàng", "50 Gold", "gold_50.png"));
        items.Add(new GachaBannerItem(standardBanner.Id, GachaRarity.Common, GachaRewardType.Gold, "100", 2500, "100 Vàng", "100 Gold", "gold_100.png"));
        items.Add(new GachaBannerItem(standardBanner.Id, GachaRarity.Common, GachaRewardType.Gold, "200", 1000, "200 Vàng", "200 Gold", "gold_200.png"));

        // Nhóm Rare.
        items.Add(new GachaBannerItem(standardBanner.Id, GachaRarity.Rare, GachaRewardType.Gold, "500", 1500, "500 Vàng", "500 Gold", "gold_500.png"));
        items.Add(new GachaBannerItem(standardBanner.Id, GachaRarity.Rare, GachaRewardType.Diamond, "10", 1000, "10 Kim Cương", "10 Diamonds", "diamond_10.png"));

        // Nhóm Epic.
        items.Add(new GachaBannerItem(standardBanner.Id, GachaRarity.Epic, GachaRewardType.Diamond, "50", 600, "50 Kim Cương", "50 Diamonds", "diamond_50.png"));
        items.Add(new GachaBannerItem(standardBanner.Id, GachaRarity.Epic, GachaRewardType.Title, "lucky_star", 200, "Danh hiệu: Ngôi Sao May Mắn", "Title: Lucky Star", "title_lucky.png"));

        // Nhóm Legendary.
        items.Add(new GachaBannerItem(standardBanner.Id, GachaRarity.Legendary, GachaRewardType.Diamond, "500", 150, "500 Kim Cương", "500 Diamonds", "diamond_500.png"));
        items.Add(new GachaBannerItem(standardBanner.Id, GachaRarity.Legendary, GachaRewardType.Title, "gacha_master", 50, "Danh hiệu: Bàn Tay Vàng", "Title: Gacha Master", "title_master.png"));

        return items;
    }
}
