/*
 * ===================================================================
 * FILE: legal/tos/page.tsx (Điều Khoản Dịch Vụ - Terms of Service)
 * BỐI CẢNH (CONTEXT):
 *   Trang hiển thị nội dung Điều Khoản Dịch Vụ của hệ thống TarotNow.
 * 
 * TỐI ƯU SEO & RENDERING:
 *   - `generateMetadata`: Tải trước meta title/description đa ngôn ngữ trên Server.
 *   - Server Component hoàn toàn, render siêu tốc vì chỉ chứa văn bản tĩnh.
 * ===================================================================
 */
import { useTranslations } from "next-intl";
import { getTranslations } from "next-intl/server";

export async function generateMetadata({ params }: { params: Promise<{ locale: string }> }) {
 const { locale } = await params;
 const t = await getTranslations({ locale, namespace: "Legal" });
 return {
  title: `${t("tos_title")} | TarotNow`,
  description: t("tos_meta_desc"),
 };
}

export default function TermsOfServicePage() {
 const t = useTranslations("Legal");

 return (
 <div className="container mx-auto max-w-4xl px-4 pb-12 pt-20 md:pt-24">
 <h1 className="text-4xl font-extrabold tn-text-primary mb-8 text-center drop-shadow-lg">
 {t("tos_title")}
 </h1>
 <div className="tn-surface-strong rounded-2xl p-8 shadow-2xl border tn-border tn-text-secondary leading-relaxed font-sans space-y-6">
 <section>
 <h2 className="text-2xl font-bold tn-text-primary mb-4">{t("tos.section1_title")}</h2>
 <p>{t("tos.section1_body")}</p>
 </section>

 <section>
 <h2 className="text-2xl font-bold tn-text-primary mb-4">{t("tos.section2_title")}</h2>
 <p>{t("tos.section2_body")}</p>
 </section>

 <section>
 <h2 className="text-2xl font-bold tn-text-primary mb-4">{t("tos.section3_title")}</h2>
 <p>{t("tos.section3_body")}</p>
 </section>

 <p className="text-sm tn-text-muted pt-8 border-t tn-border">
 {t("tos.last_updated")}
 </p>
 </div>
 </div>
 );
}
