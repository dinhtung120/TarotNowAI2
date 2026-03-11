import { useTranslations } from "next-intl";
import { getTranslations } from "next-intl/server";

export async function generateMetadata({ params: { locale } }: { params: { locale: string } }) {
    const t = await getTranslations({ locale, namespace: "Legal" });
    return {
        title: `${t("tos_title")} | TarotNow`,
        description: "Điều khoản dịch vụ và sử dụng của TarotNow.",
    };
}

export default function TermsOfServicePage() {
    const t = useTranslations("Legal");

    return (
        <div className="container mx-auto px-4 py-12 max-w-4xl">
            <h1 className="text-4xl font-extrabold text-[#DFF2CB] mb-8 text-center drop-shadow-lg">
                {t("tos_title")}
            </h1>
            <div className="bg-[#1A1F2B]/80 backdrop-blur-md rounded-2xl p-8 shadow-2xl border border-[#2D3748] text-gray-300 leading-relaxed font-sans space-y-6">
                <section>
                    <h2 className="text-2xl font-bold text-[#DFF2CB] mb-4">1. Điều khoản chung</h2>
                    <p>
                        Chào mừng bạn đến với TarotNow. Khi sử dụng dịch vụ của chúng tôi, bạn đồng ý tuân thủ các điều khoản và điều kiện này.
                        Dịch vụ của chúng tôi cung cấp giải trí tư vấn tâm lý mang tính chất tham khảo qua các lá bài Tarot và công nghệ Trí Tuệ Nhân Tạo (AI).
                    </p>
                </section>

                <section>
                    <h2 className="text-2xl font-bold text-[#DFF2CB] mb-4">2. Miễn trừ trách nhiệm AI</h2>
                    <p>
                        Tất cả nội dung giải nghĩa bài Tarot đều được tạo ra bởi AI và chỉ mang tính chất giải trí, tham khảo.
                        Chúng tôi không chịu trách nhiệm pháp lý cho các quyết định cá nhân, y tế, tài chính hoặc pháp lý của người dùng dự vào kết quả từ TarotNow.
                    </p>
                </section>

                <section>
                    <h2 className="text-2xl font-bold text-[#DFF2CB] mb-4">3. Tài khoản và thanh toán</h2>
                    <p>
                        Người dùng chịu trách nhiệm bảo mật thông tin tài khoản của mình. Các giao dịch nạp Diamond không được hoàn tiền trừ trường hợp lỗi hệ thống phát sinh từ phía cổng thanh toán.
                    </p>
                </section>

                <p className="text-sm text-gray-500 pt-8 border-t border-[#2D3748]">
                    Cập nhật lần cuối: 10/2026
                </p>
            </div>
        </div>
    );
}
