import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { ChevronRight, Sparkles } from "lucide-react";
import { getTranslations } from "next-intl/server";
import { cn } from "@/lib/utils";
import { Button } from "@/shared/components/ui";

export async function FinalCtaSection() {
  const t = await getTranslations("Index");

  return (
    <section
      className={cn(
        "relative flex flex-col items-center overflow-hidden px-4 py-40 sm:px-6",
      )}
    >
      <div
        className={cn(
          "relative z-10 flex max-w-4xl flex-col items-center text-center",
        )}
      >
        <Sparkles
          className={cn(
            "mb-10 h-16 w-16 animate-pulse text-[var(--purple-accent)]",
          )}
        />
        <h2
          className={cn(
            "mb-10 font-serif text-4xl font-black tracking-tighter text-[var(--text-ink)] uppercase md:text-6xl",
          )}
        >
          {t("final.title1")} <br />
          <span className={cn("tn-text-secondary italic")}>
            {t("final.title2")}
          </span>
        </h2>
        <Link href="/reading" tabIndex={-1}>
          <Button
            size="lg"
            className={cn("group rounded-3xl px-12 py-6")}
            rightIcon={
              <ChevronRight
                className={cn(
                  "h-5 w-5 transition-transform group-hover:translate-x-2",
                )}
              />
            }
          >
            {t("final.cta")}
          </Button>
        </Link>
      </div>
    </section>
  );
}
