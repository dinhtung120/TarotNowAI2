"use client";

import { useState } from "react";

import { useRouter } from "@/i18n/routing";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { initReadingSession } from "@/actions/readingActions";
import { Loader2, Sparkles, AlertCircle, Compass, Zap, Star, Flame, ShieldCheck, Moon } from "lucide-react";
import { useWalletStore } from "@/store/walletStore";
import { useTranslations } from "next-intl";

/**
 * Zod Schema validated form (Optional message)
 */
const formSchema = z.object({
    question: z.string().max(300, "Question is too long").optional(),
});

type FormData = z.infer<typeof formSchema>;

export default function ReadingSetupPage() {
    const router = useRouter();
    const t = useTranslations("ReadingSetup");
    const { fetchBalance } = useWalletStore(); // Reload wallet after init
    const [selectedSpread, setSelectedSpread] = useState<string>("daily_1");
    const [initError, setInitError] = useState("");
    const [isInitializing, setIsInitializing] = useState(false);

    // Form setup
    const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
        resolver: zodResolver(formSchema),
    });

    const SPREADS = [
        { id: "daily_1", name: t("daily_1_name"), desc: t("daily_1_desc"), cost: "Free", icon: <Star className="w-5 h-5" /> },
        { id: "spread_3", name: t("spread_3_name"), desc: t("spread_3_desc"), cost: "50 Gold", icon: <Flame className="w-5 h-5" /> },
        { id: "spread_5", name: t("spread_5_name"), desc: t("spread_5_desc"), cost: "100 Gold", icon: <ShieldCheck className="w-5 h-5" /> },
        { id: "spread_10", name: t("spread_10_name"), desc: t("spread_10_desc"), cost: "50 Diamond", icon: <Moon className="w-5 h-5" /> },
    ];

    const onSubmit = async (data: FormData) => {
        setIsInitializing(true);
        setInitError("");

        const response = await initReadingSession({
            spreadType: selectedSpread
        });

        if (response.success && response.data) {
            // Refresh wallet balance do đã bị trừ tiền
            await fetchBalance();

            const cardsToDrawMap: Record<string, number> = {
                "daily_1": 1,
                "spread_3": 3,
                "spread_5": 5,
                "spread_10": 10,
            };
            const cardsToDraw = cardsToDrawMap[selectedSpread] || 1;

            // Lưu thông tin vào Session Storage để truyền sang trang rút bài
            if (data.question) {
                sessionStorage.setItem(`question_${response.data.sessionId}`, data.question);
            }
            sessionStorage.setItem(`cardsToDraw_${response.data.sessionId}`, cardsToDraw.toString());

            // Redirect sang phòng rút
            router.push(`/reading/session/${response.data.sessionId}`);
        } else {
            setInitError(response.error || "Failed to initialize reading session.");
            setIsInitializing(false);
        }
    };

    return (
        <div className="min-h-screen bg-[#020108] text-zinc-100 selection:bg-purple-500/40 overflow-hidden font-sans text-xs sm:text-sm">
            {/* ===== HỆ THỐNG NỀN CAO CẤP (PREMIUM BACKGROUND) ===== */}
            <div className="fixed inset-0 z-0 pointer-events-none">
                <div className="absolute inset-0 bg-[url('https://grainy-gradients.vercel.app/noise.svg')] opacity-[0.03] mix-blend-overlay"></div>
                <div className="absolute top-[-10%] right-[-5%] w-[60vw] h-[60vw] bg-purple-900/10 blur-[120px] rounded-full animate-pulse" />
                <div className="absolute bottom-[-10%] left-[-5%] w-[50vw] h-[50vw] bg-indigo-900/10 blur-[130px] rounded-full animate-pulse delay-700" />
                
                {/* Particles */}
                <div className="absolute inset-0">
                    {Array.from({ length: 15 }).map((_, i) => (
                        <div
                            key={i}
                            className="absolute w-0.5 h-0.5 bg-white rounded-full animate-float pointer-events-none opacity-[0.1]"
                            style={{
                                top: `${Math.random() * 100}%`,
                                left: `${Math.random() * 100}%`,
                                animationDuration: `${15 + Math.random() * 20}s`,
                                animationDelay: `${-Math.random() * 20}s`,
                            }}
                        />
                    ))}
                </div>
            </div>

            <main className="relative z-10 max-w-5xl mx-auto px-6 pt-24 pb-20">
                {/* Header Section - Compact */}
                <div className="text-center mb-12 animate-in fade-in slide-in-from-bottom-4 duration-700">
                    <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-white/5 border border-white/10 text-[9px] uppercase tracking-[0.2em] font-bold text-purple-400 mb-4 shadow-xl backdrop-blur-md">
                        <Compass className="w-3 h-3" />
                        Ritual Setup
                    </div>
                    <h1 className="text-3xl md:text-5xl font-black tracking-tighter text-white mb-3">
                        {t('title')}
                    </h1>
                    <p className="text-zinc-500 max-w-lg mx-auto text-sm font-medium leading-relaxed">
                        {t('subtitle')}
                    </p>
                </div>

                {initError && (
                    <div className="mb-8 p-3 bg-red-500/10 border border-red-500/20 rounded-xl flex items-center gap-3 text-red-400 text-xs animate-in zoom-in-95">
                        <AlertCircle className="w-4 h-4 flex-shrink-0" />
                        <p>{initError}</p>
                    </div>
                )}

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-10">
                    {/* Spread Selection Grid - 2x2 Compact */}
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-3 animate-in fade-in slide-in-from-bottom-8 duration-700 delay-200">
                        {SPREADS.map((spread) => (
                            <div
                                key={spread.id}
                                onClick={() => setSelectedSpread(spread.id)}
                                className={`group relative p-5 rounded-3xl cursor-pointer transition-all duration-500 border backdrop-blur-sm overflow-hidden ${
                                    selectedSpread === spread.id
                                        ? "bg-white/5 border-purple-500/50 shadow-[0_0_40px_rgba(168,85,247,0.15)]"
                                        : "bg-white/[0.02] border-white/5 hover:border-white/10 hover:bg-white/[0.04]"
                                }`}
                            >
                                <div className="flex justify-between items-start mb-4 relative z-10">
                                    <div className={`w-10 h-10 rounded-2xl flex items-center justify-center border border-white/5 transition-transform duration-500 group-hover:scale-110 ${
                                        selectedSpread === spread.id ? "bg-purple-500 text-black" : "bg-white/5 text-zinc-400"
                                    }`}>
                                        {spread.icon}
                                    </div>
                                    <span className={`px-2 py-0.5 rounded-full text-[9px] font-black uppercase tracking-wider border ${
                                        spread.cost === 'Free' 
                                            ? 'bg-emerald-500/10 text-emerald-400 border-emerald-500/20' 
                                            : spread.id === 'spread_10'
                                                ? 'bg-indigo-500/10 text-indigo-400 border-indigo-500/20'
                                                : 'bg-amber-500/10 text-amber-400 border-amber-500/20'
                                    }`}>
                                        {spread.cost}
                                    </span>
                                </div>
                                
                                <div className="relative z-10">
                                    <h3 className="text-base font-bold text-white mb-1 tracking-tight group-hover:text-purple-300 transition-colors">
                                        {spread.name}
                                    </h3>
                                    <p className="text-zinc-500 text-xs leading-relaxed font-medium">
                                        {spread.desc}
                                    </p>
                                </div>

                                {/* Active Glow Effect */}
                                {selectedSpread === spread.id && (
                                    <div className="absolute inset-0 bg-gradient-to-br from-purple-500/5 to-transparent pointer-events-none"></div>
                                )}
                            </div>
                        ))}
                    </div>

                    {/* Question Input Area - Glassmorphism Compact */}
                    <div className="bg-white/[0.02] p-6 rounded-[2.5rem] border border-white/5 backdrop-blur-md animate-in fade-in slide-in-from-bottom-12 duration-700 delay-400">
                        <label htmlFor="question" className="flex items-center gap-2 text-[10px] font-black uppercase tracking-[0.2em] text-zinc-500 mb-4 ml-1">
                            <Sparkles className="w-3 h-3 text-purple-400" />
                            {t('question_label')}
                        </label>
                        <textarea
                            id="question"
                            rows={3}
                            {...register("question")}
                            placeholder={t('question_placeholder')}
                            className="w-full bg-black/40 border border-white/5 rounded-2xl px-5 py-4 text-sm text-white placeholder-zinc-700 focus:outline-none focus:ring-1 focus:ring-purple-500/30 resize-none transition-all shadow-inner"
                        />
                        {errors.question && (
                            <p className="mt-2 text-[11px] text-red-400 ml-1">{errors.question.message}</p>
                        )}
                    </div>

                    {/* Action Button - Premium Glow */}
                    <div className="flex justify-center pt-2 animate-in fade-in zoom-in-95 duration-700 delay-500">
                        <button
                            type="submit"
                            disabled={isInitializing}
                            className="group relative flex items-center gap-3 bg-white text-black px-12 py-4 rounded-full font-black text-xs uppercase tracking-[0.2em] hover:scale-105 active:scale-[0.98] transition-all shadow-[0_20px_40px_rgba(255,255,255,0.1)] hover:shadow-[0_25px_50px_rgba(255,255,255,0.2)] disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            {isInitializing ? (
                                <>
                                    <Loader2 className="w-4 h-4 animate-spin" />
                                    {t('preparing')}
                                </>
                            ) : (
                                <>
                                    {t('cta_draw')}
                                    <Zap className="w-4 h-4 fill-black" />
                                </>
                            )}
                        </button>
                    </div>
                </form>
            </main>

            {/* Custom Background Animations */}
            <style dangerouslySetInnerHTML={{
                __html: `
                @keyframes float {
                    0% { transform: translateY(0) translateX(0) scale(1); opacity: 0; }
                    20% { opacity: 0.2; }
                    80% { opacity: 0.2; }
                    100% { transform: translateY(-100vh) translateX(30px) scale(0.5); opacity: 0; }
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
