import { CoreFeaturesSection } from "@/features/home/landing/components/CoreFeaturesSection";
import { FeaturedReadersSection } from "@/features/home/landing/components/FeaturedReadersSection";
import { FinalCtaSection } from "@/features/home/landing/components/FinalCtaSection";
import { HeroSection } from "@/features/home/landing/components/HeroSection";
import { StatsSection } from "@/features/home/landing/components/StatsSection";
import AstralBackground from "@/shared/app-shell/layout/AstralBackground";
import Footer from "@/features/home/shared/app-shell/layout/Footer";
import { cn } from "@/lib/utils";

export default async function Home() {
  return (
    <div
      className={cn(
        "min-h-dvh overflow-x-hidden bg-[var(--bg-void)] font-sans text-[var(--text-primary)]",
      )}
    >
      <AstralBackground variant="intense" />
      <main className={cn("relative z-10")}>
        <HeroSection />
        <StatsSection />
        <FeaturedReadersSection />
        <CoreFeaturesSection />
        <FinalCtaSection />
      </main>
      <Footer />
    </div>
  );
}
