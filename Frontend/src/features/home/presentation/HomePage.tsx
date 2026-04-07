import { CoreFeaturesSection } from "@/features/home/presentation/components/CoreFeaturesSection";
import { FeaturedReadersSection } from "@/features/home/presentation/components/FeaturedReadersSection";
import { FinalCtaSection } from "@/features/home/presentation/components/FinalCtaSection";
import { HeroSection } from "@/features/home/presentation/components/HeroSection";
import { StatsSection } from "@/features/home/presentation/components/StatsSection";
import AstralBackground from "@/shared/components/layout/AstralBackground";
import Footer from "@/shared/components/layout/Footer";
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
