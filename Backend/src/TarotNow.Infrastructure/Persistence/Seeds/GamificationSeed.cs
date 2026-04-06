using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TarotNow.Infrastructure.Persistence.Seeds;

public static class GamificationSeed
{
    public static async Task SeedAsync(MongoDbContext context)
    {
        await context.Quests.DeleteManyAsync(x => x.Code == "daily_checkin");

        // 1. Seed Quests (Upsert by Code using $set)
        var initialQuests = new List<QuestDefinitionDocument>
        {
            new() {
                Code = "daily_reading_1",
                TitleVi = "Bói Bài Mỗi Ngày",
                TitleEn = "Daily Reading",
                DescriptionVi = "Hoàn thành 1 trải bài bất kỳ trong ngày hôm nay.",
                DescriptionEn = "Complete 1 tarot reading today.",
                QuestType = "daily",
                TriggerEvent = "ReadingCompleted",
                Target = 1,
                Rewards = new List<QuestRewardItem> { new() { Type = "gold", Amount = 100 } }
            },
            new() {
                Code = "weekly_reading_7",
                TitleVi = "Học Giả Tarot",
                TitleEn = "Tarot Scholar",
                DescriptionVi = "Hoàn thành tổng cộng 7 trải bài trong tuần này.",
                DescriptionEn = "Complete a total of 7 readings this week.",
                QuestType = "weekly",
                TriggerEvent = "ReadingCompleted",
                Target = 7,
                Rewards = new List<QuestRewardItem> { new() { Type = "gold", Amount = 1000 }, new() { Type = "diamond", Amount = 5 } }
            },
            new() {
                Code = "daily_post_1",
                TitleVi = "Chia Sẻ Câu Chuyện",
                TitleEn = "Share a Story",
                DescriptionVi = "Đăng một bài viết mới lên cộng đồng hôm nay để nhận thưởng.",
                DescriptionEn = "Post a new story to the community today to get reward.",
                QuestType = "daily",
                TriggerEvent = "PostCreated",
                Target = 1,
                Rewards = new List<QuestRewardItem> { new() { Type = "gold", Amount = 150 } }
            }
        };

        foreach (var q in initialQuests)
        {
            var filter = Builders<QuestDefinitionDocument>.Filter.Eq(x => x.Code, q.Code);
            var update = Builders<QuestDefinitionDocument>.Update
                .Set(x => x.TitleVi, q.TitleVi)
                .Set(x => x.TitleEn, q.TitleEn)
                .Set(x => x.DescriptionVi, q.DescriptionVi)
                .Set(x => x.DescriptionEn, q.DescriptionEn)
                .Set(x => x.QuestType, q.QuestType)
                .Set(x => x.TriggerEvent, q.TriggerEvent)
                .Set(x => x.Target, q.Target)
                .Set(x => x.Rewards, q.Rewards)
                .Set(x => x.IsActive, true);
            
            await context.Quests.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        // 2. Seed Achievements (Upsert by Code)
        var initialAchs = new List<AchievementDefinitionDocument>
        {
            new() {
                Code = "first_reading",
                TitleVi = "Bước Chân Đầu Tiên",
                TitleEn = "First Steps",
                DescriptionVi = "Hoàn thành trải bài tarot đầu tiên của bạn.",
                DescriptionEn = "Complete your very first tarot reading."
            },
            new() {
                Code = "streak_7",
                TitleVi = "Kiên Trì Bền Bỉ",
                TitleEn = "Persistent Spirit",
                DescriptionVi = "Đạt chuỗi điểm danh 7 ngày liên tiếp.",
                DescriptionEn = "Reach a 7-day check-in streak.",
                GrantsTitleCode = "title_persistent"
            }
        };

        foreach (var a in initialAchs)
        {
            var filter = Builders<AchievementDefinitionDocument>.Filter.Eq(x => x.Code, a.Code);
            var update = Builders<AchievementDefinitionDocument>.Update
                .Set(x => x.TitleVi, a.TitleVi)
                .Set(x => x.TitleEn, a.TitleEn)
                .Set(x => x.DescriptionVi, a.DescriptionVi)
                .Set(x => x.DescriptionEn, a.DescriptionEn)
                .Set(x => x.GrantsTitleCode, a.GrantsTitleCode);
            
            await context.Achievements.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        // 3. Seed Titles (Upsert by Code)
        var initialTitles = new List<TitleDefinitionDocument>
        {
            new() {
                Code = "title_newbie",
                NameVi = "Tập Sự",
                NameEn = "Novice",
                Rarity = "Common"
            },
            new() {
                Code = "title_persistent",
                NameVi = "Người Kiên Trì",
                NameEn = "The Persistent",
                Rarity = "Rare"
            },
            new() {
                Code = "title_legend",
                NameVi = "Huyền Thoại Tarot",
                NameEn = "Tarot Legend",
                Rarity = "Legendary"
            },
            new() {
                Code = "title_seeker",
                NameVi = "Kẻ Tìm Kiếm",
                NameEn = "Truth Seeker",
                Rarity = "Common"
            },
            new() {
                Code = "title_oracle",
                NameVi = "Nhà Tiên Tri",
                NameEn = "The Oracle",
                Rarity = "Epic"
            },
            new() {
                Code = "title_master",
                NameVi = "Bậc Thầy Bói Bài",
                NameEn = "Tarot Master",
                Rarity = "Legendary"
            },
            new() {
                Code = "title_owl",
                NameVi = "Cú Đêm Mất Ngủ",
                NameEn = "Night Owl",
                Rarity = "Rare"
            }
        };

        foreach (var t in initialTitles)
        {
            var filter = Builders<TitleDefinitionDocument>.Filter.Eq(x => x.Code, t.Code);
            var update = Builders<TitleDefinitionDocument>.Update
                .Set(x => x.NameVi, t.NameVi)
                .Set(x => x.NameEn, t.NameEn)
                .Set(x => x.Rarity, t.Rarity);
            
            await context.Titles.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
    }
}
