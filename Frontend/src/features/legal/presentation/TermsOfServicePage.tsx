
import { useTranslations } from "next-intl";
import { getTranslations } from "next-intl/server";
import { cn } from '@/lib/utils';

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
 <div className={cn("container mx-auto max-w-4xl px-4 pb-12 pt-20 md:pt-24")}>
 <h1 className={cn("text-4xl font-extrabold tn-text-primary mb-8 text-center drop-shadow-lg")}>
 {t("tos_title")}
 </h1>
 <div className={cn("tn-surface-strong rounded-2xl p-8 shadow-2xl border tn-border tn-text-secondary leading-relaxed font-sans space-y-6")}>
 <section>
 <h2 className={cn("text-2xl font-bold tn-text-primary mb-4")}>{t("tos.section1_title")}</h2>
 <p>{t("tos.section1_body")}</p>
 </section>

 <section>
 <h2 className={cn("text-2xl font-bold tn-text-primary mb-4")}>{t("tos.section2_title")}</h2>
 <p>{t("tos.section2_body")}</p>
 </section>

 <section>
 <h2 className={cn("text-2xl font-bold tn-text-primary mb-4")}>{t("tos.section3_title")}</h2>
 <p>{t("tos.section3_body")}</p>
 </section>

 <p className={cn("text-sm tn-text-muted pt-8 border-t tn-border")}>
 {t("tos.last_updated")}
 </p>
 </div>
 </div>
 );
}
