/*
 * ===================================================================
 * FILE: reading/page.tsx (Khởi Tạo Phiên Trải Bài)
 * BỐI CẢNH (CONTEXT):
 *   Trang cho phép User thiết lập phiên xem bài Tarot với AI.
 * 
 * FLOW GIAO DỊCH & STATE:
 *   - Lựa chọn loại trải bài (1 lá, 3 lá, 5 lá, 10 lá) và nhập nội dung câu hỏi.
 *   - Validation Zod giới hạn ký tự câu hỏi.
 *   - Gọi API `initReadingSession` để thanh toán hóa đơn (trừ Gold/Diamond từ Wallet).
 *   - Lưu question và metadata tạm thời vào `sessionStorage` rồi đẩy sang `/reading/session/[id]`.
 * ===================================================================
 */
"use client";

import { useMemo, useState } from "react";
import { useRouter } from "@/i18n/routing";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { initReadingSession } from "@/actions/readingActions";
import { Sparkles, AlertCircle, Compass, Zap, Star, Flame, ShieldCheck, Moon } from "lucide-react";
import { useWalletStore } from "@/store/walletStore";
import { useTranslations } from "next-intl";

import { GlassCard, Button, SectionHeader, Input } from "@/components/ui";

interface FormData {
 question?: string;
}

export default function ReadingSetupPage() {
 const router = useRouter();
 const t = useTranslations("ReadingSetup");
 const fetchBalance = useWalletStore((state) => state.fetchBalance);
 const [selectedSpread, setSelectedSpread] = useState<string>("daily_1");
 const [selectedCurrency, setSelectedCurrency] = useState<"gold" | "diamond">("gold");
 const [initError, setInitError] = useState("");
 const [isInitializing, setIsInitializing] = useState(false);

 const formSchema = useMemo(() => z.object({
  question: z.string().max(300, t("validation.question_too_long")).optional(),
 }), [t]);

 const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
 resolver: zodResolver(formSchema),
 });

 const SPREADS = useMemo(() => [
  { id: "daily_1", name: t("daily_1_name"), desc: t("daily_1_desc"), cost: t("cost_free"), exp: 1, icon: <Star className="w-5 h-5" />, currency: 'gold' },
  { 
   id: "spread_3", 
   name: t("spread_3_name"), 
   desc: t("spread_3_desc"), 
   cost: selectedCurrency === 'diamond' ? t("cost_diamond", { amount: 5 }) : t("cost_gold", { amount: 50 }),
   exp: selectedCurrency === 'diamond' ? 2 : 1,
   icon: <Flame className="w-5 h-5" /> 
  },
  { 
   id: "spread_5", 
   name: t("spread_5_name"), 
   desc: t("spread_5_desc"), 
   cost: selectedCurrency === 'diamond' ? t("cost_diamond", { amount: 10 }) : t("cost_gold", { amount: 100 }),
   exp: selectedCurrency === 'diamond' ? 2 : 1,
   icon: <ShieldCheck className="w-5 h-5" /> 
  },
  { 
   id: "spread_10", 
   name: t("spread_10_name"), 
   desc: t("spread_10_desc"), 
   cost: selectedCurrency === 'gold' ? t("cost_gold", { amount: 500 }) : t("cost_diamond", { amount: 50 }),
   exp: selectedCurrency === 'diamond' ? 2 : 1,
   icon: <Moon className="w-5 h-5" /> 
  },
 ], [t, selectedCurrency]);

 const onSubmit = async (data: FormData) => {
 setIsInitializing(true);
 setInitError("");

 const response = await initReadingSession({
  spreadType: selectedSpread,
  question: data.question,
  currency: selectedCurrency
 });

 if (response.success && response.data) {
 await fetchBalance();

 const cardsToDrawMap: Record<string, number> = {
 "daily_1": 1,
 "spread_3": 3,
 "spread_5": 5,
 "spread_10": 10,
 };
 const cardsToDraw = cardsToDrawMap[selectedSpread] || 1;

 if (data.question) {
 sessionStorage.setItem(`question_${response.data.sessionId}`, data.question);
 }
 sessionStorage.setItem(`cardsToDraw_${response.data.sessionId}`, cardsToDraw.toString());

 router.push(`/reading/session/${response.data.sessionId}`);
 } else {
 setInitError(response.error || t("error_init_failed"));
 setIsInitializing(false);
 }
 };

 return (
 <div className="max-w-4xl mx-auto px-4 sm:px-6 pt-10 pb-20 font-sans">
 <SectionHeader title={t('title')}
 subtitle={t('subtitle')}
 tag={t("tag")}
 tagIcon={<Compass className="w-3 h-3" />}
 className="mb-10"
 />

 {initError && (
 <div className="mb-8 p-4 bg-[var(--danger)]/10 border border-[var(--danger)]/20 rounded-xl flex items-center gap-3 text-[var(--danger)] text-sm animate-in zoom-in-95">
 <AlertCircle className="w-5 h-5 flex-shrink-0" />
 <p>{initError}</p>
 </div>
 )}

  {/* Currency Selector */}
 <div className="flex flex-col items-center mb-10 space-y-4 animate-in fade-in slide-in-from-top-4 duration-700">
  <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] italic">
   {t('select_currency')}
  </label>
  <div className="inline-flex p-1.5 bg-[var(--bg-glass)] border border-[var(--border-default)] rounded-2xl shadow-lg backdrop-blur-xl">
   <button
    type="button"
    onClick={() => setSelectedCurrency("gold")}
    className={`flex items-center gap-2 px-6 py-2.5 rounded-xl text-xs font-black uppercase tracking-wider transition-all duration-500 ${
     selectedCurrency === "gold"
      ? "bg-[var(--purple-accent)] tn-text-ink shadow-[0_0_20px_var(--purple-accent)]"
      : "tn-text-secondary hover:tn-text-primary hover:bg-[var(--purple-50)]"
    }`}
   >
    <Zap className="w-3.5 h-3.5" />
    {t('currency_gold')}
   </button>
   <button
    type="button"
    onClick={() => setSelectedCurrency("diamond")}
    className={`flex items-center gap-2 px-6 py-2.5 rounded-xl text-xs font-black uppercase tracking-wider transition-all duration-500 ${
     selectedCurrency === "diamond"
      ? "bg-[var(--amber-accent)] tn-text-ink shadow-[0_0_20px_var(--amber-accent)]"
      : "tn-text-secondary hover:tn-text-primary hover:bg-[var(--amber-50)]"
    }`}
   >
    <Star className="w-3.5 h-3.5" />
    {t('currency_diamond')}
   </button>
  </div>
 </div>

 <form onSubmit={handleSubmit(onSubmit)} className="space-y-10">
 <div className="grid grid-cols-1 md:grid-cols-2 gap-4 animate-in fade-in slide-in-from-bottom-8 duration-700 delay-200">
 {SPREADS.map((spread) => {
 const isSelected = selectedSpread === spread.id;
 const isFree = spread.id === 'daily_1';
 const isPremiumSpread = spread.id === 'spread_10';
 const showExpBonus = spread.exp > 1;

 return (
 <GlassCard
 key={spread.id}
 variant={isSelected ? "elevated" : "interactive"}
 className={`group relative cursor-pointer transition-all duration-500 overflow-hidden ${
 isSelected ? "border-[var(--purple-accent)]/50 shadow-[0_0_30px_var(--c-168-85-247-15)] ring-1 ring-[var(--purple-accent)]/50" : "hover:tn-border"
 }`}
 onClick={() => setSelectedSpread(spread.id)}
 padding="lg"
 >
 <div className="flex justify-between items-start mb-4 relative z-10">
 <div className={`w-12 h-12 rounded-2xl flex items-center justify-center border transition-all duration-500 ${
 isSelected ? "bg-[var(--purple-accent)] tn-text-ink border-transparent scale-110 shadow-[0_0_20px_var(--purple-accent)]" : "tn-surface tn-text-secondary tn-border-soft group-hover:tn-surface-strong group-hover:scale-105"
 }`}>
 {spread.icon}
 </div>
 <div className="flex flex-col items-end gap-2">
 <span className={`px-3 py-1 rounded-full text-[10px] font-black uppercase tracking-widest border transition-colors ${
 isFree ? 'bg-[var(--success-bg)] text-[var(--success)] border-[var(--success)]' 
 : selectedCurrency === 'diamond' 
 ? 'bg-[var(--info-bg)] text-[var(--info)] border-[var(--info)] shadow-[0_0_10px_var(--info-bg)]'
 : 'bg-[var(--warning-bg)] text-[var(--warning)] border-[var(--warning)]'
 }`}>
 {spread.cost}
 </span>
 <span className={`px-2 py-0.5 rounded-lg text-[8px] font-black uppercase tracking-tighter border animate-pulse ${
 showExpBonus 
 ? 'bg-[var(--purple-accent)] tn-text-ink border-transparent shadow-[0_0_10px_var(--purple-accent)]'
 : 'tn-surface tn-text-tertiary tn-border-soft'
 }`}>
 {t('exp_bonus', { amount: spread.exp })}
 </span>
 </div>
 </div>
 <div className="relative z-10">
 <h3 className={`text-xl font-bold mb-2 tracking-tight transition-colors ${
 isSelected ? 'text-[var(--text-primary)]' : 'text-[var(--text-secondary)] group-hover:tn-text-primary'
 }`}>
 {spread.name}
 </h3>
 <p className="text-[var(--text-tertiary)] text-sm leading-relaxed font-medium">
 {spread.desc}
 </p>
 </div>

 {/* Active Glow Effect */}
 {isSelected && (
 <div className="absolute inset-0 bg-gradient-to-br from-[var(--purple-accent)]/10 to-transparent pointer-events-none " />
 )}
 </GlassCard>
 )
 })}
 </div>

 <div className="animate-in fade-in slide-in-from-bottom-12 duration-700 delay-400">
 <Input
 label={t('question_label')}
 isTextarea
 placeholder={t('question_placeholder')}
 leftIcon={<Sparkles className="w-5 h-5 text-[var(--purple-accent)]" />}
 error={errors.question?.message}
 {...register("question")}
 />
 </div>

 <div className="flex justify-center pt-4 animate-in fade-in zoom-in-95 duration-700 delay-500">
 <Button
 type="submit"
 variant="brand"
 size="lg"
 isLoading={isInitializing}
 rightIcon={!isInitializing && <Zap className="w-5 h-5 ml-2" />}
 className="w-full sm:w-auto rounded-full font-black tracking-widest uppercase shadow-[0_10px_40px_var(--c-255-255-255-15)] hover:shadow-[0_15px_50px_var(--c-255-255-255-25)]"
 >
 {isInitializing ? t('preparing') : t('cta_draw')}
 </Button>
 </div>
 </form>
 </div>
 );
}
