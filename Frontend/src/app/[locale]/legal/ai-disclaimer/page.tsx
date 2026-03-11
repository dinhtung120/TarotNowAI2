import { useTranslations } from "next-intl";
import { getTranslations } from "next-intl/server";

export async function generateMetadata({ params: { locale } }: { params: { locale: string } }) {
    const t = await getTranslations({ locale, namespace: "Legal" });
    return {
        title: `${t("ai_disclaimer_title")} | TarotNow`,
        description: "Miễn trừ trách nhiệm về thông tin do AI tạo ra của TarotNow.",
    };
}

export default function AiDisclaimerPage() {
    const t = useTranslations("Legal");

    return (
        <div className="container mx-auto px-4 py-12 max-w-4xl">
            <h1 className="text-4xl font-extrabold text-[#DFF2CB] mb-8 text-center drop-shadow-lg">
                {t("ai_disclaimer_title")}
            </h1>
            <div className="bg-[#1A1F2B]/80 backdrop-blur-md rounded-2xl p-8 shadow-2xl border border-[#2D3748] text-gray-300 leading-relaxed font-sans space-y-6">
                <div className="bg-orange-500/10 border-l-4 border-orange-500 p-4 mb-6 rounded-r">
                    <p className="font-semibold text-orange-400">
                        Khuyến cáo: Các dự đoán và phân tích từ Tinh Tú AI được tạo hoàn toàn bởi mô hình ngôn ngữ lớn (LLM). Vui lòng không coi đây là một lời khuyên thay thế cho tư vấn tâm lý y khoa hay chuyên gia tài chính.
                    </p>
                </div>

                <section>
                    <h2 className="text-2xl font-bold text-[#DFF2CB] mb-4">1. Đặc tính của AI</h2>
                    <p>
                        Tất cả các thông điệp Tarot được diễn giải qua trí tuệ nhân tạo (AI). Mặc dù chúng tôi đã thiết lập thuật toán tối ưu hóa logic huyền học và trực giác Tarot, kết quả giải nghĩa của AI có thể không đúng 100% trong mọi trường hợp và ngữ cảnh.
                    </p>
                </section>

                <section>
                    <h2 className="text-2xl font-bold text-[#DFF2CB] mb-4">2. Không phải là chẩn đoán y khoa</h2>
                    <p>
                        Kết quả xem bài liên quan đến Cảm xúc, Sức khỏe hay Tiền bạc không được dùng làm căn cứ tự chẩn đoán y tế hoặc thay thế luật pháp/tài chính. Hãy luôn tham khảo ý kiến chuyên gia ngoài đời thực nếu bạn đang gặp vấn đề nghiêm trọng.
                    </p>
                </section>

                <p className="text-sm text-gray-500 pt-8 border-t border-[#2D3748]">
                    Bạn cần đồng ý với điều khoản nhận thức này trước khi đăng ký tài khoản TarotNow.
                </p>
            </div>
        </div>
    );
}
