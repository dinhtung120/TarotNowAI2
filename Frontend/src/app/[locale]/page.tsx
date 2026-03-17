import { Link } from "@/i18n/routing";
import { 
  Star, ChevronRight, ShieldCheck, Flame, Compass, Sparkle, 
  Moon, Ghost, Zap, Users, MessageSquare, Globe, Heart, 
  ArrowUpRight, Award, LucideIcon, Gem
} from "lucide-react";
import { getTranslations } from "next-intl/server";
import { listReaders, type ReaderProfile } from "@/actions/readerActions";

/**
 * Trang Chủ Redesign - Phong cách Ultra Premium Astral
 * 
 * Các nâng cấp chính:
 * 1. Typography: Kết hợp serif (Playfair Display) cho tiêu đề và sans-serif cho nội dung.
 * 2. Reader Showcase: Tự động lấy danh sách Reader từ backend.
 * 3. Universe Stats: Hiển thị các chỉ số tăng trưởng của hệ thống.
 * 4. Micro-animations: Hiệu ứng hover và background nebula mượt mà hơn.
 */

interface StatProps {
  icon: LucideIcon;
  value: string;
  label: string;
  color: string;
}

function StatItem({ icon: Icon, value, label, color }: StatProps) {
  return (
    <div className="flex flex-col items-center p-6 rounded-3xl bg-white/[0.02] border border-white/5 backdrop-blur-md">
      <div className={`w-10 h-10 rounded-2xl bg-${color}-500/10 flex items-center justify-center mb-4 border border-${color}-500/20`}>
        <Icon className={`w-5 h-5 text-${color}-400`} />
      </div>
      <div className="text-3xl font-black text-white tracking-tighter italic mb-1">{value}</div>
      <div className="text-[9px] font-black uppercase tracking-[0.2em] text-zinc-500">{label}</div>
    </div>
  );
}

export default async function Home() {
  const t = await getTranslations("Index");
  
  // Lấy danh sách 4 reader nổi bật cho phần Showcase
  const readerData = await listReaders(1, 4);
  const featuredReaders = readerData?.readers || [];

  return (
    <div className="min-h-screen bg-[#020108] text-zinc-100 selection:bg-purple-500/40 overflow-x-hidden font-sans">
      
      {/* ##### HỆ THỐNG NỀN THIÊN HÀ (NEBULA SYSTEM) ##### */}
      <div className="fixed inset-0 z-0 pointer-events-none">
        <div className="absolute inset-0 bg-[url('https://grainy-gradients.vercel.app/noise.svg')] opacity-[0.03] mix-blend-overlay"></div>
        
        {/* Animated Nebulas */}
        <div className="absolute top-[-20%] -left-1/4 w-[80vw] h-[80vw] bg-purple-900/[0.12] blur-[150px] rounded-full animate-slow-drift" />
        <div className="absolute top-1/4 -right-1/4 w-[70vw] h-[70vw] bg-indigo-900/[0.1] blur-[140px] rounded-full animate-slow-drift-reverse" />
        <div className="absolute -bottom-1/4 left-1/3 w-[60vw] h-[60vw] bg-fuchsia-900/[0.08] blur-[130px] rounded-full animate-pulse-slow" />
        
        {/* Grid Overlay */}
        <div className="absolute inset-0 opacity-[0.03] bg-[linear-gradient(to_right,#ffffff10_1px,transparent_1px),linear-gradient(to_bottom,#ffffff10_1px,transparent_1px)] bg-[size:60px_60px]"></div>
      </div>

      <main className="relative z-10">
        
        {/* ##### HERO SECTION: THE MYSTIC PORTAL ##### */}
        <section className="relative min-h-screen flex flex-col items-center justify-center px-6 pt-20">
          <div className="w-full max-w-6xl flex flex-col items-center text-center">
            
            {/* Live Tagline */}
            <div className="inline-flex items-center gap-3 px-4 py-2 rounded-full bg-white/[0.03] border border-white/10 text-[10px] uppercase tracking-[0.3em] font-black text-amber-200/90 mb-10 shadow-2xl backdrop-blur-2xl animate-in fade-in slide-in-from-bottom-4 duration-1000">
              <span className="relative flex h-2 w-2">
                <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-amber-400 opacity-75"></span>
                <span className="relative inline-flex rounded-full h-2 w-2 bg-amber-500"></span>
              </span>
              {t('tagline')}
            </div>

            {/* Main Title - Using Playfair Display */}
            <h1 className="relative mb-10 animate-in fade-in zoom-in-95 duration-1000 delay-200">
              <span className="block font-playfair italic text-4xl sm:text-6xl lg:text-7xl font-light tracking-tight text-white/90 mb-2 leading-tight">
                {t('heroTitle1')}
              </span>
              <span className="block text-6xl sm:text-8xl lg:text-9xl font-black italic tracking-tighter text-transparent bg-clip-text bg-gradient-to-b from-white via-white/80 to-white/20 leading-none pb-4 drop-shadow-[0_20px_50px_rgba(255,255,255,0.15)]">
                {t('heroTitle2')}
              </span>
              <div className="absolute -bottom-2 left-1/2 -translate-x-1/2 w-40 h-[1px] bg-gradient-to-r from-transparent via-purple-500/50 to-transparent"></div>
            </h1>

            {/* Description */}
            <p className="max-w-2xl text-lg sm:text-xl text-zinc-400 font-medium mb-14 leading-relaxed tracking-wide animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-500">
              {t('heroDesc')}
            </p>

            {/* Hero Actions */}
            <div className="flex flex-col sm:flex-row items-center gap-6 w-full sm:w-auto animate-in fade-in slide-in-from-bottom-12 duration-1000 delay-700">
              <Link
                href="/reading"
                className="group relative flex items-center justify-center px-10 py-5 bg-white text-black font-black text-xs uppercase tracking-[0.2em] rounded-2xl hover:bg-zinc-100 transition-all shadow-[0_20px_40px_rgba(255,255,255,0.1)] active:scale-95"
              >
                {t('ctaDraw')}
                <Compass className="w-5 h-5 ml-3 transition-transform duration-700 group-hover:rotate-180" />
              </Link>

              <Link
                href="/readers"
                className="group flex items-center justify-center px-10 py-5 bg-white/[0.03] text-white border border-white/10 font-black text-xs uppercase tracking-[0.2em] rounded-2xl hover:bg-white/5 transition-all backdrop-blur-xl"
              >
                Gặp Gỡ Readers
                <Users className="w-5 h-5 ml-3 transition-transform duration-300 group-hover:translate-x-1" />
              </Link>
            </div>
          </div>

          {/* Scroll Indicator */}
          <div className="absolute bottom-12 left-1/2 -translate-x-1/2 flex flex-col items-center gap-4 opacity-20">
             <div className="text-[9px] font-black uppercase tracking-[0.4em] rotate-90 origin-left translate-x-1.5 translate-y-2">Scroll</div>
             <div className="w-[1px] h-16 bg-gradient-to-b from-white to-transparent"></div>
          </div>
        </section>

        {/* ##### STATS SECTION: UNIVERSE GROWTH ##### */}
        <section className="relative w-full max-w-6xl mx-auto px-6 py-20">
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4 md:gap-8">
            <StatItem icon={Sparkle} value="50K+" label="Lượt Trải Bài" color="purple" />
            <StatItem icon={Users} value="120+" label="Reader Chuyên Nghiệp" color="amber" />
            <StatItem icon={Award} value="4.9" label="Rating Trung Bình" color="emerald" />
            <StatItem icon={Globe} value="24/7" label="Hỗ Trợ Toàn Cầu" color="cyan" />
          </div>
        </section>

        {/* ##### READER SHOWCASE: THE GUIDES ##### */}
        <section className="relative w-full max-w-7xl mx-auto px-6 py-32">
          <div className="flex flex-col md:flex-row items-end justify-between mb-20 gap-8">
            <div className="space-y-4 max-w-xl">
              <div className="text-[10px] font-black uppercase tracking-[0.4em] text-purple-400">Kết nối bậc thầy</div>
              <h2 className="text-4xl md:text-6xl font-black text-white leading-tight tracking-tighter uppercase italic">
                Bậc Thầy <span className="text-zinc-600">Dẫn Lối</span>
              </h2>
            </div>
            <Link href="/readers" className="text-xs font-black uppercase tracking-widest text-zinc-500 hover:text-white transition-colors flex items-center gap-2 group">
              Xem tất cả Readers <ArrowUpRight className="w-4 h-4 transition-transform group-hover:translate-x-1 group-hover:-translate-y-1" />
            </Link>
          </div>

          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
            {featuredReaders.map((reader: ReaderProfile) => (
              <Link 
                key={reader.userId} 
                href={`/readers/${reader.userId}` as any}
                className="group relative h-96 rounded-[2.5rem] overflow-hidden border border-white/5 bg-zinc-900/40 hover:border-purple-500/30 transition-all duration-700 hover:-translate-y-4 shadow-2xl"
              >
                {/* Background Glow */}
                <div className="absolute inset-0 bg-gradient-to-t from-black via-transparent to-transparent z-10" />
                <div className="absolute inset-0 bg-purple-500/5 opacity-0 group-hover:opacity-100 transition-opacity duration-700" />
                
                {/* Reader Meta */}
                <div className="absolute inset-x-0 bottom-0 p-8 z-20 space-y-4">
                  <div className="flex items-center justify-between">
                    <div className={`w-3 h-3 rounded-full ${reader.status === 'accepting_questions' ? 'bg-emerald-400 animate-pulse' : 'bg-zinc-600'}`} />
                    <div className="flex items-center gap-1.5 px-3 py-1 rounded-full bg-white/10 backdrop-blur-md border border-white/10">
                      <Star className="w-3 h-3 text-amber-400 fill-amber-400" />
                      <span className="text-[10px] font-black text-white">{reader.avgRating.toFixed(1)}</span>
                    </div>
                  </div>
                  
                  <div>
                    <h3 className="text-2xl font-black text-white tracking-tighter uppercase italic truncate">{reader.displayName}</h3>
                    <div className="text-[9px] font-black uppercase tracking-[0.2em] text-zinc-500 mt-1 line-clamp-1">
                      {reader.specialties.join(' • ')}
                    </div>
                  </div>

                  <div className="pt-4 border-t border-white/5 flex items-center justify-between">
                    <div className="flex items-center gap-1.5">
                      <Gem className="w-3.5 h-3.5 text-amber-500" />
                      <span className="text-xs font-black text-white">{reader.diamondPerQuestion} 💎</span>
                    </div>
                    <div className="text-[10px] font-black uppercase tracking-widest text-purple-400 group-hover:translate-x-2 transition-transform">Hồ sơ →</div>
                  </div>
                </div>

                {/* Initial Placeholder or Background Decoration */}
                <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 opacity-[0.05] pointer-events-none group-hover:scale-125 transition-transform duration-1000">
                  <Moon size={240} className="text-white" />
                </div>
              </Link>
            ))}
          </div>
        </section>

        {/* ##### CORE FEATURES: THE ASTRAL ARCHITECTURE ##### */}
        <section className="relative w-full max-w-6xl mx-auto px-6 py-32">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-1px bg-white/5 border border-white/10 rounded-[3rem] overflow-hidden shadow-2xl backdrop-blur-md">
             {/* Feature 1: AI Insight */}
             <div className="group relative p-12 flex flex-col gap-10 hover:bg-purple-500/5 transition-all duration-700">
                <div className="w-14 h-14 rounded-2xl bg-white/5 flex items-center justify-center border border-white/10 transition-all duration-700 group-hover:scale-110 group-hover:bg-purple-500 group-hover:shadow-[0_0_30px_rgba(168,85,247,0.4)]">
                  <Zap className="w-6 h-6 group-hover:text-black transition-colors" />
                </div>
                <div className="space-y-4">
                  <h3 className="text-2xl font-black text-white italic tracking-tighter uppercase">{t('f1Title')}</h3>
                  <p className="text-zinc-500 text-sm leading-relaxed font-semibold transition-colors duration-500 group-hover:text-zinc-300">
                    {t('f1Desc')}
                  </p>
                </div>
                <div className="absolute top-0 right-0 w-32 h-32 bg-purple-500/10 blur-[60px] rounded-full opacity-0 group-hover:opacity-100 transition-opacity duration-700"></div>
             </div>

             {/* Feature 2: Personalized Path */}
             <div className="group relative p-12 flex flex-col gap-10 hover:bg-amber-500/5 border-x border-white/5 transition-all duration-700">
                <div className="w-14 h-14 rounded-2xl bg-white/5 flex items-center justify-center border border-white/10 transition-all duration-700 group-hover:scale-110 group-hover:bg-amber-500 group-hover:shadow-[0_0_30px_rgba(245,158,11,0.4)]">
                   <Flame className="w-6 h-6 group-hover:text-black transition-colors" />
                </div>
                <div className="space-y-4">
                  <h3 className="text-2xl font-black text-white italic tracking-tighter uppercase">{t('f2Title')}</h3>
                  <p className="text-zinc-500 text-sm leading-relaxed font-semibold transition-colors duration-500 group-hover:text-zinc-300">
                    {t('f2Desc')}
                  </p>
                </div>
                <div className="absolute top-0 right-0 w-32 h-32 bg-amber-500/10 blur-[60px] rounded-full opacity-0 group-hover:opacity-100 transition-opacity duration-700"></div>
             </div>

             {/* Feature 3: Transparent Protocol */}
             <div className="group relative p-12 flex flex-col gap-10 hover:bg-emerald-500/5 transition-all duration-700">
                <div className="w-14 h-14 rounded-2xl bg-white/5 flex items-center justify-center border border-white/10 transition-all duration-700 group-hover:scale-110 group-hover:bg-emerald-500 group-hover:shadow-[0_0_30px_rgba(16,185,129,0.4)]">
                   <ShieldCheck className="w-6 h-6 group-hover:text-black transition-colors" />
                </div>
                <div className="space-y-4">
                  <h3 className="text-2xl font-black text-white italic tracking-tighter uppercase">{t('f3Title')}</h3>
                  <p className="text-zinc-500 text-sm leading-relaxed font-semibold transition-colors duration-500 group-hover:text-zinc-300">
                    {t('f3Desc')}
                  </p>
                </div>
                <div className="absolute top-0 right-0 w-32 h-32 bg-emerald-500/10 blur-[60px] rounded-full opacity-0 group-hover:opacity-100 transition-opacity duration-700"></div>
             </div>
          </div>
        </section>

        {/* ##### FINAL CALL: RESONANCE ##### */}
        <section className="relative px-6 py-40 flex flex-col items-center overflow-hidden">
          <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[150%] aspect-square bg-purple-500/[0.03] blur-[150px] rounded-full" />
          
          <div className="relative z-10 flex flex-col items-center text-center max-w-4xl">
            <Moon className="w-16 h-16 text-zinc-800 mb-10 animate-pulse" />
            <h2 className="text-5xl md:text-8xl font-black text-white mb-12 tracking-tighter uppercase">
               Bắt đầu hành trình <br/> <span className="text-zinc-600 italic">Ánh Sáng & Sự Thật</span>
            </h2>
            
            <Link
              href="/reading"
              className="group relative flex items-center gap-4 bg-white text-black px-12 py-6 rounded-3xl font-black text-sm uppercase tracking-[0.3em] hover:scale-105 active:scale-95 transition-all shadow-[0_30px_60px_rgba(255,255,255,0.1)] overflow-hidden"
            >
              <div className="absolute inset-x-0 bottom-0 h-1 bg-gradient-to-r from-purple-500 via-amber-500 to-indigo-500 transform translate-y-1 group-hover:translate-y-0 transition-transform duration-500" />
              Trải bài ngay
              <ChevronRight className="w-5 h-5 group-hover:translate-x-2 transition-transform" />
            </Link>
          </div>
        </section>
      </main>

      {/* FOOTER: THE END OF KNOWLEDGE */}
      <footer className="relative z-10 py-24 border-t border-white/5 flex flex-col items-center gap-12 bg-black">
        <div className="flex flex-col items-center gap-4">
          <span className="text-3xl font-black italic tracking-tighter bg-gradient-to-b from-white to-zinc-600 text-transparent bg-clip-text">TarotNow AI</span>
          <p className="text-[10px] text-zinc-500 font-black uppercase tracking-[0.5em]">The Spiritual Algorithm</p>
        </div>

        <div className="flex flex-wrap justify-center gap-10 text-[10px] font-black text-zinc-500 uppercase tracking-[0.3em]">
          <Link href="/reading" className="hover:text-white transition-colors">Dịch vụ</Link>
          <Link href="/readers" className="hover:text-white transition-colors">Readers</Link>
          <Link href="/wallet" className="hover:text-white transition-colors">Tín dụng</Link>
          <Link href="/profile" className="hover:text-white transition-colors">Hỗ trợ</Link>
        </div>

        <div className="flex flex-col items-center gap-4 pt-8">
          <div className="flex gap-4">
            <div className="w-8 h-8 rounded-full border border-white/10 hover:border-white/30 transition-all flex items-center justify-center cursor-pointer"><Globe className="w-4 h-4" /></div>
            <div className="w-8 h-8 rounded-full border border-white/10 hover:border-white/30 transition-all flex items-center justify-center cursor-pointer"><Heart className="w-4 h-4" /></div>
            <div className="w-8 h-8 rounded-full border border-white/10 hover:border-white/30 transition-all flex items-center justify-center cursor-pointer"><MessageSquare className="w-4 h-4" /></div>
          </div>
          <p className="text-[8px] text-zinc-800 font-black tracking-[0.2em] uppercase">© 2026 TarotNow AI • Premium Spiritual Experience • All Rights Reserved</p>
        </div>
      </footer>

      {/* ##### CUSTOM STYLES & ANIMATIONS ##### */}
      <style dangerouslySetInnerHTML={{
        __html: `
        @keyframes float {
          0% { transform: translateY(10vh) translateX(0) scale(1); opacity: 0; }
          20% { opacity: 0.3; }
          80% { opacity: 0.3; }
          100% { transform: translateY(-100vh) translateX(50px) scale(0.5); opacity: 0; }
        }
        .animate-float {
          animation: float 15s linear infinite;
        }
        @keyframes drift {
          0%, 100% { transform: translate(0, 0) scale(1); }
          50% { transform: translate(10vw, 5vh) scale(1.1); }
        }
        .animate-slow-drift { animation: drift 25s ease-in-out infinite; }
        .animate-slow-drift-reverse { animation: drift 30s ease-in-out infinite reverse; }
        @keyframes pulse-slow {
          0%, 100% { opacity: 0.05; transform: scale(1); }
          50% { opacity: 0.1; transform: scale(1.15); }
        }
        .animate-pulse-slow { animation: pulse-slow 20s ease-in-out infinite; }
      `}} />
    </div>
  );
}
