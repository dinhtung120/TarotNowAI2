import { Link } from "@/i18n/routing";
import { Star, ChevronRight, ShieldCheck, Flame, Compass, Sparkle, Moon, Ghost, Zap } from "lucide-react";
import { getTranslations } from "next-intl/server";

export default async function Home() {
  const t = await getTranslations("Index");

  return (
    <div className="min-h-screen bg-[#020108] text-zinc-100 selection:bg-purple-500/40 overflow-hidden font-sans text-sm sm:text-base">
      {/* ===== HỆ THỐNG NỀN CAO CẤP (PREMIUM BACKGROUND SYSTEM) ===== */}
      <div className="fixed inset-0 z-0 pointer-events-none overflow-hidden">
        <div className="absolute inset-0 bg-[url('https://grainy-gradients.vercel.app/noise.svg')] opacity-[0.05] mix-blend-overlay"></div>
        
        <div className="absolute top-[-15%] left-[-10%] w-[70vw] h-[70vw] bg-purple-900/15 blur-[120px] rounded-full animate-[pulse_12s_infinite_ease-in-out]" />
        <div className="absolute top-[20%] right-[-15%] w-[60vw] h-[60vw] bg-indigo-900/20 blur-[150px] rounded-full animate-[pulse_10s_infinite_ease-in-out_1s]" />
        <div className="absolute bottom-[-20%] left-[5%] w-[50vw] h-[50vw] bg-fuchsia-900/15 blur-[130px] rounded-full animate-[pulse_15s_infinite_ease-in-out_2s]" />
        
        <div className="absolute inset-0">
          {Array.from({ length: 20 }).map((_, i) => (
            <div
              key={i}
              className="absolute w-1 h-1 bg-white rounded-full animate-float pointer-events-none opacity-[0.2]"
              style={{
                top: `${Math.random() * 100}%`,
                left: `${Math.random() * 100}%`,
                animationDuration: `${10 + Math.random() * 20}s`,
                animationDelay: `${-Math.random() * 20}s`,
                transform: `scale(${Math.random()})`,
              }}
            />
          ))}
        </div>

        <div className="absolute inset-0 opacity-[0.02] bg-[linear-gradient(to_right,#80808012_1px,transparent_1px),linear-gradient(to_bottom,#80808012_1px,transparent_1px)] bg-[size:40px_40px] [mask-image:radial-gradient(ellipse_60%_50%_at_50%_0%,#000_70%,transparent_100%)]"></div>
      </div>

      <main className="relative z-10">
        {/* ===== HERO SECTION: TYPOGRAPHY VÀ ÁNH SÁNG ===== */}
        <section className="relative min-h-[85vh] flex flex-col items-center justify-center px-6 pt-12 overflow-hidden">
          <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-full max-w-4xl aspect-square bg-purple-500/5 blur-[120px] rounded-full pointer-events-none"></div>

          <div className="relative z-10 w-full max-w-5xl flex flex-col items-center text-center">
            {/* Tagline Aura - Nhỏ lại */}
            <div className="inline-flex items-center gap-2.5 px-3.5 py-1.5 rounded-full bg-zinc-900/50 border border-zinc-800 text-[9px] uppercase tracking-[0.25em] font-bold text-amber-200/80 mb-8 shadow-xl backdrop-blur-md animate-in fade-in slide-in-from-bottom-3 duration-700">
              <span className="relative flex h-1.5 w-1.5">
                <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-amber-400 opacity-75"></span>
                <span className="relative inline-flex rounded-full h-1.5 w-1.5 bg-amber-500"></span>
              </span>
              {t('tagline')}
            </div>

            {/* Tiêu đề chính - Thu nhỏ bớt */}
            <h1 className="relative mb-8 animate-in fade-in zoom-in-95 duration-1000">
              <span className="block text-3xl sm:text-5xl lg:text-6xl font-light tracking-tight text-white mb-1 leading-none">
                {t('heroTitle1')}
              </span>
              <span className="block text-5xl sm:text-7xl lg:text-8xl font-black italic tracking-tighter text-transparent bg-clip-text bg-gradient-to-b from-white via-white to-white/40 leading-none pb-3 drop-shadow-[0_15px_40px_rgba(255,255,255,0.1)]">
                {t('heroTitle2')}
              </span>
              <div className="absolute -bottom-1 left-1/2 -translate-x-1/2 w-1/4 h-[1px] bg-gradient-to-r from-transparent via-purple-500/50 to-transparent"></div>
            </h1>

            {/* Mô tả - Gọn gàng hơn */}
            <p className="max-w-xl text-base sm:text-lg text-zinc-400 font-medium mb-12 leading-relaxed tracking-wide animate-in fade-in slide-in-from-bottom-6 duration-700 delay-300">
              {t('heroDesc')}
            </p>

            {/* CTAs - Nhỏ gọn hơn */}
            <div className="flex flex-col sm:flex-row items-center gap-4 w-full sm:w-auto animate-in fade-in slide-in-from-bottom-10 duration-700 delay-500">
              <Link
                href="/reading"
                className="group relative flex items-center justify-center px-8 py-3.5 bg-white text-black font-black text-xs uppercase tracking-widest rounded-full hover:bg-zinc-200 transition-all active:scale-95 shadow-[0_0_30px_rgba(255,255,255,0.15)] hover:shadow-[0_0_50px_rgba(255,255,255,0.3)]"
              >
                {t('ctaDraw')}
                <Compass className="w-4 h-4 ml-2.5 transition-transform duration-500 group-hover:rotate-180" />
              </Link>

              <Link
                href="/collection"
                className="group flex items-center justify-center px-8 py-3.5 bg-transparent text-white border border-white/20 font-bold text-xs uppercase tracking-widest rounded-full hover:bg-white/5 transition-all backdrop-blur-md"
              >
                {t('ctaCollection')}
                <ChevronRight className="w-4 h-4 ml-1.5 transition-transform duration-300 group-hover:translate-x-1" />
              </Link>
            </div>
          </div>

          <div className="absolute bottom-8 left-1/2 -translate-x-1/2 animate-bounce opacity-30">
             <div className="w-[1px] h-10 bg-gradient-to-b from-white to-transparent"></div>
          </div>
        </section>

        {/* ===== FEATURES: THU NHỎ SECTION ===== */}
        <section className="relative w-full max-w-6xl mx-auto px-6 py-20 md:py-28">
          <div className="flex flex-col md:flex-row gap-12 items-end mb-16">
            <div className="flex-1 space-y-4">
              <span className="text-[10px] font-black uppercase tracking-[0.4em] text-purple-400/80">{t('tagline')}</span>
              <h2 className="text-3xl md:text-5xl font-black text-white leading-tight tracking-tight">
                {t('featureTitle')}
              </h2>
            </div>
            <div className="flex-1 md:max-w-sm">
              <p className="text-base text-zinc-500 font-medium leading-relaxed">
                {t('featureDesc')}
              </p>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-1px bg-white/5 border border-white/10 rounded-[2.5rem] overflow-hidden shadow-2xl backdrop-blur-sm">
             {/* Feature 1 */}
             <div className="group relative p-10 md:p-12 flex flex-col gap-8 hover:bg-purple-500/5 transition-all duration-700 cursor-default">
                <div className="w-12 h-12 rounded-full bg-white/5 flex items-center justify-center border border-white/10 transition-transform duration-700 group-hover:scale-110 group-hover:bg-purple-500 group-hover:text-black">
                  <Star className="w-5 h-5" />
                </div>
                <div className="space-y-3">
                  <h3 className="text-xl font-bold text-white transition-colors duration-500 group-hover:text-purple-300">{t('f1Title')}</h3>
                  <p className="text-zinc-500 text-sm leading-relaxed font-medium transition-colors duration-500 group-hover:text-zinc-300">
                    {t('f1Desc')}
                  </p>
                </div>
                <div className="absolute top-0 right-0 w-24 h-24 bg-purple-500/10 blur-[50px] rounded-full opacity-0 group-hover:opacity-100 transition-opacity duration-700"></div>
             </div>

             {/* Feature 2 */}
             <div className="group relative p-10 md:p-12 flex flex-col gap-8 hover:bg-amber-500/5 border-l border-white/10 transition-all duration-700 cursor-default">
                <div className="w-12 h-12 rounded-full bg-white/5 flex items-center justify-center border border-white/10 transition-transform duration-700 group-hover:scale-110 group-hover:bg-amber-500 group-hover:text-black">
                   <Flame className="w-5 h-5" />
                </div>
                <div className="space-y-3">
                  <h3 className="text-xl font-bold text-white transition-colors duration-500 group-hover:text-amber-300">{t('f2Title')}</h3>
                  <p className="text-zinc-500 text-sm leading-relaxed font-medium transition-colors duration-500 group-hover:text-zinc-300">
                    {t('f2Desc')}
                  </p>
                </div>
                <div className="absolute top-0 right-0 w-24 h-24 bg-amber-500/10 blur-[50px] rounded-full opacity-0 group-hover:opacity-100 transition-opacity duration-700"></div>
             </div>

             {/* Feature 3 */}
             <div className="group relative p-10 md:p-12 flex flex-col gap-8 hover:bg-cyan-500/5 border-l border-white/10 transition-all duration-700 cursor-default">
                <div className="w-12 h-12 rounded-full bg-white/5 flex items-center justify-center border border-white/10 transition-transform duration-700 group-hover:scale-110 group-hover:bg-cyan-500 group-hover:text-black">
                   <ShieldCheck className="w-5 h-5" />
                </div>
                <div className="space-y-3">
                  <h3 className="text-xl font-bold text-white transition-colors duration-500 group-hover:text-cyan-300">{t('f3Title')}</h3>
                  <p className="text-zinc-500 text-sm leading-relaxed font-medium transition-colors duration-500 group-hover:text-zinc-300">
                    {t('f3Desc')}
                  </p>
                </div>
                <div className="absolute top-0 right-0 w-24 h-24 bg-cyan-500/10 blur-[50px] rounded-full opacity-0 group-hover:opacity-100 transition-opacity duration-700"></div>
             </div>
          </div>
        </section>

        {/* ===== FINAL CTA SECTION: THU NHỎ ===== */}
        <section className="relative px-6 py-24 md:py-32 max-w-5xl mx-auto flex flex-col items-center">
          <div className="relative group p-0.5 rounded-[3rem] bg-gradient-to-br from-white/10 via-zinc-800 to-white/5 w-full flex flex-col items-center overflow-hidden">
             <div className="absolute inset-0 bg-purple-500/5 blur-[80px] rounded-full opacity-50"></div>
             <div className="relative w-full py-16 md:py-20 rounded-[2.8rem] bg-zinc-950/90 flex flex-col items-center text-center px-8">
                <Moon className="w-10 h-10 text-zinc-500 mb-6 animate-pulse" />
                <h2 className="text-3xl md:text-5xl font-black text-white mb-6 max-w-2xl tracking-tighter">
                   Bắt đầu hành trình <br/> <span className="text-zinc-500 italic">Thấu hiểu bản thân</span>
                </h2>
                <Link
                  href="/reading"
                  className="group flex items-center gap-3 bg-white text-black px-10 py-4 rounded-full font-black text-xs uppercase tracking-widest hover:scale-105 active:scale-95 transition-all shadow-[0_15px_30px_rgba(255,255,255,0.1)]"
                >
                  Trải bài ngay
                  <Zap className="w-4 h-4 fill-black" />
                </Link>
             </div>
          </div>
        </section>
      </main>

      {/* FOOTER ĐƠN GIẢN SANG TRỌNG */}
      <footer className="relative z-10 py-16 border-t border-white/5 flex flex-col items-center gap-8">
        <span className="text-lg font-black bg-gradient-to-r from-zinc-100 to-zinc-500 text-transparent bg-clip-text">TarotNow AI</span>
        <div className="flex gap-6 text-[10px] font-bold text-zinc-500 uppercase tracking-[0.25em]">
          <Link href="/reading" className="hover:text-white transition-colors">Dịch vụ</Link>
          <Link href="/wallet" className="hover:text-white transition-colors">Tín dụng</Link>
          <Link href="/profile" className="hover:text-white transition-colors">Hỗ trợ</Link>
        </div>
        <p className="text-[9px] text-zinc-700 font-medium tracking-widest uppercase">© 2026 TarotNow AI • Premium Spiritual Experience</p>
      </footer>

      {/* CUSTOM ANIMATIONS */}
      <style dangerouslySetInnerHTML={{
        __html: `
        @keyframes float {
          0% { transform: translateY(0) translateX(0) scale(1); opacity: 0; }
          20% { opacity: 0.3; }
          80% { opacity: 0.3; }
          100% { transform: translateY(-100vh) translateX(50px) scale(0.5); opacity: 0; }
        }
        .animate-float {
          animation-name: float;
          animation-timing-function: linear;
          animation-iteration-count: infinite;
        }
      `}} />
    </div>
  );
}
