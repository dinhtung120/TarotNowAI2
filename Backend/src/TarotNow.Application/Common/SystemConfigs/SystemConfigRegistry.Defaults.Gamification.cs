namespace TarotNow.Application.Common.SystemConfigs;

public static partial class SystemConfigRegistry
{
    private static object[] BuildDefaultQuestDefinitions()
    {
        return
        [
            BuildDailyReadingQuest(),
            BuildWeeklyReadingQuest(),
            BuildDailyPostQuest()
        ];
    }

    private static object BuildDailyReadingQuest()
    {
        return new
        {
            code = "daily_reading_1",
            titleVi = "Bói Bài Mỗi Ngày",
            titleEn = "Daily Reading",
            titleZh = "每日占卜",
            descriptionVi = "Hoàn thành 1 trải bài bất kỳ trong ngày hôm nay.",
            descriptionEn = "Complete 1 tarot reading today.",
            descriptionZh = "今天完成 1 次塔罗占卜。",
            questType = "daily",
            triggerEvent = "ReadingCompleted",
            target = 1,
            isActive = true,
            rewards = new[] { QuestReward("gold", 100) }
        };
    }

    private static object BuildWeeklyReadingQuest()
    {
        return new
        {
            code = "weekly_reading_7",
            titleVi = "Học Giả Tarot",
            titleEn = "Tarot Scholar",
            titleZh = "塔罗学者",
            descriptionVi = "Hoàn thành tổng cộng 7 trải bài trong tuần này.",
            descriptionEn = "Complete a total of 7 readings this week.",
            descriptionZh = "本周完成共 7 次占卜。",
            questType = "weekly",
            triggerEvent = "ReadingCompleted",
            target = 7,
            isActive = true,
            rewards = new[] { QuestReward("gold", 1000), QuestReward("diamond", 5) }
        };
    }

    private static object BuildDailyPostQuest()
    {
        return new
        {
            code = "daily_post_1",
            titleVi = "Chia Sẻ Câu Chuyện",
            titleEn = "Share a Story",
            titleZh = "分享故事",
            descriptionVi = "Đăng một bài viết mới lên cộng đồng hôm nay để nhận thưởng.",
            descriptionEn = "Post a new story to the community today to get reward.",
            descriptionZh = "今天发布一篇社区帖子即可获得奖励。",
            questType = "daily",
            triggerEvent = "PostCreated",
            target = 1,
            isActive = true,
            rewards = new[] { QuestReward("gold", 150) }
        };
    }

    private static object QuestReward(string type, int amount)
    {
        return new
        {
            type,
            amount,
            titleCode = (string?)null
        };
    }

    private static object[] BuildDefaultAchievementDefinitions()
    {
        return
        [
            new
            {
                code = "first_reading",
                titleVi = "Lần Đầu Giải Bài",
                titleEn = "First Reading",
                titleZh = "首次解牌",
                descriptionVi = "Hoàn thành phiên giải bài đầu tiên của bạn.",
                descriptionEn = "Complete your first reading session.",
                descriptionZh = "完成你的首次解牌。",
                icon = "ach-first-reading",
                grantsTitleCode = "novice_reader",
                isActive = true
            }
        ];
    }

    private static object[] BuildDefaultTitleDefinitions()
    {
        return
        [
            new
            {
                code = "novice_reader",
                nameVi = "Người Tập Sự",
                nameEn = "Novice Reader",
                nameZh = "新手占卜师",
                descriptionVi = "Danh hiệu dành cho người mới bắt đầu con đường tâm linh.",
                descriptionEn = "Title for first-time seekers.",
                descriptionZh = "送给初入灵性之路的新手称号。",
                rarity = "Common",
                isActive = true
            }
        ];
    }
}
