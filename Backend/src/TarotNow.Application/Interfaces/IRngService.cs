
namespace TarotNow.Application.Interfaces;

// Contract RNG để chuẩn hóa thao tác xáo bài và chọn thưởng có trọng số.
public interface IRngService
{
    /// <summary>
    /// Xáo bộ bài theo số lượng deckSize để tạo thứ tự rút ngẫu nhiên công bằng.
    /// Luồng xử lý: sinh hoán vị ngẫu nhiên của các vị trí lá bài và trả mảng kết quả.
    /// </summary>
    int[] ShuffleDeck(int deckSize = 78);

    /// <summary>
    /// Sinh cờ bool ngẫu nhiên bằng nguồn CSPRNG.
    /// </summary>
    bool NextBoolean();

    /// <summary>
    /// Chọn một phần tử theo trọng số để phục vụ quay gacha có xác suất cấu hình.
    /// Luồng xử lý: tính tổng trọng số từ items, bốc mẫu ngẫu nhiên, trả item trúng kèm seed audit.
    /// </summary>
    GachaRngResult WeightedSelect(System.Collections.Generic.IEnumerable<WeightedItem> items, string? seedForAudit = null);
}

// Item trọng số đầu vào cho thuật toán chọn ngẫu nhiên có tỷ lệ.
public class WeightedItem
{
    // Định danh phần thưởng cần bốc chọn.
    public System.Guid ItemId { get; set; }

    // Trọng số theo basis points để tránh sai số số thực.
    public int WeightBasisPoints { get; set; }
}

// Kết quả chọn ngẫu nhiên cho gacha gồm item trúng và seed truy vết.
public class GachaRngResult
{
    // Định danh phần thưởng được chọn.
    public System.Guid SelectedItemId { get; set; }

    // Seed dùng để audit quy trình random.
    public string RngSeed { get; set; } = string.Empty;
}
