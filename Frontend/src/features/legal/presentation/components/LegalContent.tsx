import { cn } from "@/lib/utils";

/**
 * Component chứa nội dung Điều khoản dịch vụ chi tiết cho TarotNow AI
 */
export function TermsOfServiceContent() {
  return (
    <div className={cn("space-y-4 text-sm tn-text-secondary leading-relaxed")}>
      <section>
        <h3 className={cn("text-base font-bold tn-text-primary mb-2")}>1. Chấp nhận Điều khoản</h3>
        <p>
          Bằng cách đăng ký và sử dụng TarotNow AI, bạn đồng ý tuân thủ các Điều khoản dịch vụ này.
          Nếu bạn không đồng ý với bất kỳ phần nào của các điều khoản, vui lòng ngừng sử dụng dịch vụ ngay lập tức.
        </p>
      </section>

      <section>
        <h3 className={cn("text-base font-bold tn-text-primary mb-2")}>2. Dịch vụ AI & Miễn trừ trách nhiệm</h3>
        <p>Dịch vụ của chúng tôi cung cấp các kiến giải Tarot dựa trên trí tuệ nhân tạo (AI). Bạn cần hiểu rõ rằng:</p>
        <ul className={cn("list-disc pl-5 mt-2 space-y-2")}>
          <li>
            <span className={cn("font-semibold tn-text-primary")}>Tính chất tham khảo:</span> Kết quả giải bài chỉ mang tính chất chiêm nghiệm, tham khảo và giải trí.
          </li>
          <li>
            <span className={cn("font-semibold tn-text-primary")}>Không thay thế chuyên gia:</span> AI không phải là chuyên gia và không thể cung cấp những lời khuyên chính xác về Y tế, Pháp lý hoặc Tài chính.
          </li>
          <li>
            <span className={cn("font-semibold tn-text-primary")}>Trách nhiệm cá nhân:</span> TarotNow không chịu trách nhiệm về bất kỳ quyết định, hành động nào của người dùng dựa trên các thông tin từ hệ thống.
          </li>
        </ul>
      </section>

      <section>
        <h3 className={cn("text-base font-bold tn-text-primary mb-2")}>3. Tài khoản & Bảo mật</h3>
        <p>
          Bạn chịu trách nhiệm hoàn toàn về việc bảo mật mật khẩu của mình. Bất kỳ hoạt động nào diễn ra dưới tài khoản của bạn sẽ được coi là do bạn thực hiện.
          Chúng tôi có quyền tạm dừng hoặc xóa tài khoản nếu phát hiện hành vi vi phạm pháp luật hoặc cố ý phá hoại hệ thống.
        </p>
      </section>

      <section>
        <h3 className={cn("text-base font-bold tn-text-primary mb-2")}>4. Kim cương & Giao dịch</h3>
        <p>
          Kim cương là đơn vị tiền tệ ảo dùng để truy cập các dịch vụ cao cấp.
          Các giao dịch mua Kim cương là không thể hoàn trả sau khi đã thực hiện thành công, trừ trường hợp lỗi hệ thống được xác minh.
          Chúng tôi cam kết sử dụng cơ chế bảo mật giao dịch (Escrow) để đảm bảo quyền lợi tối đa cho người dùng.
        </p>
      </section>

      <div className={cn("pt-4 border-t tn-border tn-text-muted text-xs italic")}>
        Cập nhật lần cuối: 12 tháng 04, 2026
      </div>
    </div>
  );
}

/**
 * Component chứa nội dung Chính sách bảo mật chi tiết cho TarotNow AI
 */
export function PrivacyPolicyContent() {
  return (
    <div className={cn("space-y-4 text-sm tn-text-secondary leading-relaxed")}>
      <section>
        <h3 className={cn("text-base font-bold tn-text-primary mb-2")}>1. Thu thập dữ liệu</h3>
        <p>Chúng tôi chỉ thu thập các thông tin cần thiết để vận hành và cải thiện dịch vụ:</p>
        <ul className={cn("list-disc pl-5 mt-2 space-y-2")}>
          <li><span className={cn("font-semibold tn-text-primary")}>Thông tin cơ bản:</span> Email và Tên hiển thị khi bạn đăng ký.</li>
          <li><span className={cn("font-semibold tn-text-primary")}>Dữ liệu trải bài:</span> Các câu hỏi và lá bài bạn nhận được để AI phân tích và lưu trữ lịch sử cho bạn.</li>
          <li><span className={cn("font-semibold tn-text-primary")}>Hành vi sử dụng:</span> Cách bạn tương tác với AI để chúng tôi tối ưu hóa trải nghiệm.</li>
        </ul>
      </section>

      <section>
        <h3 className={cn("text-base font-bold tn-text-primary mb-2")}>2. Bảo vệ dữ liệu</h3>
        <p>
          Mọi dữ liệu cá nhân của bạn đều được mã hóa theo tiêu chuẩn công nghiệp và lưu trữ trên hệ thống máy chủ an toàn.
          Chúng tôi cam kết <strong>tuyệt đối không bán</strong>, không chia sẻ thông tin cá nhân của bạn cho bên thứ ba vì bất kỳ mục đích thương mại nào mà không được sự đồng ý của bạn.
        </p>
      </section>

      <section>
        <h3 className={cn("text-base font-bold tn-text-primary mb-2")}>3. Quyền xóa dữ liệu</h3>
        <p>
          Người dùng có quyền yêu cầu xem, chỉnh sửa hoặc xóa vĩnh viễn toàn bộ dữ liệu cá nhân của mình trên TarotNow.
          Sau khi yêu cầu xóa được thực thi, mọi dữ liệu lịch sử trải bài của bạn sẽ không thể khôi phục.
        </p>
      </section>

      <div className={cn("pt-4 border-t tn-border tn-text-muted text-xs italic")}>
        Cập nhật lần cuối: 12 tháng 04, 2026
      </div>
    </div>
  );
}
