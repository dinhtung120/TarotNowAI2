import { useTranslations } from "next-intl";
import { getTranslations } from "next-intl/server";

export async function generateMetadata({ params: { locale } }: { params: { locale: string } }) {
 const t = await getTranslations({ locale, namespace: "Legal" });
 return {
 title: `${t("privacy_title")} | TarotNow`,
 description: "Chính sách bảo mật dữ liệu của TarotNow.",
 };
}

export default function PrivacyPolicyPage() {
 const t = useTranslations("Legal");

 return (
 <div className="container mx-auto px-4 py-12 max-w-4xl">
 <h1 className="text-4xl font-extrabold tn-text-primary mb-8 text-center drop-shadow-lg">
 {t("privacy_title")}
 </h1>
 <div className="tn-surface-strong rounded-2xl p-8 shadow-2xl border tn-border tn-text-secondary leading-relaxed font-sans space-y-6">
 <section>
 <h2 className="text-2xl font-bold tn-text-primary mb-4">1. Thu thập thông tin</h2>
 <p>
 TarotNow tôn trọng quyền riêng tư của bạn. Chúng tôi thu thập các thông tin cần thiết như Địa chỉ Email, Tên hiển thị, và Ngày sinh nhằm cá nhân hóa trải nghiệm đọc bài Tarot, tính toán Cung Hoàng Đạo và Thần Số Học.
 </p>
 </section>

 <section>
 <h2 className="text-2xl font-bold tn-text-primary mb-4">2. Sử dụng dữ liệu</h2>
 <p>
 Dữ liệu của bạn được sử dụng mục đích nội bộ để lưu trữ lịch sử xem bài (Reading History). Lịch sử truy vấn AI được gửi một cách mã hóa đến nhà cung cấp (OpenAI) để thu thập kết quả và không nhằm mục đích huấn luyện AI (training) theo cam kết của đối tác.
 </p>
 </section>

 <section>
 <h2 className="text-2xl font-bold tn-text-primary mb-4">3. Bảo vệ dữ liệu</h2>
 <p>
 Tất cả thông tin tài khoản và mật khẩu được mã hóa an toàn. Chúng tôi không mua bán, trao đổi dữ liệu cá nhân của người dùng cho bên thứ ba dưới mọi hình thức vì mục đích thương mại.
 </p>
 </section>

 <p className="text-sm tn-text-muted pt-8 border-t tn-border">
 Cập nhật lần cuối: 10/2026
 </p>
 </div>
 </div>
 );
}
