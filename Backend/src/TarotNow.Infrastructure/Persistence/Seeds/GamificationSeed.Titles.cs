using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Seeds;

public static partial class GamificationSeed
{
    private static async Task SeedTitleDefinitionsAsync(MongoDbContext context)
    {
        foreach (var title in BuildInitialTitles())
        {
            var filter = Builders<TitleDefinitionDocument>.Filter.Eq(x => x.Code, title.Code);
            var update = Builders<TitleDefinitionDocument>.Update
                .Set(x => x.NameVi, title.NameVi)
                .Set(x => x.NameEn, title.NameEn)
                .Set(x => x.Rarity, title.Rarity);

            await context.Titles.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
    }

    private static List<TitleDefinitionDocument> BuildInitialTitles()
    {
        return new List<TitleDefinitionDocument>
        {
            new() { Code = "title_newbie", NameVi = "Tập Sự", NameEn = "Novice", Rarity = "Common" },
            new() { Code = "title_persistent", NameVi = "Người Kiên Trì", NameEn = "The Persistent", Rarity = "Rare" },
            new() { Code = "title_legend", NameVi = "Huyền Thoại Tarot", NameEn = "Tarot Legend", Rarity = "Legendary" },
            new() { Code = "title_seeker", NameVi = "Kẻ Tìm Kiếm", NameEn = "Truth Seeker", Rarity = "Common" },
            new() { Code = "title_oracle", NameVi = "Nhà Tiên Tri", NameEn = "The Oracle", Rarity = "Epic" },
            new() { Code = "title_master", NameVi = "Bậc Thầy Bói Bài", NameEn = "Tarot Master", Rarity = "Legendary" },
            new() { Code = "title_owl", NameVi = "Cú Đêm Mất Ngủ", NameEn = "Night Owl", Rarity = "Rare" }
        };
    }
}
