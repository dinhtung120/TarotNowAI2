/*
 * ===================================================================
 * FILE: page.tsx (Trang Chủ Astral Premium)
 * BỐI CẢNH (CONTEXT):
 *   Landing page tĩnh hiển thị giới thiệu, thông số và Reader nổi bật.
 * 
 * RENDERING (SERVER COMPONENT):
 *   Render nội dung từ Server với cấu trúc HTML tĩnh có sẵn SEO. 
 *   Sử dụng Suspense để loading danh sách Reader song song mà không khóa
 *   tiến trình render nội dung phía trên.
 * ===================================================================
 */
import { Link } from "@/i18n/routing";
import { Star, ChevronRight, ShieldCheck, Flame, Compass, Zap, Users, ArrowUpRight, Award, LucideIcon, Gem, Sparkles
} from "lucide-react";
import { getTranslations } from "next-intl/server";
import { Suspense } from "react";
import { listFeaturedReaders, type ReaderProfile } from "@/features/reader/public";
import { normalizeReaderStatus } from "@/features/reader/domain/readerStatus";

import AstralBackground from "@/shared/components/layout/AstralBackground";
import Footer from "@/shared/components/layout/Footer";
import { Button, GlassCard, SectionHeader, Badge } from "@/shared/components/ui";

/**
 * Trang Chủ Redesign - Phong cách Ultra Premium Astral
 * * Các nâng cấp chính:
 * 1. Typography: Kết hợp serif (Playfair Display) cho tiêu đề và sans-serif cho nội dung.
 * 2. Sử dụng Design System: AstralBackground, Footer, SectionHeader, Button...
 * 3. Loại bỏ inline CSS/animations (đã move sang globals.css).
 */

interface StatProps {
 icon: LucideIcon;
 value: string;
 label: string;
 color: "purple" | "amber" | "success" | "info";
}

function StatItem({ icon: Icon, value, label, color }: StatProps) {
 const styleByColor: Record<StatProps["color"], { bg: string; accent: string }> = {
  purple: { bg: "var(--purple-50)", accent: "var(--purple-accent)" },
  amber: { bg: "var(--amber-50)", accent: "var(--amber-accent)" },
  success: { bg: "var(--success-bg)", accent: "var(--success)" },
  info: { bg: "var(--info-bg)", accent: "var(--info)" },
 };
 const styles = styleByColor[color];

 return (
 <GlassCard variant="elevated" className="flex flex-col items-center">
 <div className={`w-10 h-10 rounded-2xl flex items-center justify-center mb-4 border border-[var(--border-subtle)]`}
 style={{
 backgroundColor: styles.bg,
 borderColor: `color-mix(in srgb, ${styles.accent} 20%, transparent)`,
 color: styles.accent
 }}>
 <Icon className="w-5 h-5" />
 </div>
 <div className="text-2xl font-black text-[var(--text-ink)] tracking-tighter italic mb-1">{value}</div>
 <div className="text-[9px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)]">{label}</div>
 </GlassCard>
 );
}

function FeaturedReadersFallback() {
 return (
 <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
 {Array.from({ length: 4 }).map((_, index) => (
 <div
 key={`featured-reader-skeleton-${index}`}
 className="h-96 rounded-[2.5rem] border border-[var(--border-default)] bg-[var(--bg-surface)] animate-pulse"
 />
 ))}
 </div>
 );
}

async function FeaturedReadersGrid() {
 const t = await getTranslations("Index");
 const featuredReadersResult = await listFeaturedReaders(4);
 const featuredReaders =
  featuredReadersResult.success && featuredReadersResult.data
   ? featuredReadersResult.data
   : [];

 if (featuredReaders.length === 0) {
 return <FeaturedReadersFallback />;
 }

 return (
 <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
 {featuredReaders.map((reader: ReaderProfile) => {
 const readerStatus = normalizeReaderStatus(reader.status);
 const statusIndicatorClassName =
  readerStatus === 'online'
   ? 'bg-[var(--success)] animate-pulse'
   : readerStatus === 'busy'
    ? 'bg-[var(--warning)]'
    : 'bg-[var(--text-muted)]';

 return (
 <Link key={reader.userId} href={`/readers/${reader.userId}`}
 className="group relative h-96 rounded-[2.5rem] overflow-hidden border border-[var(--border-default)] bg-[var(--bg-surface)] hover:border-[var(--border-focus)] transition-all duration-700 hover:-translate-y-4 shadow-[var(--shadow-card)] preserve-3d"
 >
 {/* Avatar ảnh nền — hiển thị ảnh đại diện reader phủ kín card */}
 {reader.avatarUrl ? (
 <img
  src={reader.avatarUrl}
  alt={reader.displayName}
  className="absolute inset-0 w-full h-full object-cover z-0 transition-transform duration-700 group-hover:scale-110"
 />
 ) : (
 <div className="absolute inset-0 w-full h-full z-0 flex items-center justify-center bg-gradient-to-br from-[var(--purple-accent)]/20 to-[var(--bg-surface)]">
  <span className="text-6xl font-black text-[var(--text-muted)]/30 italic uppercase select-none">
  {reader.displayName?.charAt(0) || '?'}
  </span>
 </div>
 )}
 {/* Gradient Overlay */}
 <div className="absolute inset-0 bg-gradient-to-t from-[var(--bg-void)] via-[var(--bg-void)]/40 to-transparent z-10" />
 <div className="absolute inset-0 bg-[var(--purple-glow)] opacity-0 group-hover:opacity-100 transition-opacity duration-700 z-10" />
 {/* Reader Meta */}
 <div className="absolute inset-x-0 bottom-0 p-8 z-20 space-y-4">
 <div className="flex items-center justify-between">
 <div className={`w-3 h-3 rounded-full ${statusIndicatorClassName}`} />
 <Badge variant="default" size="sm" className="bg-[var(--bg-glass)] border-[var(--border-default)]">
 <Star className="w-3 h-3 text-[var(--amber-accent)] fill-[var(--amber-accent)]" />
 <span className="tn-text-primary">{reader.avgRating.toFixed(1)}</span>
 </Badge>
 </div>
 <div>
 <h3 className="text-xl font-black text-[var(--text-ink)] tracking-tighter uppercase italic truncate">{reader.displayName}</h3>
 <div className="text-[9px] font-black uppercase tracking-[0.2em] text-[var(--purple-accent)] mt-1 line-clamp-1">
 {reader.specialties.join(' • ')}
 </div>
 </div>

 <div className="pt-4 border-t border-[var(--border-subtle)] flex items-center justify-between">
 <div className="flex items-center gap-1.5">
 <Gem className="w-3.5 h-3.5 text-[var(--amber-accent)]" />
 <span className="text-xs font-black text-[var(--text-ink)]">{reader.diamondPerQuestion} 💎</span>
 </div>
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)] group-hover:translate-x-2 transition-transform">{t('showcase.profileCta')}</div>
 </div>
 </div>
 </Link>
 );
 })}
 </div>
 );
}

export default async function Home() {
 const t = await getTranslations("Index");

 return (
 <div className="min-h-dvh bg-[var(--bg-void)] text-[var(--text-primary)] overflow-x-hidden font-sans">
 {/* ##### HỆ THỐNG NỀN THIÊN HÀ (NEBULA SYSTEM) ##### */}
 <AstralBackground variant="intense" />

 <main className="relative z-10">
 {/* ##### HERO SECTION: THE MYSTIC PORTAL ##### */}
 <section className="relative min-h-dvh flex flex-col items-center justify-center px-4 sm:px-6 pt-20 pb-28 md:pb-24">
 <div className="w-full max-w-6xl flex flex-col items-center text-center">
 {/* Live Tagline */}
 <div className="inline-flex items-center gap-3 px-4 py-2 rounded-full bg-[var(--bg-glass)] border border-[var(--border-default)] text-[10px] uppercase tracking-[0.3em] font-black text-[color:var(--c-hex-9f8338)] mb-10 shadow-[var(--shadow-card)] animate-in fade-in slide-in-from-bottom-4 duration-1000">
 <span className="relative flex h-2 w-2">
 <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-[var(--amber-accent)] opacity-75"></span>
 <span className="relative inline-flex rounded-full h-2 w-2 bg-[var(--amber-accent)]"></span>
 </span>
 {t('tagline')}
 </div>

 {/* Main Title - Using Playfair Display */}
 <h1 className="relative mb-10 animate-in fade-in zoom-in-95 duration-1000 delay-200">
 <span className="block font-serif italic text-3xl sm:text-5xl lg:text-6xl font-light tracking-tight text-[var(--text-secondary)] mb-2 leading-tight">
 {t('heroTitle1')}
 </span>
 <span className="block text-5xl sm:text-7xl lg:text-8xl font-black italic tracking-tighter text-transparent bg-clip-text bg-gradient-to-b from-[color:var(--c-hex-8f7bb4)] via-[var(--purple-accent)] to-[var(--mint-accent)] leading-none pb-4 drop-shadow-[0_20px_40px_var(--c-168-156-255-30)]">
 {t('heroTitle2')}
 </span>
 <div className="absolute -bottom-2 left-1/2 -translate-x-1/2 w-40 h-[1px] bg-gradient-to-r from-transparent via-[color:var(--c-168-156-255-50)] to-transparent"></div>
 </h1>

 {/* Description */}
 <p className="max-w-2xl text-base sm:text-lg text-[var(--text-secondary)] font-medium mb-14 leading-relaxed tracking-wide animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-500">
 {t('heroDesc')}
 </p>

 {/* Hero Actions */}
 <div className="relative z-10 flex flex-col sm:flex-row items-center gap-6 w-full sm:w-auto animate-in fade-in slide-in-from-bottom-12 duration-1000 delay-700">
 <Link href="/reading" tabIndex={-1}>
 <Button size="lg" rightIcon={<Compass className="w-5 h-5 ml-2 transition-transform duration-700 group-hover:rotate-180" />}
 className="group shadow-[var(--glow-white)]"
 >
 {t('ctaDraw')}
 </Button>
 </Link>

 <Link href="/readers" tabIndex={-1}>
 <Button variant="secondary" size="lg"
 rightIcon={<Users className="w-5 h-5 ml-2 transition-transform duration-300 group-hover:translate-x-1" />}
 className="group"
 >
 {t('ctaMeetReaders')}
 </Button>
 </Link>
 </div>
 </div>

 {/* Scroll Indicator */}
 <div className="pointer-events-none absolute bottom-4 left-1/2 z-0 hidden -translate-x-1/2 flex-col items-center gap-4 opacity-50 md:flex">
 <div className="text-[9px] font-black uppercase tracking-[0.4em] text-[var(--text-muted)] rotate-90 origin-left translate-x-1.5 translate-y-2">{t('scroll')}</div>
 <div className="w-[1px] h-16 bg-gradient-to-b from-[var(--purple-accent)] to-transparent"></div>
 </div>
 </section>

 {/* ##### STATS SECTION: UNIVERSE GROWTH ##### */}
 <section className="relative w-full max-w-6xl mx-auto px-4 sm:px-6 py-20">
 <div className="grid grid-cols-2 md:grid-cols-4 gap-4 md:gap-8">
 <StatItem icon={Zap} value="50K+" label={t('stats.readings')} color="purple" />
 <StatItem icon={Users} value="120+" label={t('stats.readers')} color="amber" />
 <StatItem icon={Award} value="4.9" label={t('stats.rating')} color="success" />
 <StatItem icon={Flame} value="24/7" label={t('stats.support')} color="info" />
 </div>
 </section>

 {/* ##### READER SHOWCASE: THE GUIDES ##### */}
 <section className="relative w-full max-w-7xl mx-auto px-4 sm:px-6 py-32">
 <SectionHeader
 tag={t('showcase.tag')}
 title={t('showcase.title')}
 titleMuted={t('showcase.titleMuted')}
 action={
 <Link href="/readers" className="text-xs font-black uppercase tracking-widest text-[var(--text-secondary)] hover:text-[var(--text-ink)] transition-colors inline-flex items-center gap-2 group min-h-11 px-2 rounded-xl hover:bg-[var(--purple-50)]">
 {t('showcase.viewAll')} <ArrowUpRight className="w-4 h-4 transition-transform group-hover:translate-x-1 group-hover:-translate-y-1" />
 </Link>
 }
 className="mb-20"
 />

 <Suspense fallback={<FeaturedReadersFallback />}>
 <FeaturedReadersGrid />
 </Suspense>
 </section>

 {/* ##### CORE FEATURES: THE ASTRAL ARCHITECTURE ##### */}
 <section className="relative w-full max-w-6xl mx-auto px-4 sm:px-6 py-32">
 <div className="grid grid-cols-1 md:grid-cols-3 gap-px bg-[var(--border-subtle)] border border-[var(--border-default)] rounded-[3rem] overflow-hidden shadow-[var(--shadow-elevated)] ">
 {/* Feature 1: AI Insight */}
 <div className="group relative p-8 sm:p-12 flex flex-col gap-10 bg-[var(--bg-glass)] hover:bg-[var(--purple-50)] transition-all duration-700 border-b md:border-b-0 md:border-r border-[var(--border-subtle)]">
 <div className="w-14 h-14 rounded-2xl bg-[var(--bg-elevated)] flex items-center justify-center border border-[var(--border-default)] transition-all duration-700 group-hover:scale-110 group-hover:bg-[var(--purple-accent)] animate-glow-pulse">
 <Zap className="w-6 h-6 group-hover:text-[var(--text-ink)] transition-colors" />
 </div>
 <div className="space-y-4">
 <h3 className="text-xl font-black text-[var(--text-ink)] italic tracking-tighter uppercase">{t('f1Title')}</h3>
 <p className="text-[var(--text-secondary)] text-sm leading-relaxed font-semibold transition-colors duration-500 group-hover:text-[var(--text-ink)]">
 {t('f1Desc')}
 </p>
 </div>
 </div>

 {/* Feature 2: Personalized Path */}
 <div className="group relative p-8 sm:p-12 flex flex-col gap-10 bg-[var(--bg-glass)] hover:bg-[var(--amber-50)] transition-all duration-700 border-b md:border-b-0 md:border-r border-[var(--border-subtle)]">
 <div className="w-14 h-14 rounded-2xl bg-[var(--bg-elevated)] flex items-center justify-center border border-[var(--border-default)] transition-all duration-700 group-hover:scale-110 group-hover:bg-[var(--amber-accent)] animate-glow-pulse">
 <Flame className="w-6 h-6 group-hover:text-[var(--text-ink)] transition-colors" />
 </div>
 <div className="space-y-4">
 <h3 className="text-xl font-black text-[var(--text-ink)] italic tracking-tighter uppercase">{t('f2Title')}</h3>
 <p className="text-[var(--text-secondary)] text-sm leading-relaxed font-semibold transition-colors duration-500 group-hover:text-[var(--text-ink)]">
 {t('f2Desc')}
 </p>
 </div>
 </div>

 {/* Feature 3: Transparent Protocol */}
 <div className="group relative p-8 sm:p-12 flex flex-col gap-10 bg-[var(--bg-glass)] hover:bg-[var(--success-bg)] transition-all duration-700">
 <div className="w-14 h-14 rounded-2xl bg-[var(--bg-elevated)] flex items-center justify-center border border-[var(--border-default)] transition-all duration-700 group-hover:scale-110 group-hover:bg-[var(--success)] animate-glow-pulse">
 <ShieldCheck className="w-6 h-6 group-hover:text-[var(--text-ink)] transition-colors" />
 </div>
 <div className="space-y-4">
 <h3 className="text-xl font-black text-[var(--text-ink)] italic tracking-tighter uppercase">{t('f3Title')}</h3>
 <p className="text-[var(--text-secondary)] text-sm leading-relaxed font-semibold transition-colors duration-500 group-hover:text-[var(--text-ink)]">
 {t('f3Desc')}
 </p>
 </div>
 </div>
 </div>
 </section>

 {/* ##### FINAL CALL: RESONANCE ##### */}
 <section className="relative px-4 sm:px-6 py-40 flex flex-col items-center overflow-hidden">
 <div className="relative z-10 flex flex-col items-center text-center max-w-4xl">
 <Sparkles className="w-16 h-16 text-[var(--purple-accent)] mb-10 animate-pulse" />
 <h2 className="text-4xl md:text-6xl font-black text-[var(--text-ink)] mb-10 tracking-tighter uppercase font-serif">
 {t('final.title1')} <br/> <span className="text-[var(--text-secondary)] italic">{t('final.title2')}</span>
 </h2>
 <Link href="/reading" tabIndex={-1}>
 <Button size="lg" className="group px-12 py-6 rounded-3xl" rightIcon={<ChevronRight className="w-5 h-5 group-hover:translate-x-2 transition-transform" />}>
 {t('final.cta')}
 </Button>
 </Link>
 </div>
 </section>
 </main>

 <Footer />
 </div>
 );
}
