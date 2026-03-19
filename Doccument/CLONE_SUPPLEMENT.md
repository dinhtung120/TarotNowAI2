# 📎 PHỤ LỤC: DỮ LIỆU & CẤU HÌNH CẦN THIẾT ĐỂ DỰ ÁN CHẠY ĐƯỢC

> **File này bổ sung cho `CLONE_PROJECT_GUIDE.md` và `CODING_ORDER_DETAIL.md`.**  
> Chứa những nội dung **copy-paste trực tiếp** mà 2 file kia chỉ mô tả chứ chưa cung cấp.

---

## MỤC LỤC

1. [File `appsettings.json` hoàn chỉnh](#1-appsettingsjson-hoàn-chỉnh)
2. [File `appsettings.Development.json`](#2-appsettingsdevelopmentjson)
3. [AI Prompt mẫu (System + User)](#3-ai-prompt-mẫu)
4. [Seed 78 lá bài Tarot vào MongoDB](#4-seed-78-lá-bài-tarot)
5. [Seed tài khoản SuperAdmin](#5-seed-tài-khoản-superadmin)
6. [File `middleware.ts` cho Next.js i18n](#6-middlewarets-nextjs-i18n)
7. [File Frontend cấu hình: `next.config.ts`, `postcss.config.mjs`](#7-cấu-hình-frontend)

---

## 1. `appsettings.json` HOÀN CHỈNH

> Đặt tại: `Backend/src/TarotNow.Api/appsettings.json`  
> ⚠️ Thay `REPLACE_...` bằng giá trị thật trước khi deploy production.

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Database=tarotweb;Username=tarot_user;Password=tarot_dev_pass",
    "MongoDB": "mongodb://localhost:27017/tarotweb",
    "Redis": "localhost:6379"
  },
  "Jwt": {
    "SecretKey": "PHUONG_AN_B_JWT_SECRET_KEY_REPLACE_ME_IN_PRODUCTION",
    "Issuer": "TarotNowAI",
    "Audience": "TarotNowAIUsers",
    "ExpiryMinutes": 60,
    "RefreshExpiryDays": 7
  },
  "PaymentGateway": {
    "WebhookSecret": "REPLACE_WITH_STRONG_WEBHOOK_SECRET"
  },
  "Security": {
    "MfaEncryptionKey": "REPLACE_WITH_STRONG_MFA_ENCRYPTION_KEY"
  },
  "Redis": {
    "ConnectionString": "localhost:6379",
    "InstanceName": "TarotNow:"
  },
  "AiProvider": {
    "ApiKey": "REPLACE_WITH_REAL_KEY_OR_USE_ENV_VAR",
    "BaseUrl": "https://api.openai.com/v1/",
    "Model": "gpt-4o-mini",
    "TimeoutSeconds": 30,
    "MaxRetries": 2
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information",
        "Microsoft.EntityFrameworkCore": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}"
        }
      }
    ],
    "Enrich": ["FromLogContext", "WithMachineName", "WithThreadId"]
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:3000"]
  },
  "ApiVersion": {
    "DefaultVersion": "1.0",
    "ReportApiVersions": true
  },
  "SystemConfig": {
    "DailyFreeGold": 5,
    "DailyAiQuota": 3,
    "InFlightAiCap": 3,
    "ReadingRateLimitSeconds": 30
  },
  "AllowedHosts": "*"
}
```

**Giải thích từng mục:**

| Mục | Ý nghĩa |
|-----|---------|
| `ConnectionStrings.PostgreSQL` | Kết nối PostgreSQL — đúng user/pass đã tạo ở Bước 2.1 |
| `ConnectionStrings.MongoDB` | Kết nối MongoDB — mặc định localhost |
| `ConnectionStrings.Redis` | Kết nối Redis — mặc định localhost:6379 |
| `Jwt.SecretKey` | **BẮT BUỘC ≥ 32 ký tự.** Dùng để ký JWT token. Dev key đã sẵn ở Development.json |
| `Jwt.ExpiryMinutes` | Access token hết hạn sau 60 phút |
| `Jwt.RefreshExpiryDays` | Refresh token hết hạn sau 7 ngày |
| `AiProvider.ApiKey` | OpenAI API key — lấy từ https://platform.openai.com |
| `AiProvider.Model` | Model AI sử dụng — `gpt-4o-mini` (rẻ, nhanh) |
| `AiProvider.TimeoutSeconds` | Timeout mỗi request AI — 30 giây |
| `AiProvider.MaxRetries` | Retry tối đa 2 lần nếu lỗi mạng |
| `SystemConfig.DailyFreeGold` | Mỗi ngày user được tặng 5 Gold miễn phí |
| `SystemConfig.DailyAiQuota` | Mỗi ngày được hỏi AI tối đa 3 lần |
| `SystemConfig.InFlightAiCap` | Tối đa 3 request AI chạy cùng lúc |
| `SystemConfig.ReadingRateLimitSeconds` | Giãn cách tối thiểu 30 giây giữa 2 lần rút bài |

---

## 2. `appsettings.Development.json`

> Đặt tại: `Backend/src/TarotNow.Api/appsettings.Development.json`  
> File này **override** `appsettings.json` khi chạy dev.

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=tarotweb;Username=tarot_user;Password=tarot_dev_pass",
    "MongoDB": "mongodb://localhost:27017/tarotweb",
    "Redis": "localhost:6379"
  },
  "Jwt": {
    "SecretKey": "TarotNow_Development_Secret_Key_Do_Not_Use_In_Production_12345"
  },
  "PaymentGateway": {
    "WebhookSecret": "TarotNow_Dev_WebhookSecret_2026"
  },
  "Security": {
    "MfaEncryptionKey": "TarotNow_Dev_MfaEncryption_2026"
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Debug",
        "Microsoft.EntityFrameworkCore": "Information",
        "Microsoft.EntityFrameworkCore.Database.Command": "Information"
      }
    }
  }
}
```

**Khác biệt so với production:**
- `Jwt.SecretKey` dùng key dev (>32 ký tự, an toàn cho local)
- `Serilog.MinimumLevel` = Debug (log chi tiết hơn)
- `PaymentGateway.WebhookSecret` dùng key test

---

## 3. AI PROMPT MẪU

> AI cần 2 loại prompt: **System Prompt** (ai đóng vai gì) và **User Prompt** (hỏi gì).

### System Prompt (vai trò AI)

```
You are a mystical, wise, and empathetic Tarot Reader. Format your response clearly using Markdown.
```

> **Giải thích:** Đây là prompt ngắn nhưng đủ. AI sẽ đóng vai thầy bói Tarot, trả lời bằng Markdown (để FE render đẹp). Nếu muốn thay đổi phong cách → sửa đoạn này.

### User Prompt — Giải bài lần đầu

```
Interpret this reading for me. Spread Type: {spreadType}. Cards Chosen: {cardsJson}
```

**Ví dụ thực tế:**
```
Interpret this reading for me. Spread Type: three_card. Cards Chosen: [{"code":"the_fool","position":0,"isReversed":false},{"code":"the_magician","position":1,"isReversed":true},{"code":"the_high_priestess","position":2,"isReversed":false}]
```

### User Prompt — Follow-up (hỏi thêm)

```
Based on my previous reading (Spread: {spreadType}, Cards: {cardsJson}), answer my follow-up question: {followupQuestion}
```

**Ví dụ:**
```
Based on my previous reading (Spread: three_card, Cards: [...]), answer my follow-up question: Lá The Fool ngược nghĩa là tôi nên cẩn thận điều gì?
```

### Nơi đặt prompt trong code

```
File: Application/Features/Reading/Commands/StreamReading/StreamReadingCommand.cs
Dòng: ~189-194

string systemPrompt = "You are a mystical, wise, and empathetic Tarot Reader. Format your response clearly using Markdown.";

string userPrompt = string.IsNullOrWhiteSpace(request.FollowupQuestion)
    ? $"Interpret this reading for me. Spread Type: {session.SpreadType}. Cards Chosen: {session.CardsDrawn}"
    : $"Based on my previous reading (Spread: {session.SpreadType}, Cards: {session.CardsDrawn}), answer my follow-up question: {request.FollowupQuestion}";
```

> 💡 **Mẹo cải thiện prompt:** Thêm ngôn ngữ `"Respond in {locale} language."` vào cuối systemPrompt để AI trả lời đúng ngôn ngữ. Thêm `"You must interpret each card's position (past/present/future for three_card spread)."` để giải thích theo vị trí.

---

## 4. SEED 78 LÁ BÀI TAROT

> **Bắt buộc phải chạy** trước khi dùng tính năng Rút bài. Nếu không có dữ liệu → `ICardsCatalogRepository.GetAllAsync()` trả mảng rỗng → không rút được.

### Cách 1: Chạy script MongoDB shell (Nhanh nhất)

Tạo file `seed_cards.js` rồi chạy: `mongosh tarotweb seed_cards.js`

```javascript
// seed_cards.js — Seed 78 lá bài Tarot vào collection card_catalog
// Chạy: mongosh tarotweb seed_cards.js

db.card_catalog.drop(); // Xóa cũ nếu có

// ===== 22 LÁ MAJOR ARCANA (0-21) =====
const majorArcana = [
  { _id: 0, code: "the_fool", name: { vi: "Kẻ Ngốc", en: "The Fool", zh: "愚者" }, arcana: "major", suit: null, number: 0, element: "air",
    meanings: { upright: { vi: "Khởi đầu mới, tự do, hồn nhiên", en: "New beginnings, freedom, innocence", zh: "新开始、自由、天真" }, reversed: { vi: "Liều lĩnh, thiếu suy nghĩ", en: "Recklessness, lack of thought", zh: "鲁莽、缺乏思考" } } },
  { _id: 1, code: "the_magician", name: { vi: "Pháp Sư", en: "The Magician", zh: "魔术师" }, arcana: "major", suit: null, number: 1, element: "air",
    meanings: { upright: { vi: "Quyền năng, kỹ năng, tập trung", en: "Power, skill, concentration", zh: "力量、技能、专注" }, reversed: { vi: "Thao túng, lừa dối", en: "Manipulation, deceit", zh: "操纵、欺骗" } } },
  { _id: 2, code: "the_high_priestess", name: { vi: "Nữ Tư Tế", en: "The High Priestess", zh: "女祭司" }, arcana: "major", suit: null, number: 2, element: "water",
    meanings: { upright: { vi: "Trực giác, bí ẩn, tiềm thức", en: "Intuition, mystery, subconscious", zh: "直觉、神秘、潜意识" }, reversed: { vi: "Bí mật, rút lui", en: "Secrets, withdrawal", zh: "秘密、退缩" } } },
  { _id: 3, code: "the_empress", name: { vi: "Hoàng Hậu", en: "The Empress", zh: "皇后" }, arcana: "major", suit: null, number: 3, element: "earth",
    meanings: { upright: { vi: "Phong phú, nuôi dưỡng, thiên nhiên", en: "Abundance, nurturing, nature", zh: "丰盛、养育、自然" }, reversed: { vi: "Phụ thuộc, nghèo nàn sáng tạo", en: "Dependence, creative block", zh: "依赖、缺乏创造力" } } },
  { _id: 4, code: "the_emperor", name: { vi: "Hoàng Đế", en: "The Emperor", zh: "皇帝" }, arcana: "major", suit: null, number: 4, element: "fire",
    meanings: { upright: { vi: "Quyền lực, cấu trúc, ổn định", en: "Authority, structure, stability", zh: "权威、结构、稳定" }, reversed: { vi: "Độc đoán, cứng nhắc", en: "Tyranny, rigidity", zh: "专制、僵化" } } },
  { _id: 5, code: "the_hierophant", name: { vi: "Giáo Hoàng", en: "The Hierophant", zh: "教皇" }, arcana: "major", suit: null, number: 5, element: "earth",
    meanings: { upright: { vi: "Truyền thống, đạo đức, hướng dẫn", en: "Tradition, morality, guidance", zh: "传统、道德、指导" }, reversed: { vi: "Nổi loạn, thách thức quy ước", en: "Rebellion, challenging convention", zh: "叛逆、挑战惯例" } } },
  { _id: 6, code: "the_lovers", name: { vi: "Người Yêu", en: "The Lovers", zh: "恋人" }, arcana: "major", suit: null, number: 6, element: "air",
    meanings: { upright: { vi: "Tình yêu, hài hòa, lựa chọn", en: "Love, harmony, choices", zh: "爱情、和谐、选择" }, reversed: { vi: "Bất hòa, mất cân bằng", en: "Disharmony, imbalance", zh: "不和谐、失衡" } } },
  { _id: 7, code: "the_chariot", name: { vi: "Cỗ Xe", en: "The Chariot", zh: "战车" }, arcana: "major", suit: null, number: 7, element: "water",
    meanings: { upright: { vi: "Chiến thắng, ý chí, quyết tâm", en: "Victory, willpower, determination", zh: "胜利、意志力、决心" }, reversed: { vi: "Mất phương hướng, thiếu kiểm soát", en: "Lack of direction, lack of control", zh: "迷失方向、缺乏控制" } } },
  { _id: 8, code: "strength", name: { vi: "Sức Mạnh", en: "Strength", zh: "力量" }, arcana: "major", suit: null, number: 8, element: "fire",
    meanings: { upright: { vi: "Dũng cảm, kiên nhẫn, lòng trắc ẩn", en: "Courage, patience, compassion", zh: "勇气、耐心、慈悲" }, reversed: { vi: "Yếu đuối, nghi ngờ bản thân", en: "Weakness, self-doubt", zh: "虚弱、自我怀疑" } } },
  { _id: 9, code: "the_hermit", name: { vi: "Ẩn Sĩ", en: "The Hermit", zh: "隐士" }, arcana: "major", suit: null, number: 9, element: "earth",
    meanings: { upright: { vi: "Suy ngẫm, tìm kiếm nội tâm", en: "Introspection, inner search", zh: "内省、内心探索" }, reversed: { vi: "Cô lập, cô đơn", en: "Isolation, loneliness", zh: "孤立、寂寞" } } },
  { _id: 10, code: "wheel_of_fortune", name: { vi: "Bánh Xe Vận Mệnh", en: "Wheel of Fortune", zh: "命运之轮" }, arcana: "major", suit: null, number: 10, element: "fire",
    meanings: { upright: { vi: "Vận may, chu kỳ, bước ngoặt", en: "Luck, cycles, turning point", zh: "好运、周期、转折点" }, reversed: { vi: "Xui xẻo, kháng cự thay đổi", en: "Bad luck, resisting change", zh: "厄运、抗拒改变" } } },
  { _id: 11, code: "justice", name: { vi: "Công Lý", en: "Justice", zh: "正义" }, arcana: "major", suit: null, number: 11, element: "air",
    meanings: { upright: { vi: "Công bằng, sự thật, nhân quả", en: "Fairness, truth, karma", zh: "公平、真相、因果" }, reversed: { vi: "Bất công, thiếu trung thực", en: "Injustice, dishonesty", zh: "不公、不诚实" } } },
  { _id: 12, code: "the_hanged_man", name: { vi: "Kẻ Bị Treo", en: "The Hanged Man", zh: "倒吊人" }, arcana: "major", suit: null, number: 12, element: "water",
    meanings: { upright: { vi: "Buông bỏ, hy sinh, nhìn nhận mới", en: "Letting go, sacrifice, new perspective", zh: "放手、牺牲、新视角" }, reversed: { vi: "Trì hoãn, kháng cự", en: "Stalling, resistance", zh: "拖延、抵抗" } } },
  { _id: 13, code: "death", name: { vi: "Tử Thần", en: "Death", zh: "死神" }, arcana: "major", suit: null, number: 13, element: "water",
    meanings: { upright: { vi: "Kết thúc, chuyển đổi, tái sinh", en: "Endings, transformation, rebirth", zh: "结束、转变、重生" }, reversed: { vi: "Sợ thay đổi, bám víu quá khứ", en: "Fear of change, clinging to past", zh: "害怕改变、执着过去" } } },
  { _id: 14, code: "temperance", name: { vi: "Tiết Chế", en: "Temperance", zh: "节制" }, arcana: "major", suit: null, number: 14, element: "fire",
    meanings: { upright: { vi: "Cân bằng, kiên nhẫn, điều độ", en: "Balance, patience, moderation", zh: "平衡、耐心、节制" }, reversed: { vi: "Mất cân bằng, thái quá", en: "Imbalance, excess", zh: "失衡、过度" } } },
  { _id: 15, code: "the_devil", name: { vi: "Ác Quỷ", en: "The Devil", zh: "恶魔" }, arcana: "major", suit: null, number: 15, element: "earth",
    meanings: { upright: { vi: "Ràng buộc, cám dỗ, nghiện ngập", en: "Bondage, temptation, addiction", zh: "束缚、诱惑、成瘾" }, reversed: { vi: "Giải thoát, vượt qua giới hạn", en: "Liberation, overcoming limits", zh: "解放、突破限制" } } },
  { _id: 16, code: "the_tower", name: { vi: "Tháp", en: "The Tower", zh: "塔" }, arcana: "major", suit: null, number: 16, element: "fire",
    meanings: { upright: { vi: "Sụp đổ, thay đổi đột ngột, giác ngộ", en: "Upheaval, sudden change, revelation", zh: "剧变、突变、启示" }, reversed: { vi: "Trì hoãn thảm họa, sợ thay đổi", en: "Averting disaster, fear of change", zh: "避免灾难、害怕变化" } } },
  { _id: 17, code: "the_star", name: { vi: "Ngôi Sao", en: "The Star", zh: "星星" }, arcana: "major", suit: null, number: 17, element: "air",
    meanings: { upright: { vi: "Hy vọng, cảm hứng, bình yên", en: "Hope, inspiration, serenity", zh: "希望、灵感、宁静" }, reversed: { vi: "Tuyệt vọng, mất niềm tin", en: "Despair, loss of faith", zh: "绝望、失去信念" } } },
  { _id: 18, code: "the_moon", name: { vi: "Mặt Trăng", en: "The Moon", zh: "月亮" }, arcana: "major", suit: null, number: 18, element: "water",
    meanings: { upright: { vi: "Ảo tưởng, trực giác, lo lắng", en: "Illusion, intuition, anxiety", zh: "幻觉、直觉、焦虑" }, reversed: { vi: "Giải tỏa sợ hãi, sáng tỏ", en: "Releasing fear, clarity", zh: "释放恐惧、清明" } } },
  { _id: 19, code: "the_sun", name: { vi: "Mặt Trời", en: "The Sun", zh: "太阳" }, arcana: "major", suit: null, number: 19, element: "fire",
    meanings: { upright: { vi: "Niềm vui, thành công, sức sống", en: "Joy, success, vitality", zh: "快乐、成功、活力" }, reversed: { vi: "Bi quan tạm thời, trì hoãn thành công", en: "Temporary pessimism, delayed success", zh: "暂时悲观、延迟成功" } } },
  { _id: 20, code: "judgement", name: { vi: "Phán Xét", en: "Judgement", zh: "审判" }, arcana: "major", suit: null, number: 20, element: "fire",
    meanings: { upright: { vi: "Phục sinh, tự đánh giá, giác ngộ", en: "Rebirth, self-evaluation, awakening", zh: "重生、自我评价、觉醒" }, reversed: { vi: "Tự phê phán, nghi ngờ", en: "Self-criticism, doubt", zh: "自我批评、怀疑" } } },
  { _id: 21, code: "the_world", name: { vi: "Thế Giới", en: "The World", zh: "世界" }, arcana: "major", suit: null, number: 21, element: "earth",
    meanings: { upright: { vi: "Hoàn thành, thành tựu, viên mãn", en: "Completion, accomplishment, fulfillment", zh: "完成、成就、圆满" }, reversed: { vi: "Chưa hoàn thành, thiếu kết thúc", en: "Incompletion, lack of closure", zh: "未完成、缺乏结局" } } }
];

// ===== 56 LÁ MINOR ARCANA (22-77) =====
const suits = ["wands", "cups", "swords", "pentacles"];
const suitNames = {
  wands: { vi: "Gậy", en: "Wands", zh: "权杖" },
  cups: { vi: "Cốc", en: "Cups", zh: "圣杯" },
  swords: { vi: "Kiếm", en: "Swords", zh: "宝剑" },
  pentacles: { vi: "Đồng Xu", en: "Pentacles", zh: "钱币" }
};
const suitElements = { wands: "fire", cups: "water", swords: "air", pentacles: "earth" };
const cardNames = ["Ace","Two","Three","Four","Five","Six","Seven","Eight","Nine","Ten","Page","Knight","Queen","King"];
const cardNamesVi = ["Át","Hai","Ba","Bốn","Năm","Sáu","Bảy","Tám","Chín","Mười","Thị Vệ","Hiệp Sĩ","Hoàng Hậu","Nhà Vua"];
const cardNamesZh = ["王牌","二","三","四","五","六","七","八","九","十","侍从","骑士","王后","国王"];

let minorCards = [];
let id = 22;
for (const suit of suits) {
  for (let i = 0; i < 14; i++) {
    minorCards.push({
      _id: id++,
      code: `${cardNames[i].toLowerCase()}_of_${suit}`,
      name: {
        vi: `${cardNamesVi[i]} ${suitNames[suit].vi}`,
        en: `${cardNames[i]} of ${suitNames[suit].en}`,
        zh: `${suitNames[suit].zh}${cardNamesZh[i]}`
      },
      arcana: "minor",
      suit: suit,
      number: i + 1,
      element: suitElements[suit],
      meanings: {
        upright: { vi: `Ý nghĩa xuôi ${cardNamesVi[i]} ${suitNames[suit].vi}`, en: `Upright meaning for ${cardNames[i]} of ${suitNames[suit].en}`, zh: `正位含义` },
        reversed: { vi: `Ý nghĩa ngược ${cardNamesVi[i]} ${suitNames[suit].vi}`, en: `Reversed meaning for ${cardNames[i]} of ${suitNames[suit].en}`, zh: `逆位含义` }
      }
    });
  }
}

// INSERT TẤT CẢ
db.card_catalog.insertMany([...majorArcana, ...minorCards]);
print(`✅ Đã seed ${db.card_catalog.countDocuments()} lá bài Tarot!`);
```

**Cách chạy:**
```bash
# Lưu đoạn trên vào file seed_cards.js
mongosh tarotweb seed_cards.js
# Output: ✅ Đã seed 78 lá bài Tarot!

# Kiểm tra:
mongosh tarotweb --eval "db.card_catalog.countDocuments()"
# Output: 78
```

> ⚠️ **Lưu ý:** 22 lá Major Arcana có meanings chi tiết bằng 3 ngôn ngữ. 56 lá Minor Arcana dùng placeholder — bạn nên thay `"Ý nghĩa xuôi..."` bằng nội dung thực tế sau.

### Cách 2: Seed từ code C# (khi app khởi động)

Nếu muốn seed tự động khi `dotnet run`:

```
File: Infrastructure/Persistence/MongoDbSeedService.cs
  - Implement IHostedService
  - OnStartAsync: kiểm tra card_catalog.CountDocuments() == 0 → InsertMany(78 documents)
  - Đăng ký trong DI: services.AddHostedService<MongoDbSeedService>()
```

---

## 5. SEED TÀI KHOẢN SUPERADMIN

> Cần 1 tài khoản admin để quản lý hệ thống. Có 2 cách:

### Cách 1: SQL trực tiếp (Nhanh nhất)

```sql
-- Chạy trong psql:
-- Trước tiên, hash password "Admin@123456" bằng Argon2id
-- (hoặc tạm dùng cách 2 bên dưới cho đúng hash)

-- Nếu biết hash Argon2id (lấy từ code RegisterCommand), chạy:
INSERT INTO users (
  id, email, username, password_hash, display_name, date_of_birth,
  has_consented, level, exp, status, role, reader_status,
  mfa_enabled, created_at,
  wallet_gold_balance, wallet_diamond_balance, wallet_frozen_diamond_balance, wallet_total_diamonds_purchased
) VALUES (
  gen_random_uuid(),
  'admin@tarotnow.ai',
  'superadmin',
  '<PASTE_ARGON2ID_HASH_HERE>',
  'Super Admin',
  '1990-01-01',
  true, 99, 0, 'active', 'admin', 'approved',
  false, NOW(),
  999999, 999999, 0, 0
);
```

### Cách 2: Đăng ký thường + promote bằng SQL (Đơn giản hơn)

```
1. Đăng ký tài khoản thường qua /register (email: admin@tarotnow.ai)
2. Xác thực email (xem OTP trong terminal)
3. Promote lên admin bằng SQL:

UPDATE users SET role = 'admin', status = 'active', reader_status = 'approved'
WHERE email = 'admin@tarotnow.ai';
```

> 💡 **Cách 2 được khuyến nghị** vì password hash tự động đúng format Argon2id.

### Cách 3: Seed endpoint dev-only

```
File: Controllers/DiagController.cs (optional)
  - Chỉ chạy trong Development environment
  - POST "/api/v1/diag/seed-admin" → tạo admin với password mặc định
  - ⚠️ XÓA hoặc DISABLE endpoint này trước khi deploy production
```

---

## 6. `middleware.ts` CHO NEXT.JS I18N

> Next.js App Router cần file `middleware.ts` ở **root thư mục `src/`** để routing đa ngôn ngữ tự động. Nếu thiếu file này → URL không có prefix `/vi/`, `/en/`, `/zh/`.

Đặt tại: `Frontend/src/middleware.ts`

```typescript
// middleware.ts — Routing đa ngôn ngữ cho Next.js
// Tự động chuyển hướng: /login → /vi/login (nếu locale mặc định là 'vi')

import createMiddleware from 'next-intl/middleware';
import { routing } from './i18n/routing';

export default createMiddleware(routing);

export const config = {
  // Áp dụng cho tất cả route NGOẠI TRỪ: API routes, static files, images
  matcher: ['/', '/(vi|en|zh)/:path*']
};
```

---

## 7. CẤU HÌNH FRONTEND

### File `next.config.ts`

Đặt tại: `Frontend/next.config.ts`

```typescript
import type { NextConfig } from 'next';
import createNextIntlPlugin from 'next-intl/plugin';

// Tạo plugin i18n — đọc cấu hình từ src/i18n/request.ts
const withNextIntl = createNextIntlPlugin('./src/i18n/request.ts');

const nextConfig: NextConfig = {
  // Cho phép load ảnh từ domain bên ngoài
  images: {
    remotePatterns: [
      { protocol: 'https', hostname: '**' }
    ]
  }
};

export default withNextIntl(nextConfig);
```

### File `postcss.config.mjs`

Đặt tại: `Frontend/postcss.config.mjs`

```javascript
/** @type {import('postcss-load-config').Config} */
const config = {
  plugins: {
    '@tailwindcss/postcss': {},
  },
};

export default config;
```

### File `.env.local`

Đặt tại: `Frontend/.env.local`

```
NEXT_PUBLIC_API_URL=http://localhost:5037/api/v1
```

### File `src/i18n/request.ts`

Đặt tại: `Frontend/src/i18n/request.ts`

```typescript
import { getRequestConfig } from 'next-intl/server';
import { routing } from './routing';

export default getRequestConfig(async ({ requestLocale }) => {
  let locale = await requestLocale;

  // Kiểm tra locale có hợp lệ không, fallback sang 'vi'
  if (!locale || !routing.locales.includes(locale as any)) {
    locale = routing.defaultLocale;
  }

  return {
    locale,
    messages: (await import(`../../messages/${locale}.json`)).default
  };
});
```

### File `src/i18n/routing.ts`

Đặt tại: `Frontend/src/i18n/routing.ts`

```typescript
import { defineRouting } from 'next-intl/routing';
import { createNavigation } from 'next-intl/navigation';

export const routing = defineRouting({
  locales: ['vi', 'en', 'zh'],
  defaultLocale: 'vi'
});

export const { Link, redirect, usePathname, useRouter } =
  createNavigation(routing);
```

---

## ✅ CHECKLIST SAU KHI BỔ SUNG

| # | Mục | Đã có | File |
|---|-----|-------|------|
| 1 | appsettings.json | ✅ | `Api/appsettings.json` |
| 2 | appsettings.Development.json | ✅ | `Api/appsettings.Development.json` |
| 3 | AI Prompt mẫu | ✅ | Trong `StreamReadingCommand.cs` |
| 4 | Seed 78 lá bài | ✅ | Script `seed_cards.js` |
| 5 | Seed SuperAdmin | ✅ | SQL hoặc Cách 2 |
| 6 | middleware.ts | ✅ | `Frontend/src/middleware.ts` |
| 7 | next.config.ts | ✅ | `Frontend/next.config.ts` |

> 🎉 **Với 3 file: `CLONE_PROJECT_GUIDE.md` + `CODING_ORDER_DETAIL.md` + `CLONE_SUPPLEMENT.md`, bạn có đủ 100% thông tin để code lại dự án từ đầu!**
