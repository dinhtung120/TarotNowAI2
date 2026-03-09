// =============================================================================
// TarotWeb – Seed: cards_catalog (78 lá bài Tarot)
// =============================================================================
// Mục đích: Chèn dữ liệu tĩnh 78 lá bài Tarot vào collection cards_catalog.
//   - 22 lá Major Arcana (_id: 1-22)
//   - 56 lá Minor Arcana (_id: 23-78) gồm 4 bộ: Wands, Cups, Swords, Pentacles
//
// Cách chạy:
//   mongosh mongodb://localhost:27017/tarotweb < database/mongodb/seed_cards.js
//
// Idempotent: Dùng insertMany với ordered:false. Nếu chạy lại, lá trùng _id sẽ
// bị bỏ qua (duplicate key error) nhưng các lá mới (nếu có) vẫn được chèn.
//
// Schema mỗi lá bài:
//   _id: Int32 (1-78) – ID cố định
//   code: string – Mã ổn định (vd: the_fool, ace_of_wands)
//   name: {vi, en, zh} – Tên đa ngôn ngữ
//   arcana: "major" | "minor"
//   suit: null (Major) | "wands" | "cups" | "swords" | "pentacles"
//   number: Int32 – Số thứ tự trong bộ
//   element: "fire" | "water" | "air" | "earth"
//   meanings: { upright: {keywords[], description}, reversed: {keywords[], description} }
//   created_at, updated_at: ISODate
// =============================================================================

(function () {
  "use strict";

  // Biến thời gian hiện tại để gán cho created_at và updated_at
  const now = new Date();

  // =========================================================================
  // MAJOR ARCANA (22 lá, _id: 1-22)
  // =========================================================================
  // Major Arcana đại diện cho các bài học lớn trong cuộc sống, hành trình tâm linh.
  // Không thuộc bộ (suit) nào → suit: null.
  // Element được gán theo truyền thống Tarot phổ biến nhất.
  const majorArcana = [
    {
      _id: 0, code: "the_fool", number: 0, element: "air",
      name: { vi: "Kẻ Khờ", en: "The Fool", zh: "愚者" },
      meanings: {
        upright: { keywords: ["khởi đầu mới", "tự do", "ngây thơ", "phiêu lưu"], description: "Một khởi đầu mới đầy tiềm năng, bước vào cuộc phiêu lưu với tâm trí cởi mở và tự do." },
        reversed: { keywords: ["liều lĩnh", "bất cẩn", "sợ hãi", "thiếu suy nghĩ"], description: "Hành động thiếu suy nghĩ, liều lĩnh hoặc sợ hãi không dám bước ra khỏi vùng an toàn." }
      }
    },
    {
      _id: 1, code: "the_magician", number: 1, element: "air",
      name: { vi: "Nhà Ảo Thuật", en: "The Magician", zh: "魔术师" },
      meanings: {
        upright: { keywords: ["ý chí", "sáng tạo", "kỹ năng", "tập trung"], description: "Sử dụng tài năng và nguồn lực để biến ý tưởng thành hiện thực." },
        reversed: { keywords: ["lừa dối", "thiếu tập trung", "lãng phí tài năng", "thao túng"], description: "Sử dụng kỹ năng sai mục đích hoặc thiếu tập trung để đạt được mục tiêu." }
      }
    },
    {
      _id: 2, code: "the_high_priestess", number: 2, element: "water",
      name: { vi: "Nữ Tư Tế", en: "The High Priestess", zh: "女祭司" },
      meanings: {
        upright: { keywords: ["trực giác", "tiềm thức", "bí ẩn", "nội tâm"], description: "Lắng nghe tiếng nói nội tâm, tin tưởng trực giác và sự thông thái bên trong." },
        reversed: { keywords: ["bỏ qua trực giác", "bí mật", "xa rời nội tâm", "hời hợt"], description: "Bỏ qua trực giác, giữ bí mật hoặc mất kết nối với bản thân." }
      }
    },
    {
      _id: 3, code: "the_empress", number: 3, element: "earth",
      name: { vi: "Hoàng Hậu", en: "The Empress", zh: "皇后" },
      meanings: {
        upright: { keywords: ["sung túc", "nữ tính", "thiên nhiên", "nuôi dưỡng"], description: "Thời kỳ sung túc, sáng tạo và kết nối sâu sắc với thiên nhiên." },
        reversed: { keywords: ["phụ thuộc", "sáng tạo bị chặn", "thiếu chăm sóc", "trống rỗng"], description: "Thiếu sự nuôi dưỡng, sáng tạo bị kìm hãm hoặc phụ thuộc quá nhiều vào người khác." }
      }
    },
    {
      _id: 4, code: "the_emperor", number: 4, element: "fire",
      name: { vi: "Hoàng Đế", en: "The Emperor", zh: "皇帝" },
      meanings: {
        upright: { keywords: ["quyền lực", "cấu trúc", "ổn định", "lãnh đạo"], description: "Sự lãnh đạo mạnh mẽ, kỷ luật và xây dựng nền tảng vững chắc." },
        reversed: { keywords: ["độc đoán", "cứng nhắc", "mất kiểm soát", "thiếu kỷ luật"], description: "Lạm dụng quyền lực, quá cứng nhắc hoặc thiếu kỷ luật cần thiết." }
      }
    },
    {
      _id: 5, code: "the_hierophant", number: 5, element: "earth",
      name: { vi: "Giáo Hoàng", en: "The Hierophant", zh: "教皇" },
      meanings: {
        upright: { keywords: ["truyền thống", "tâm linh", "giáo dục", "tuân thủ"], description: "Tìm kiếm hướng dẫn tâm linh, tuân theo truyền thống và giá trị cộng đồng." },
        reversed: { keywords: ["nổi loạn", "tự do cá nhân", "thách thức truyền thống", "giáo điều"], description: "Thách thức những niềm tin lỗi thời hoặc cảm thấy bị gò bó bởi quy tắc." }
      }
    },
    {
      _id: 6, code: "the_lovers", number: 6, element: "air",
      name: { vi: "Đôi Tình Nhân", en: "The Lovers", zh: "恋人" },
      meanings: {
        upright: { keywords: ["tình yêu", "hòa hợp", "lựa chọn", "kết nối"], description: "Sự kết nối sâu sắc, lựa chọn quan trọng và mối quan hệ hài hòa." },
        reversed: { keywords: ["mất cân bằng", "xung đột", "lựa chọn sai", "chia rẽ"], description: "Mất hòa hợp trong mối quan hệ hoặc đối mặt với sự lựa chọn khó khăn." }
      }
    },
    {
      _id: 7, code: "the_chariot", number: 7, element: "water",
      name: { vi: "Cỗ Xe", en: "The Chariot", zh: "战车" },
      meanings: {
        upright: { keywords: ["ý chí", "chiến thắng", "quyết tâm", "kiểm soát"], description: "Vượt qua trở ngại bằng ý chí mạnh mẽ và sự tập trung." },
        reversed: { keywords: ["mất kiểm soát", "thiếu định hướng", "hung hăng", "thất bại"], description: "Mất phương hướng, thiếu tự chủ hoặc bị lực lượng đối lập kéo giữ." }
      }
    },
    {
      _id: 8, code: "strength", number: 8, element: "fire",
      name: { vi: "Sức Mạnh", en: "Strength", zh: "力量" },
      meanings: {
        upright: { keywords: ["sức mạnh nội tâm", "can đảm", "kiên nhẫn", "từ bi"], description: "Sức mạnh thật sự đến từ bên trong: lòng can đảm, kiên nhẫn và từ bi." },
        reversed: { keywords: ["yếu đuối", "tự nghi ngờ", "thiếu kiên nhẫn", "bất an"], description: "Thiếu tự tin, nghi ngờ bản thân hoặc để cảm xúc chi phối." }
      }
    },
    {
      _id: 9, code: "the_hermit", number: 9, element: "earth",
      name: { vi: "Ẩn Sĩ", en: "The Hermit", zh: "隐士" },
      meanings: {
        upright: { keywords: ["nội tâm", "cô độc", "tìm kiếm", "trí tuệ"], description: "Thời gian tĩnh lặng để suy ngẫm, tìm kiếm câu trả lời từ bên trong." },
        reversed: { keywords: ["cô lập", "chối bỏ", "trốn tránh", "cô đơn"], description: "Cô lập bản thân quá mức hoặc từ chối sự giúp đỡ từ người khác." }
      }
    },
    {
      _id: 10, code: "wheel_of_fortune", number: 10, element: "fire",
      name: { vi: "Bánh Xe Vận Mệnh", en: "Wheel of Fortune", zh: "命运之轮" },
      meanings: {
        upright: { keywords: ["vận may", "chu kỳ", "thay đổi", "cơ hội"], description: "Thay đổi tích cực, vận may đến và sự luân chuyển của cuộc sống." },
        reversed: { keywords: ["xui xẻo", "kháng cự thay đổi", "mất kiểm soát", "trì trệ"], description: "Giai đoạn xui xẻo hoặc chống lại những thay đổi không thể tránh khỏi." }
      }
    },
    {
      _id: 11, code: "justice", number: 11, element: "air",
      name: { vi: "Công Lý", en: "Justice", zh: "正义" },
      meanings: {
        upright: { keywords: ["công bằng", "sự thật", "trách nhiệm", "nhân quả"], description: "Sự thật được phơi bày, công lý được thực thi và chịu trách nhiệm." },
        reversed: { keywords: ["bất công", "gian dối", "thiếu trách nhiệm", "thiên vị"], description: "Bất công, thiếu trung thực hoặc trốn tránh hậu quả." }
      }
    },
    {
      _id: 12, code: "the_hanged_man", number: 12, element: "water",
      name: { vi: "Người Bị Treo", en: "The Hanged Man", zh: "倒吊人" },
      meanings: {
        upright: { keywords: ["hy sinh", "góc nhìn mới", "buông bỏ", "chờ đợi"], description: "Tạm dừng để nhìn mọi thứ từ góc độ khác, chấp nhận buông bỏ." },
        reversed: { keywords: ["trì hoãn", "kháng cự", "hy sinh vô ích", "bế tắc"], description: "Kháng cự sự thay đổi cần thiết hoặc hy sinh không cần thiết." }
      }
    },
    {
      _id: 13, code: "death", number: 13, element: "water",
      name: { vi: "Tử Thần", en: "Death", zh: "死神" },
      meanings: {
        upright: { keywords: ["kết thúc", "chuyển đổi", "tái sinh", "thay đổi"], description: "Kết thúc một giai đoạn để mở ra khởi đầu mới, sự chuyển đổi sâu sắc." },
        reversed: { keywords: ["kháng cự thay đổi", "sợ kết thúc", "trì trệ", "bám víu"], description: "Sợ hãi sự thay đổi, bám víu vào quá khứ và từ chối tiến về phía trước." }
      }
    },
    {
      _id: 14, code: "temperance", number: 14, element: "fire",
      name: { vi: "Tiết Chế", en: "Temperance", zh: "节制" },
      meanings: {
        upright: { keywords: ["cân bằng", "kiên nhẫn", "điều độ", "hài hòa"], description: "Tìm kiếm sự cân bằng, kiên nhẫn trong mọi việc và hòa hợp các mặt đối lập." },
        reversed: { keywords: ["mất cân bằng", "thái quá", "thiếu kiên nhẫn", "xung đột nội tâm"], description: "Thiếu điều độ, hành động thái quá hoặc mất cân bằng trong cuộc sống." }
      }
    },
    {
      _id: 15, code: "the_devil", number: 15, element: "earth",
      name: { vi: "Ác Quỷ", en: "The Devil", zh: "恶魔" },
      meanings: {
        upright: { keywords: ["cám dỗ", "ràng buộc", "nghiện ngập", "bóng tối"], description: "Bị ràng buộc bởi cám dỗ, thói quen xấu hoặc nỗi sợ hãi." },
        reversed: { keywords: ["giải phóng", "tự do", "phá vỡ xiềng xích", "thức tỉnh"], description: "Giải phóng bản thân khỏi những ràng buộc tiêu cực và tìm lại tự do." }
      }
    },
    {
      _id: 16, code: "the_tower", number: 16, element: "fire",
      name: { vi: "Tháp", en: "The Tower", zh: "塔" },
      meanings: {
        upright: { keywords: ["đổ vỡ", "thay đổi đột ngột", "sự thật", "giải phóng"], description: "Sự đổ vỡ bất ngờ phá bỏ những gì không còn phù hợp để xây dựng lại." },
        reversed: { keywords: ["trì hoãn thảm họa", "sợ thay đổi", "tránh né", "khủng hoảng cá nhân"], description: "Cố tránh sự sụp đổ cần thiết hoặc trải qua khủng hoảng nội tâm." }
      }
    },
    {
      _id: 17, code: "the_star", number: 17, element: "air",
      name: { vi: "Ngôi Sao", en: "The Star", zh: "星星" },
      meanings: {
        upright: { keywords: ["hy vọng", "niềm tin", "cảm hứng", "bình yên"], description: "Niềm hy vọng mới, sự chữa lành và cảm hứng sáng tạo." },
        reversed: { keywords: ["mất hy vọng", "chán nản", "mất niềm tin", "cô đơn"], description: "Mất niềm tin vào tương lai, cảm thấy chán nản và thiếu cảm hứng." }
      }
    },
    {
      _id: 18, code: "the_moon", number: 18, element: "water",
      name: { vi: "Mặt Trăng", en: "The Moon", zh: "月亮" },
      meanings: {
        upright: { keywords: ["ảo tưởng", "trực giác", "tiềm thức", "sợ hãi"], description: "Đối mặt với nỗi sợ hãi, ảo tưởng và những điều ẩn giấu trong tiềm thức." },
        reversed: { keywords: ["sáng tỏ", "vượt qua sợ hãi", "sự thật lộ ra", "giải thoát"], description: "Sự thật dần sáng tỏ, vượt qua nỗi sợ hãi và ảo tưởng." }
      }
    },
    {
      _id: 19, code: "the_sun", number: 19, element: "fire",
      name: { vi: "Mặt Trời", en: "The Sun", zh: "太阳" },
      meanings: {
        upright: { keywords: ["niềm vui", "thành công", "sinh lực", "tích cực"], description: "Niềm vui rạng rỡ, thành công và sự lạc quan tràn đầy." },
        reversed: { keywords: ["chán nản tạm thời", "thiếu sáng tạo", "chậm trễ", "tự cao"], description: "Niềm vui bị che khuất tạm thời hoặc tự mãn quá mức." }
      }
    },
    {
      _id: 20, code: "judgement", number: 20, element: "fire",
      name: { vi: "Phán Xét", en: "Judgement", zh: "审判" },
      meanings: {
        upright: { keywords: ["tái sinh", "soi xét", "tha thứ", "thức tỉnh"], description: "Thời điểm tổng kết, soi xét bản thân và bước vào giai đoạn mới." },
        reversed: { keywords: ["tự phê phán", "từ chối bài học", "nghi ngờ", "trốn tránh"], description: "Tự phê phán quá mức hoặc từ chối học hỏi từ quá khứ." }
      }
    },
    {
      _id: 21, code: "the_world", number: 21, element: "earth",
      name: { vi: "Thế Giới", en: "The World", zh: "世界" },
      meanings: {
        upright: { keywords: ["hoàn thành", "thành tựu", "trọn vẹn", "hội nhập"], description: "Hoàn thành một chu kỳ lớn, đạt được sự trọn vẹn và hài lòng." },
        reversed: { keywords: ["chưa hoàn thành", "thiếu kết thúc", "trì hoãn", "thiếu trọn vẹn"], description: "Chưa hoàn thành mục tiêu hoặc cảm thấy thiếu sự trọn vẹn." }
      }
    }
  ];

  // Gán thuộc tính chung cho Major Arcana: arcana, suit, timestamps
  majorArcana.forEach(function (card) {
    card.arcana = "major";
    card.suit = null;
    card.created_at = now;
    card.updated_at = now;
  });

  // =========================================================================
  // MINOR ARCANA (56 lá, _id: 22-77)
  // =========================================================================
  // Minor Arcana chia thành 4 bộ (suits), mỗi bộ 14 lá (Ace-10, Page, Knight, Queen, King).
  // Mỗi bộ tương ứng với một nguyên tố (element) và khía cạnh cuộc sống khác nhau.

  // Cấu hình 4 bộ bài Minor Arcana
  // startId: _id bắt đầu cho bộ đó trong database
  const suits = [
    {
      suit: "wands", element: "fire", startId: 22,
      // Wands (Gậy) – Nguyên tố Lửa: hành động, sáng tạo, ý chí, năng lượng
      names: [
        { vi: "Át Gậy", en: "Ace of Wands", zh: "权杖王牌" },
        { vi: "Hai Gậy", en: "Two of Wands", zh: "权杖二" },
        { vi: "Ba Gậy", en: "Three of Wands", zh: "权杖三" },
        { vi: "Bốn Gậy", en: "Four of Wands", zh: "权杖四" },
        { vi: "Năm Gậy", en: "Five of Wands", zh: "权杖五" },
        { vi: "Sáu Gậy", en: "Six of Wands", zh: "权杖六" },
        { vi: "Bảy Gậy", en: "Seven of Wands", zh: "权杖七" },
        { vi: "Tám Gậy", en: "Eight of Wands", zh: "权杖八" },
        { vi: "Chín Gậy", en: "Nine of Wands", zh: "权杖九" },
        { vi: "Mười Gậy", en: "Ten of Wands", zh: "权杖十" },
        { vi: "Thị Đồng Gậy", en: "Page of Wands", zh: "权杖侍从" },
        { vi: "Kỵ Sĩ Gậy", en: "Knight of Wands", zh: "权杖骑士" },
        { vi: "Nữ Hoàng Gậy", en: "Queen of Wands", zh: "权杖王后" },
        { vi: "Quốc Vương Gậy", en: "King of Wands", zh: "权杖国王" }
      ],
      meanings: [
        { upright: { keywords: ["khởi đầu", "cảm hứng", "tiềm năng"], description: "Nguồn năng lượng sáng tạo mới, khởi đầu đầy cảm hứng." }, reversed: { keywords: ["trì hoãn", "thiếu động lực", "thất vọng"], description: "Thiếu nguồn cảm hứng hoặc trì hoãn dự án." } },
        { upright: { keywords: ["lập kế hoạch", "tương lai", "quyết định"], description: "Nhìn về tương lai và lên kế hoạch hành động." }, reversed: { keywords: ["sợ thay đổi", "thiếu kế hoạch", "bất an"], description: "Sợ hãi trước những điều chưa biết." } },
        { upright: { keywords: ["mở rộng", "tầm nhìn", "lãnh đạo"], description: "Mở rộng tầm nhìn, nắm bắt cơ hội mới." }, reversed: { keywords: ["trì hoãn", "thiếu tầm nhìn", "thất vọng"], description: "Thiếu tầm nhìn xa hoặc kết quả không như mong đợi." } },
        { upright: { keywords: ["ăn mừng", "hài hòa", "mốc quan trọng"], description: "Ăn mừng thành tựu, sự hài hòa trong gia đình." }, reversed: { keywords: ["xung đột", "bất ổn", "thiếu ổn định"], description: "Bất hòa trong gia đình hoặc mất ổn định." } },
        { upright: { keywords: ["cạnh tranh", "xung đột", "tranh luận"], description: "Cạnh tranh lành mạnh hoặc đối mặt với thách thức." }, reversed: { keywords: ["tránh xung đột", "thỏa hiệp", "hòa giải"], description: "Tìm cách tránh xung đột hoặc thỏa hiệp." } },
        { upright: { keywords: ["chiến thắng", "thành công", "công nhận"], description: "Chiến thắng vinh quang, được thừa nhận thành tựu." }, reversed: { keywords: ["thất bại", "thiếu tự tin", "kiêu ngạo"], description: "Thiếu sự công nhận hoặc tự mãn quá mức." } },
        { upright: { keywords: ["bảo vệ", "kiên cường", "thách thức"], description: "Đứng vững trước thử thách, bảo vệ lập trường." }, reversed: { keywords: ["bỏ cuộc", "áp đảo", "mệt mỏi"], description: "Cảm thấy kiệt sức hoặc bị áp đảo." } },
        { upright: { keywords: ["tốc độ", "hành động", "chuyển động"], description: "Sự việc tiến triển nhanh chóng, hành động tức thì." }, reversed: { keywords: ["trì hoãn", "vội vàng", "mất kiểm soát"], description: "Hành động vội vàng hoặc sự trì hoãn không mong muốn." } },
        { upright: { keywords: ["kiên trì", "sức bền", "cảnh giác"], description: "Kiên trì dù mệt mỏi, sẵn sàng đối mặt thử thách cuối." }, reversed: { keywords: ["kiệt sức", "nghi ngờ", "từ bỏ"], description: "Kiệt sức, muốn bỏ cuộc khi gần đến đích." } },
        { upright: { keywords: ["gánh nặng", "trách nhiệm", "áp lực"], description: "Gánh vác quá nhiều trách nhiệm, cần biết ủy thác." }, reversed: { keywords: ["giải thoát", "buông bỏ", "nhẹ nhõm"], description: "Buông bỏ gánh nặng không cần thiết." } },
        { upright: { keywords: ["khám phá", "nhiệt huyết", "tự do"], description: "Khám phá thế giới với sự nhiệt huyết và tò mò." }, reversed: { keywords: ["thiếu định hướng", "hời hợt", "nóng vội"], description: "Thiếu kinh nghiệm hoặc quá nóng vội." } },
        { upright: { keywords: ["hành động", "phiêu lưu", "đam mê"], description: "Hành động táo bạo, đam mê và phiêu lưu." }, reversed: { keywords: ["nóng nảy", "liều lĩnh", "thiếu kiên nhẫn"], description: "Quá nóng nảy hoặc hành động thiếu suy nghĩ." } },
        { upright: { keywords: ["tự tin", "quyết đoán", "quyến rũ"], description: "Tự tin, quyến rũ và truyền cảm hứng cho người khác." }, reversed: { keywords: ["ích kỷ", "ghen tuông", "thiếu tự tin"], description: "Thiếu tự tin hoặc quá ích kỷ trong mối quan hệ." } },
        { upright: { keywords: ["lãnh đạo", "tầm nhìn", "kinh nghiệm"], description: "Lãnh đạo bằng tầm nhìn và kinh nghiệm phong phú." }, reversed: { keywords: ["độc đoán", "cứng nhắc", "thiếu linh hoạt"], description: "Lãnh đạo quá cứng nhắc hoặc thiếu linh hoạt." } }
      ]
    },
    {
      suit: "cups", element: "water", startId: 36,
      // Cups (Cốc) – Nguyên tố Nước: cảm xúc, tình yêu, mối quan hệ, trực giác
      names: [
        { vi: "Át Cốc", en: "Ace of Cups", zh: "圣杯王牌" },
        { vi: "Hai Cốc", en: "Two of Cups", zh: "圣杯二" },
        { vi: "Ba Cốc", en: "Three of Cups", zh: "圣杯三" },
        { vi: "Bốn Cốc", en: "Four of Cups", zh: "圣杯四" },
        { vi: "Năm Cốc", en: "Five of Cups", zh: "圣杯五" },
        { vi: "Sáu Cốc", en: "Six of Cups", zh: "圣杯六" },
        { vi: "Bảy Cốc", en: "Seven of Cups", zh: "圣杯七" },
        { vi: "Tám Cốc", en: "Eight of Cups", zh: "圣杯八" },
        { vi: "Chín Cốc", en: "Nine of Cups", zh: "圣杯九" },
        { vi: "Mười Cốc", en: "Ten of Cups", zh: "圣杯十" },
        { vi: "Thị Đồng Cốc", en: "Page of Cups", zh: "圣杯侍从" },
        { vi: "Kỵ Sĩ Cốc", en: "Knight of Cups", zh: "圣杯骑士" },
        { vi: "Nữ Hoàng Cốc", en: "Queen of Cups", zh: "圣杯王后" },
        { vi: "Quốc Vương Cốc", en: "King of Cups", zh: "圣杯国王" }
      ],
      meanings: [
        { upright: { keywords: ["tình yêu mới", "cảm hứng", "hạnh phúc"], description: "Tình yêu mới nảy nở hoặc cảm hứng sáng tạo tràn đầy." }, reversed: { keywords: ["trống rỗng", "mất kết nối", "buồn bã"], description: "Cảm xúc trống rỗng, mất kết nối với bản thân." } },
        { upright: { keywords: ["kết nối", "đối tác", "hòa hợp"], description: "Mối quan hệ hòa hợp, đối tác lý tưởng." }, reversed: { keywords: ["mất cân bằng", "chia rẽ", "thiếu thấu hiểu"], description: "Mất cân bằng trong mối quan hệ." } },
        { upright: { keywords: ["ăn mừng", "tình bạn", "cộng đồng"], description: "Niềm vui chia sẻ cùng bạn bè và cộng đồng." }, reversed: { keywords: ["cô lập", "quá đà", "tin đồn"], description: "Cô lập khỏi nhóm hoặc ăn chơi quá mức." } },
        { upright: { keywords: ["suy ngẫm", "bất mãn", "bỏ lỡ"], description: "Không hài lòng với hiện tại, bỏ lỡ cơ hội." }, reversed: { keywords: ["trân trọng", "nhận ra", "tiến về phía trước"], description: "Nhận ra giá trị và trân trọng những gì đang có." } },
        { upright: { keywords: ["mất mát", "tiếc nuối", "đau buồn"], description: "Đau buồn vì mất mát nhưng vẫn còn những gì đáng trân trọng." }, reversed: { keywords: ["chấp nhận", "tha thứ", "tiến về phía trước"], description: "Bắt đầu chấp nhận mất mát và tiến về phía trước." } },
        { upright: { keywords: ["hoài niệm", "ký ức", "ngây thơ"], description: "Hoài niệm ký ức đẹp, sự ngây thơ và đơn thuần." }, reversed: { keywords: ["sống trong quá khứ", "thiếu trưởng thành", "buông bỏ"], description: "Bám víu quá khứ thay vì sống với hiện tại." } },
        { upright: { keywords: ["lựa chọn", "ảo tưởng", "tưởng tượng"], description: "Nhiều lựa chọn hấp dẫn nhưng cần phân biệt thực tế." }, reversed: { keywords: ["quyết đoán", "rõ ràng", "hành động"], description: "Cuối cùng đưa ra quyết định rõ ràng." } },
        { upright: { keywords: ["ra đi", "tìm kiếm", "từ bỏ"], description: "Rời bỏ tình huống không còn mang lại ý nghĩa." }, reversed: { keywords: ["sợ thay đổi", "bám víu", "do dự"], description: "Sợ rời bỏ những gì quen thuộc." } },
        { upright: { keywords: ["mãn nguyện", "ước nguyện", "hạnh phúc"], description: "Ước nguyện thành hiện thực, sự mãn nguyện sâu sắc." }, reversed: { keywords: ["tham lam", "bất mãn", "kiêu ngạo"], description: "Không bao giờ hài lòng hoặc tự mãn quá mức." } },
        { upright: { keywords: ["hạnh phúc gia đình", "hài hòa", "viên mãn"], description: "Hạnh phúc gia đình trọn vẹn, mối quan hệ viên mãn." }, reversed: { keywords: ["bất hòa", "gia đình tan vỡ", "mất kết nối"], description: "Xung đột gia đình hoặc thiếu sự hài hòa." } },
        { upright: { keywords: ["sáng tạo", "trực giác", "nhạy cảm"], description: "Sáng tạo phong phú, trực giác mạnh mẽ." }, reversed: { keywords: ["dễ tổn thương", "ảo tưởng", "chưa trưởng thành"], description: "Quá nhạy cảm hoặc sống trong ảo tưởng." } },
        { upright: { keywords: ["lãng mạn", "duyên dáng", "mời gọi"], description: "Lãng mạn, đi theo tiếng gọi trái tim." }, reversed: { keywords: ["thay đổi", "không thực tế", "thất vọng"], description: "Thay đổi thất thường hoặc kỳ vọng không thực tế." } },
        { upright: { keywords: ["đồng cảm", "trực giác", "chữa lành"], description: "Đồng cảm sâu sắc, trực giác mạnh và khả năng chữa lành." }, reversed: { keywords: ["quá nhạy cảm", "đồng phụ thuộc", "mất cân bằng cảm xúc"], description: "Quá nhạy cảm hoặc đặt nhu cầu người khác lên trước." } },
        { upright: { keywords: ["nhân từ", "bình tĩnh", "cân bằng cảm xúc"], description: "Kiểm soát cảm xúc tốt, tử tế và nhân từ." }, reversed: { keywords: ["lạnh lùng", "ức chế cảm xúc", "thao túng"], description: "Đè nén cảm xúc hoặc lạnh lùng với người khác." } }
      ]
    },
    {
      suit: "swords", element: "air", startId: 50,
      // Swords (Kiếm) – Nguyên tố Gió: tư duy, giao tiếp, xung đột, sự thật
      names: [
        { vi: "Át Kiếm", en: "Ace of Swords", zh: "宝剑王牌" },
        { vi: "Hai Kiếm", en: "Two of Swords", zh: "宝剑二" },
        { vi: "Ba Kiếm", en: "Three of Swords", zh: "宝剑三" },
        { vi: "Bốn Kiếm", en: "Four of Swords", zh: "宝剑四" },
        { vi: "Năm Kiếm", en: "Five of Swords", zh: "宝剑五" },
        { vi: "Sáu Kiếm", en: "Six of Swords", zh: "宝剑六" },
        { vi: "Bảy Kiếm", en: "Seven of Swords", zh: "宝剑七" },
        { vi: "Tám Kiếm", en: "Eight of Swords", zh: "宝剑八" },
        { vi: "Chín Kiếm", en: "Nine of Swords", zh: "宝剑九" },
        { vi: "Mười Kiếm", en: "Ten of Swords", zh: "宝剑十" },
        { vi: "Thị Đồng Kiếm", en: "Page of Swords", zh: "宝剑侍从" },
        { vi: "Kỵ Sĩ Kiếm", en: "Knight of Swords", zh: "宝剑骑士" },
        { vi: "Nữ Hoàng Kiếm", en: "Queen of Swords", zh: "宝剑王后" },
        { vi: "Quốc Vương Kiếm", en: "King of Swords", zh: "宝剑国王" }
      ],
      meanings: [
        { upright: { keywords: ["sáng suốt", "đột phá", "sự thật"], description: "Sự sáng suốt và rõ ràng trong tư duy, ý tưởng đột phá." }, reversed: { keywords: ["nhầm lẫn", "hỗn loạn", "suy nghĩ tiêu cực"], description: "Tư duy lộn xộn hoặc sử dụng trí tuệ sai hướng." } },
        { upright: { keywords: ["bế tắc", "lựa chọn khó", "cân nhắc"], description: "Đối mặt lựa chọn khó khăn, cần bình tĩnh cân nhắc." }, reversed: { keywords: ["quyết đoán", "quá tải thông tin", "thiên lệch"], description: "Cuối cùng đưa ra quyết định sau thời gian cân nhắc." } },
        { upright: { keywords: ["đau lòng", "thất vọng", "mất mát"], description: "Nỗi đau lòng, sự thất vọng hoặc phản bội." }, reversed: { keywords: ["hồi phục", "tha thứ", "chữa lành"], description: "Bắt đầu hồi phục và tha thứ sau nỗi đau." } },
        { upright: { keywords: ["nghỉ ngơi", "phục hồi", "suy ngẫm"], description: "Cần thời gian nghỉ ngơi để phục hồi năng lượng." }, reversed: { keywords: ["kiệt sức", "bồn chồn", "vội vàng"], description: "Vội vàng trở lại hoạt động khi chưa hồi phục." } },
        { upright: { keywords: ["xung đột", "thắng bất chính", "mất mát"], description: "Chiến thắng bằng mọi giá nhưng mất mát nhiều hơn." }, reversed: { keywords: ["hòa giải", "hối hận", "chấp nhận thua"], description: "Nhận ra cái giá của xung đột và tìm cách hòa giải." } },
        { upright: { keywords: ["chuyển đổi", "rời xa", "hồi phục"], description: "Rời xa khó khăn để tìm đến nơi bình yên hơn." }, reversed: { keywords: ["kháng cự", "bám víu", "chưa sẵn sàng"], description: "Chưa sẵn sàng rời bỏ tình huống tiêu cực." } },
        { upright: { keywords: ["lừa dối", "chiến lược", "bí mật"], description: "Hành động lén lút hoặc sử dụng chiến lược khôn ngoan." }, reversed: { keywords: ["bại lộ", "trung thực", "tự thú"], description: "Bí mật bị phơi bày hoặc quyết định trung thực." } },
        { upright: { keywords: ["mắc kẹt", "bất lực", "tự giới hạn"], description: "Cảm thấy bị giam giữ bởi hoàn cảnh hoặc chính mình." }, reversed: { keywords: ["giải phóng", "nhìn thấy lối thoát", "tự tin"], description: "Nhận ra rằng giới hạn phần lớn do chính mình tạo ra." } },
        { upright: { keywords: ["lo âu", "ác mộng", "sợ hãi"], description: "Lo âu và sợ hãi ám ảnh, đặc biệt vào ban đêm." }, reversed: { keywords: ["vượt qua lo âu", "hy vọng", "bình tĩnh"], description: "Bắt đầu vượt qua nỗi lo âu và tìm lại sự bình yên." } },
        { upright: { keywords: ["kết thúc đau đớn", "phản bội", "sụp đổ"], description: "Kết thúc đau đớn và đột ngột, nhưng mở ra khởi đầu mới." }, reversed: { keywords: ["hồi phục", "bài học", "tái sinh"], description: "Từ từ hồi phục sau bi kịch và rút ra bài học." } },
        { upright: { keywords: ["tò mò", "thông minh", "cảnh giác"], description: "Tò mò, thông minh và luôn sẵn sàng học hỏi." }, reversed: { keywords: ["đa nghi", "tin đồn", "thiếu suy nghĩ"], description: "Nói mà không suy nghĩ hoặc lan truyền tin đồn." } },
        { upright: { keywords: ["nhanh nhẹn", "quyết đoán", "tham vọng"], description: "Hành động nhanh và quyết đoán với tham vọng lớn." }, reversed: { keywords: ["vội vàng", "hung hăng", "thiếu kế hoạch"], description: "Hành động quá vội vàng mà thiếu suy nghĩ kỹ." } },
        { upright: { keywords: ["sắc bén", "độc lập", "trung thực"], description: "Trí tuệ sắc bén, độc lập và trung thực không khoan nhượng." }, reversed: { keywords: ["lạnh lùng", "cay độc", "cô đơn"], description: "Quá lạnh lùng hoặc sử dụng lời nói gây tổn thương." } },
        { upright: { keywords: ["quyền lực trí tuệ", "công bằng", "logic"], description: "Lãnh đạo bằng trí tuệ, công bằng và logic." }, reversed: { keywords: ["lạm quyền", "bất công", "tàn nhẫn"], description: "Lạm dụng trí tuệ để thao túng hoặc kiểm soát." } }
      ]
    },
    {
      suit: "pentacles", element: "earth", startId: 64,
      // Pentacles (Tiền) – Nguyên tố Đất: vật chất, tài chính, sức khỏe, công việc
      names: [
        { vi: "Át Tiền", en: "Ace of Pentacles", zh: "金币王牌" },
        { vi: "Hai Tiền", en: "Two of Pentacles", zh: "金币二" },
        { vi: "Ba Tiền", en: "Three of Pentacles", zh: "金币三" },
        { vi: "Bốn Tiền", en: "Four of Pentacles", zh: "金币四" },
        { vi: "Năm Tiền", en: "Five of Pentacles", zh: "金币五" },
        { vi: "Sáu Tiền", en: "Six of Pentacles", zh: "金币六" },
        { vi: "Bảy Tiền", en: "Seven of Pentacles", zh: "金币七" },
        { vi: "Tám Tiền", en: "Eight of Pentacles", zh: "金币八" },
        { vi: "Chín Tiền", en: "Nine of Pentacles", zh: "金币九" },
        { vi: "Mười Tiền", en: "Ten of Pentacles", zh: "金币十" },
        { vi: "Thị Đồng Tiền", en: "Page of Pentacles", zh: "金币侍从" },
        { vi: "Kỵ Sĩ Tiền", en: "Knight of Pentacles", zh: "金币骑士" },
        { vi: "Nữ Hoàng Tiền", en: "Queen of Pentacles", zh: "金币王后" },
        { vi: "Quốc Vương Tiền", en: "King of Pentacles", zh: "金币国王" }
      ],
      meanings: [
        { upright: { keywords: ["cơ hội tài chính", "sung túc", "nền tảng"], description: "Cơ hội tài chính mới, nền tảng vững chắc cho tương lai." }, reversed: { keywords: ["mất cơ hội", "thiếu kế hoạch", "lãng phí"], description: "Bỏ lỡ cơ hội tài chính hoặc lãng phí nguồn lực." } },
        { upright: { keywords: ["cân bằng", "linh hoạt", "đa nhiệm"], description: "Cân bằng nhiều ưu tiên với sự linh hoạt." }, reversed: { keywords: ["quá tải", "mất cân bằng", "lộn xộn"], description: "Quá tải công việc, không thể cân bằng mọi thứ." } },
        { upright: { keywords: ["làm việc nhóm", "kỹ năng", "chất lượng"], description: "Hợp tác hiệu quả, phát huy kỹ năng chuyên môn." }, reversed: { keywords: ["thiếu hợp tác", "chất lượng kém", "xung đột nhóm"], description: "Thiếu sự hợp tác hoặc không đáp ứng tiêu chuẩn." } },
        { upright: { keywords: ["an toàn", "tiết kiệm", "ổn định"], description: "Bảo vệ tài sản, tìm kiếm sự ổn định tài chính." }, reversed: { keywords: ["tham lam", "keo kiệt", "sợ mất mát"], description: "Quá keo kiệt hoặc bám víu vào vật chất." } },
        { upright: { keywords: ["khó khăn", "nghèo khó", "cô lập"], description: "Giai đoạn khó khăn về tài chính hoặc tinh thần." }, reversed: { keywords: ["hồi phục", "tìm giúp đỡ", "hy vọng"], description: "Bắt đầu vượt qua khó khăn, tìm thấy sự hỗ trợ." } },
        { upright: { keywords: ["chia sẻ", "hào phóng", "từ thiện"], description: "Chia sẻ tài nguyên, hào phóng với người khác." }, reversed: { keywords: ["ích kỷ", "nợ nần", "bất bình đẳng"], description: "Thiếu sự công bằng trong cho đi và nhận lại." } },
        { upright: { keywords: ["kiên nhẫn", "đầu tư dài hạn", "đánh giá"], description: "Kiên nhẫn chờ đợi kết quả từ sự đầu tư lâu dài." }, reversed: { keywords: ["thiếu kiên nhẫn", "đầu tư thất bại", "nản lòng"], description: "Thiếu kiên nhẫn hoặc thất vọng với kết quả." } },
        { upright: { keywords: ["chăm chỉ", "kỹ năng", "hoàn thiện"], description: "Chăm chỉ rèn luyện và hoàn thiện kỹ năng." }, reversed: { keywords: ["lười biếng", "hời hợt", "thiếu tập trung"], description: "Thiếu tập trung hoặc không đầu tư đủ nỗ lực." } },
        { upright: { keywords: ["thịnh vượng", "tự lập", "xa xỉ"], description: "Thịnh vượng và tận hưởng thành quả lao động." }, reversed: { keywords: ["phụ thuộc", "sống quá mức", "thiếu an toàn"], description: "Sống quá mức khả năng hoặc phụ thuộc người khác." } },
        { upright: { keywords: ["gia tài", "truyền thống", "ổn định dài lâu"], description: "Gia tài vững chắc, sự ổn định qua nhiều thế hệ." }, reversed: { keywords: ["xung đột gia đình", "mất gia tài", "bất ổn"], description: "Xung đột về tài sản gia đình hoặc bất ổn tài chính." } },
        { upright: { keywords: ["học hỏi", "cơ hội", "tham vọng"], description: "Cơ hội học hỏi và phát triển tài chính." }, reversed: { keywords: ["thiếu tiến bộ", "lãng phí thời gian", "thiếu tham vọng"], description: "Thiếu tham vọng hoặc không tận dụng cơ hội." } },
        { upright: { keywords: ["đáng tin cậy", "kiên nhẫn", "chăm chỉ"], description: "Tiến bước chậm mà chắc, đáng tin cậy và kiên nhẫn." }, reversed: { keywords: ["ù lỳ", "quá thận trọng", "nhàm chán"], description: "Quá thận trọng đến mức bỏ lỡ cơ hội." } },
        { upright: { keywords: ["nuôi dưỡng", "thực tế", "an toàn"], description: "Mang lại sự an toàn và nuôi dưỡng thông qua thực tế." }, reversed: { keywords: ["chểnh mảng", "hy sinh quá nhiều", "mất kết nối"], description: "Hy sinh bản thân quá nhiều cho người khác." } },
        { upright: { keywords: ["giàu có", "thành đạt", "an toàn"], description: "Thành công tài chính, lãnh đạo kinh doanh xuất sắc." }, reversed: { keywords: ["tham lam", "lãng phí", "thiếu đạo đức"], description: "Quá tập trung vào tiền bạc mà bỏ quên giá trị khác." } }
      ]
    }
  ];

  // =========================================================================
  // Tạo danh sách 56 lá Minor Arcana từ cấu hình suits
  // =========================================================================
  const minorArcana = [];

  // Hàm helper: tạo code từ tên tiếng Anh
  // Ví dụ: "Ace of Wands" → "ace_of_wands", "Page of Cups" → "page_of_cups"
  function toCode(enName) {
    return enName.toLowerCase().replace(/ /g, "_");
  }

  suits.forEach(function (suitConfig) {
    for (var i = 0; i < 14; i++) {
      minorArcana.push({
        _id: suitConfig.startId + i,
        code: toCode(suitConfig.names[i].en),
        name: suitConfig.names[i],
        arcana: "minor",
        suit: suitConfig.suit,
        number: i + 1,         // Ace=1, Two=2, ..., King=14
        element: suitConfig.element,
        meanings: suitConfig.meanings[i],
        created_at: now,
        updated_at: now
      });
    }
  });

  // =========================================================================
  // Gộp 22 Major + 56 Minor = 78 lá bài
  // =========================================================================
  const allCards = majorArcana.concat(minorArcana);

  // Kiểm tra: đảm bảo đúng 78 lá
  if (allCards.length !== 78) {
    print("ERROR: Expected 78 cards but got " + allCards.length);
    quit(1);
  }

  // =========================================================================
  // Insert vào collection cards_catalog
  // =========================================================================
  // ordered: false → nếu lá nào trùng _id (đã tồn tại) thì bỏ qua, không dừng
  // Cách này đảm bảo idempotent: chạy lại không lỗi
  try {
    var result = db.cards_catalog.insertMany(allCards, { ordered: false });
    print("Inserted " + result.insertedIds.length + " cards successfully.");
  } catch (e) {
    // Nếu có duplicate key errors (do chạy lại), vẫn in số lá đã insert
    if (e.writeErrors) {
      var duplicates = e.writeErrors.filter(function (err) { return err.code === 11000; });
      print("Skipped " + duplicates.length + " duplicate cards (already exist).");
      print("Inserted " + (allCards.length - duplicates.length) + " new cards.");
    } else {
      throw e;
    }
  }

  // Verify: đếm tổng số lá trong collection
  var count = db.cards_catalog.countDocuments();
  print("Total cards in cards_catalog: " + count);
  if (count !== 78) {
    print("WARNING: Expected 78 cards but found " + count);
  }

  print("Seed cards_catalog completed.");

})();
