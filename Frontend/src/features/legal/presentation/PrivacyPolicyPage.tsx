/*
 * ===================================================================
 * FILE: legal/privacy/page.tsx (Chính Sách Bảo Mật - Privacy Policy)
 * BỐI CẢNH (CONTEXT):
 *   Trang hiển thị nội dung Chính Sách Bảo Mật Dữ Liệu hệ thống TarotNow.
 * 
 * TỐI ƯU SEO & RENDERING:
 *   - `generateMetadata`: Khai báo SEO đa ngôn ngữ trên Server.
 *   - Hoàn toàn render phía máy chủ (SSR) đảm bảo tốc độ và truy xuất Bot ổn định.
 * ===================================================================
 */
import { useTranslations } from "next-intl";
import { getTranslations } from "next-intl/server";

export async function generateMetadata({ params }: { params: Promise<{ locale: string }> }) {
 const { locale } = await params;
 const t = await getTranslations({ locale, namespace: "Legal" });
 return {
  title: `${t("privacy_title")} | TarotNow`,
  description: t("privacy_meta_desc"),
 };
}

export default function PrivacyPolicyPage() {
 const t = useTranslations("Legal");

 return (
 <div className="container mx-auto max-w-4xl px-4 pb-12 pt-20 md:pt-24">
 <h1 className="text-4xl font-extrabold tn-text-primary mb-8 text-center drop-shadow-lg">
 {t("privacy_title")}
 </h1>
 <div className="tn-surface-strong rounded-2xl p-8 shadow-2xl border tn-border tn-text-secondary leading-relaxed font-sans space-y-6">
 <section>
 <h2 className="text-2xl font-bold tn-text-primary mb-4">{t("privacy.section1_title")}</h2>
 <p>{t("privacy.section1_body")}</p>
 </section>

 <section>
 <h2 className="text-2xl font-bold tn-text-primary mb-4">{t("privacy.section2_title")}</h2>
 <p>{t("privacy.section2_body")}</p>
 </section>

 <section>
 <h2 className="text-2xl font-bold tn-text-primary mb-4">{t("privacy.section3_title")}</h2>
 <p>{t("privacy.section3_body")}</p>
 </section>

 <p className="text-sm tn-text-muted pt-8 border-t tn-border">
 {t("privacy.last_updated")}
 </p>
 </div>
 </div>
 );
}
