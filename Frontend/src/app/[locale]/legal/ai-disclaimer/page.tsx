import { useTranslations } from "next-intl";
import { getTranslations } from "next-intl/server";

export async function generateMetadata({ params }: { params: Promise<{ locale: string }> }) {
 const { locale } = await params;
 const t = await getTranslations({ locale, namespace: "Legal" });
 return {
  title: `${t("ai_disclaimer_title")} | TarotNow`,
  description: t("ai_disclaimer_meta_desc"),
 };
}

export default function AiDisclaimerPage() {
 const t = useTranslations("Legal");

 return (
 <div className="container mx-auto max-w-4xl px-4 pb-12 pt-20 md:pt-24">
 <h1 className="text-4xl font-extrabold tn-text-primary mb-8 text-center drop-shadow-lg">
 {t("ai_disclaimer_title")}
 </h1>
 <div className="tn-surface-strong rounded-2xl p-8 shadow-2xl border tn-border tn-text-secondary leading-relaxed font-sans space-y-6">
 <div className="bg-[var(--warning)]/10 border-l-4 border-[var(--warning)] p-4 mb-6 rounded-r">
 <p className="font-semibold text-[var(--warning)]">
 {t("ai_disclaimer.warning")}
 </p>
 </div>

 <section>
 <h2 className="text-2xl font-bold tn-text-primary mb-4">{t("ai_disclaimer.section1_title")}</h2>
 <p>{t("ai_disclaimer.section1_body")}</p>
 </section>

 <section>
 <h2 className="text-2xl font-bold tn-text-primary mb-4">{t("ai_disclaimer.section2_title")}</h2>
 <p>{t("ai_disclaimer.section2_body")}</p>
 </section>

 <p className="text-sm tn-text-muted pt-8 border-t tn-border">
 {t("ai_disclaimer.consent_note")}
 </p>
 </div>
 </div>
 );
}
