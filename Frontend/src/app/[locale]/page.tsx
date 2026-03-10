import { Link } from "@/i18n/routing";
import { Star, ChevronRight, ShieldCheck, Flame, Compass, Sparkle } from "lucide-react";
import { getTranslations } from "next-intl/server";

export default async function Home() {
  const t = await getTranslations("Index");

  return (
    <div className="min-h-screen bg-[#030014] text-white selection:bg-fuchsia-500/30 overflow-hidden font-sans">
      {/* Cấu trúc Nền (Cosmic Background Effects) */}
      <div className="fixed inset-0 z-0 pointer-events-none">
        {/* Lưới tinh thể mờ ảo */}
        <div className="absolute inset-0 bg-[url('https://grainy-gradients.vercel.app/noise.svg')] opacity-[0.03] mix-blend-overlay"></div>

        {/* Các vệt sáng Nebula xoay chậm */}
        <div className="absolute top-[-20%] left-[-10%] w-[50%] h-[50%] bg-purple-600/20 blur-[150px] rounded-[100%] animate-pulse" style={{ animationDuration: '8s' }} />
        <div className="absolute top-[30%] right-[-20%] w-[60%] h-[60%] bg-indigo-900/40 blur-[160px] rounded-[100%]" />
        <div className="absolute bottom-[-20%] left-[10%] w-[40%] h-[50%] bg-fuchsia-900/20 blur-[140px] rounded-[100%] animate-pulse" style={{ animationDuration: '10s' }} />
        <div className="absolute bottom-[10%] right-[30%] w-[30%] h-[30%] bg-amber-600/10 blur-[120px] rounded-[100%]" />
      </div>

      <main className="relative z-10 flex flex-col items-center">
        {/* Phần Hero Tối thượng */}
        <section className="relative w-full max-w-7xl px-6 pt-32 pb-24 md:pt-56 md:pb-40 flex flex-col items-center text-center">

          {/* Tagline Aura */}
          <div className="group relative inline-flex items-center gap-2 px-5 py-2.5 rounded-full bg-white/5 border border-white/10 text-sm font-semibold text-amber-200 mb-10 shadow-[0_0_20px_rgba(251,191,36,0.15)] backdrop-blur-xl transition-all duration-300 hover:scale-105 hover:bg-white/10 cursor-default">
            <span className="absolute inset-0 rounded-full bg-gradient-to-r from-amber-500/20 to-fuchsia-500/20 blur-md opacity-0 group-hover:opacity-100 transition-opacity duration-500"></span>
            <Sparkle className="w-4 h-4 text-amber-400 animate-pulse" />
            <span className="relative z-10 tracking-wide uppercase text-[11px] sm:text-xs">{t('tagline')}</span>
          </div>

          {/* Tiêu đề chính */}
          <h1 className="text-5xl sm:text-7xl lg:text-8xl font-black tracking-tighter mb-8 leading-[1.05] relative w-full">
            <span className="block text-transparent bg-clip-text bg-gradient-to-br from-zinc-100 to-zinc-500 drop-shadow-sm pb-2">
              {t('heroTitle1')}
            </span>
            <div className="relative inline-block mt-2 sm:mt-4">
              {/* Hiệu ứng chớp sáng phía sau chữ */}
              <span className="absolute -inset-2 blur-2xl bg-gradient-to-r from-purple-500 via-fuchsia-500 to-amber-500 opacity-40 z-0"></span>
              <span className="relative z-10 text-transparent bg-clip-text bg-gradient-to-r from-purple-300 via-fuchsia-200 to-amber-200 drop-shadow-[0_0_15px_rgba(232,121,249,0.3)]">
                {t('heroTitle2')}
              </span>
            </div>
          </h1>

          {/* Đoạn mô tả mượt mà */}
          <p className="max-w-2xl mx-auto text-lg sm:text-xl text-zinc-400/90 font-medium mb-14 leading-relaxed tracking-wide">
            {t('heroDesc')}
          </p>

          {/* Cụm Nút Action Cao cấp */}
          <div className="flex flex-col sm:flex-row items-center justify-center gap-6 w-full sm:w-auto">
            <Link
              href="/reading"
              className="group relative inline-flex items-center justify-center px-10 py-5 font-bold text-white transition-all duration-300 bg-gradient-to-r from-purple-600 to-fuchsia-600 rounded-2xl hover:scale-105 active:scale-95 shadow-[0_0_40px_rgba(192,38,211,0.3)] hover:shadow-[0_0_60px_rgba(192,38,211,0.5)] w-full sm:w-auto border border-white/10 overflow-hidden"
            >
              {/* Lớp phản quang chạy ngang */}
              <div className="absolute inset-0 -translate-x-full group-hover:animate-[shimmer_1.5s_infinite] bg-gradient-to-r from-transparent via-white/20 to-transparent skew-x-12"></div>
              <span className="relative z-10 text-lg tracking-wide">{t('ctaDraw')}</span>
              <Compass className="relative z-10 w-5 h-5 ml-3 transition-transform duration-300 group-hover:rotate-45" />
            </Link>

            <Link
              href="/collection"
              className="group inline-flex items-center justify-center px-10 py-5 font-bold text-zinc-300 transition-all duration-300 bg-white/5 border border-white/10 rounded-2xl hover:bg-white/10 hover:text-white backdrop-blur-md w-full sm:w-auto hover:shadow-[0_0_30px_rgba(255,255,255,0.05)]"
            >
              <span className="tracking-wide text-lg">{t('ctaCollection')}</span>
              <ChevronRight className="w-5 h-5 ml-2 transition-transform duration-300 group-hover:translate-x-1.5 opacity-70 group-hover:opacity-100" />
            </Link>
          </div>

        </section>

        {/* Dải ngăn cách phát sáng mờ */}
        <div className="w-full max-w-5xl h-px bg-gradient-to-r from-transparent via-purple-500/20 to-transparent my-10" />

        {/* Phần Features (3 Trụ cột Tarot) */}
        <section className="relative w-full max-w-7xl px-6 py-24 md:py-32">

          <div className="text-center mb-20 space-y-4">
            <h2 className="text-4xl md:text-5xl font-black text-transparent bg-clip-text bg-gradient-to-r from-zinc-200 to-zinc-500 tracking-tight">{t('featureTitle')}</h2>
            <p className="text-lg text-zinc-500 max-w-2xl mx-auto font-medium">{t('featureDesc')}</p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 relative z-10">
            {/* Feature 1 - Insight */}
            <div className="relative p-[1px] rounded-3xl bg-gradient-to-b from-white/10 to-transparent group hover:from-purple-500/50 transition-colors duration-500">
              <div className="absolute inset-0 bg-purple-500/10 blur-3xl opacity-0 group-hover:opacity-100 transition-opacity duration-500 rounded-3xl" />
              <div className="relative h-full p-8 md:p-10 rounded-[23px] bg-zinc-950/80 backdrop-blur-xl border border-white/[0.05] overflow-hidden flex flex-col items-start gap-6">
                <div className="w-16 h-16 rounded-2xl bg-gradient-to-br from-purple-500/20 to-fuchsia-500/5 text-purple-300 flex items-center justify-center shadow-[inset_0_0_20px_rgba(168,85,247,0.1)] ring-1 ring-purple-500/30 group-hover:scale-110 group-hover:rotate-6 transition-transform duration-500">
                  <Star className="w-8 h-8" strokeWidth={1.5} />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-zinc-100 mb-4 tracking-tight group-hover:text-purple-300 transition-colors">{t('f1Title')}</h3>
                  <p className="text-zinc-400 leading-relaxed text-[15px] group-hover:text-zinc-300 transition-colors">
                    {t('f1Desc')}
                  </p>
                </div>
              </div>
            </div>

            {/* Feature 2 - Energy */}
            <div className="relative p-[1px] rounded-3xl bg-gradient-to-b from-white/10 to-transparent group hover:from-amber-500/50 transition-colors duration-500 md:-translate-y-8">
              <div className="absolute inset-0 bg-amber-500/10 blur-3xl opacity-0 group-hover:opacity-100 transition-opacity duration-500 rounded-3xl" />
              <div className="relative h-full p-8 md:p-10 rounded-[23px] bg-zinc-950/80 backdrop-blur-xl border border-white/[0.05] overflow-hidden flex flex-col items-start gap-6">
                <div className="w-16 h-16 rounded-2xl bg-gradient-to-br from-amber-500/20 to-orange-500/5 text-amber-300 flex items-center justify-center shadow-[inset_0_0_20px_rgba(245,158,11,0.1)] ring-1 ring-amber-500/30 group-hover:scale-110 group-hover:-rotate-6 transition-transform duration-500">
                  <Flame className="w-8 h-8" strokeWidth={1.5} />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-zinc-100 mb-4 tracking-tight group-hover:text-amber-300 transition-colors">{t('f2Title')}</h3>
                  <p className="text-zinc-400 leading-relaxed text-[15px] group-hover:text-zinc-300 transition-colors">
                    {t('f2Desc')}
                  </p>
                </div>
              </div>
            </div>

            {/* Feature 3 - Protection */}
            <div className="relative p-[1px] rounded-3xl bg-gradient-to-b from-white/10 to-transparent group hover:from-cyan-500/50 transition-colors duration-500">
              <div className="absolute inset-0 bg-cyan-500/10 blur-3xl opacity-0 group-hover:opacity-100 transition-opacity duration-500 rounded-3xl" />
              <div className="relative h-full p-8 md:p-10 rounded-[23px] bg-zinc-950/80 backdrop-blur-xl border border-white/[0.05] overflow-hidden flex flex-col items-start gap-6">
                <div className="w-16 h-16 rounded-2xl bg-gradient-to-br from-cyan-500/20 to-blue-500/5 text-cyan-300 flex items-center justify-center shadow-[inset_0_0_20px_rgba(6,182,212,0.1)] ring-1 ring-cyan-500/30 group-hover:scale-110 group-hover:rotate-6 transition-transform duration-500">
                  <ShieldCheck className="w-8 h-8" strokeWidth={1.5} />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-zinc-100 mb-4 tracking-tight group-hover:text-cyan-300 transition-colors">{t('f3Title')}</h3>
                  <p className="text-zinc-400 leading-relaxed text-[15px] group-hover:text-zinc-300 transition-colors">
                    {t('f3Desc')}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </section>

      </main>

      {/* Khung chìm Animation Shimmer Effect */}
      <style dangerouslySetInnerHTML={{
        __html: `
        @keyframes shimmer {
          100% {
            transform: translateX(100%);
          }
        }
      `}} />
    </div>
  );
}
