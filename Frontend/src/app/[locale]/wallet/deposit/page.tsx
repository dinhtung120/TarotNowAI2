'use client';

/**
 * Trang Nạp Diamond (Deposit Page) - Astral Premium Redesign
 * 
 * Các cải tiến:
 * 1. Hệ thống Nền Astral: Nebula + Spiritual Particles.
 * 2. Premium Preset Grid: Các mức nạp được thiết kế dạng Glass Cards với hiệu ứng hover nâng cao.
 * 3. 3D Order Confirmation: Thẻ xác nhận đơn hàng với phong cách Glassmorphism 3D.
 * 4. Navbar Fix: Sử dụng pt-28 để đảm bảo hiển thị bên dưới thanh điều hướng.
 * 5. Gold Promotion: Thưởng khuyến mãi được chuyển sang Gold (Coins) thay vì Diamond.
 */

import React, { useEffect, useState } from 'react';
import { createDepositOrder, CreateDepositOrderResponse } from '@/actions/depositActions';
import { listPromotions, DepositPromotion } from '@/actions/promotionActions';
import { useWalletStore } from '@/store/walletStore';
import { 
    Gem, ArrowLeft, Sparkles, Zap, Crown, 
    Gift, CheckCircle2, Loader2, AlertCircle, ExternalLink,
    ChevronRight, Wallet, Activity, CreditCard, Coins
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';

const EXCHANGE_RATE = 1000;

const PRESET_AMOUNTS = [
    { vnd: 50_000, label: '50K', icon: Zap, color: 'text-cyan-400' },
    { vnd: 100_000, label: '100K', icon: Zap, color: 'text-cyan-400' },
    { vnd: 200_000, label: '200K', icon: Sparkles, color: 'text-purple-400' },
    { vnd: 500_000, label: '500K', icon: Sparkles, color: 'text-purple-400' },
    { vnd: 1_000_000, label: '1M', icon: Crown, color: 'text-amber-400' },
    { vnd: 2_000_000, label: '2M', icon: Crown, color: 'text-amber-400' },
];

export default function DepositPage() {
    const router = useRouter();
    const { balance, fetchBalance } = useWalletStore();
    const [selectedAmount, setSelectedAmount] = useState<number>(100_000);
    const [customAmount, setCustomAmount] = useState<string>('');
    const [isCustom, setIsCustom] = useState(false);

    const [promotions, setPromotions] = useState<DepositPromotion[]>([]);
    const [isLoadingPromos, setIsLoadingPromos] = useState(true);

    const [isSubmitting, setIsSubmitting] = useState(false);
    const [orderResult, setOrderResult] = useState<CreateDepositOrderResponse | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [isMounted, setIsMounted] = useState(false);

    useEffect(() => {
        setIsMounted(true);
        fetchBalance();
        const fetchPromos = async () => {
            setIsLoadingPromos(true);
            const data = await listPromotions(true);
            setPromotions(data ?? []);
            setIsLoadingPromos(false);
        };
        fetchPromos();
    }, [fetchBalance]);

    const actualAmount = isCustom ? (parseInt(customAmount) || 0) : selectedAmount;
    const baseDiamond = Math.floor(actualAmount / EXCHANGE_RATE);
    
    // Tìm khuyến mãi tốt nhất. Mặc dù field là bonusDiamond nhưng theo business logic mới, đây là Gold.
    const bestPromotion = promotions
        .filter(p => p.isActive && p.minAmountVnd <= actualAmount)
        .sort((a, b) => b.bonusDiamond - a.bonusDiamond)[0];
    const bonusGold = bestPromotion?.bonusDiamond ?? 0;
    const isValid = actualAmount >= 10_000;

    const handlePresetSelect = (amount: number) => {
        setSelectedAmount(amount);
        setIsCustom(false);
        setCustomAmount('');
        setError(null);
        setOrderResult(null);
    };

    const handleCustomFocus = () => {
        setIsCustom(true);
        setError(null);
        setOrderResult(null);
    };

    const handleDeposit = async () => {
        if (!isValid) {
            setError('Số tiền nạp tối thiểu là 10.000 VND.');
            return;
        }
        setIsSubmitting(true);
        setError(null);
        try {
            const result = await createDepositOrder(actualAmount);
            if (!result) {
                setError('Không thể tạo đơn nạp tiền. Vui lòng thử lại sau.');
                return;
            }
            setOrderResult(result);
            if (result.paymentUrl) {
                setTimeout(() => {
                    window.open(result.paymentUrl, '_blank');
                }, 1500);
            }
        } catch (err) {
            setError('Đã xảy ra lỗi. Vui lòng thử lại.');
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="min-h-screen bg-[#020108] text-zinc-100 selection:bg-purple-500/40 overflow-x-hidden font-sans pb-32">
            {/* ##### PREMIUM BACKGROUND SYSTEM ##### */}
            <div className="fixed inset-0 z-0 pointer-events-none">
                <div className="absolute inset-0 bg-[url('https://grainy-gradients.vercel.app/noise.svg')] opacity-[0.03] mix-blend-overlay"></div>
                <div className="absolute top-0 -right-1/4 w-[70vw] h-[70vw] bg-purple-900/[0.1] blur-[120px] rounded-full animate-slow-pulse" />
                <div className="absolute bottom-0 -left-1/4 w-[60vw] h-[60vw] bg-indigo-900/[0.05] blur-[130px] rounded-full animate-slow-pulse delay-700" />
                
                {/* Spiritual Particles */}
                <div className="absolute inset-0">
                    {Array.from({ length: 25 }).map((_, i) => (
                        <div
                            key={i}
                            className="absolute w-[1px] h-[1px] bg-white rounded-full animate-float opacity-[0.1]"
                            style={{
                                top: `${Math.random() * 100}%`,
                                left: `${Math.random() * 100}%`,
                                animationDuration: `${20 + Math.random() * 30}s`,
                                animationDelay: `${-Math.random() * 20}s`,
                            }}
                        />
                    ))}
                </div>
            </div>

            <main className="relative z-10 max-w-5xl mx-auto px-6 pt-28">
                {/* Navigation & Header */}
                <div className="mb-12 animate-in fade-in slide-in-from-bottom-4 duration-1000">
                    <button 
                        onClick={() => router.push('/wallet')}
                        className="group flex items-center gap-2 text-zinc-500 hover:text-white transition-colors text-[10px] font-black uppercase tracking-[0.2em] mb-8"
                    >
                        <ArrowLeft className="w-3.5 h-3.5 transition-transform group-hover:-translate-x-1" />
                        Quay lại kho báu
                    </button>

                    <div className="space-y-4">
                        <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-purple-500/5 border border-purple-500/10 text-[9px] uppercase tracking-[0.2em] font-black text-purple-400 shadow-xl backdrop-blur-md">
                            <Zap className="w-3 h-3 text-amber-500" />
                            Energy Supply
                        </div>
                        <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white uppercase italic">
                            Tiếp Nạp Diamond
                        </h1>
                        <p className="text-zinc-500 font-medium max-w-lg text-sm leading-relaxed">
                            Cung cấp năng lượng Diamond để thực hiện các giao thức tiên tri cao cấp nhất.
                        </p>
                    </div>
                </div>

                {/* Grid Layout: Controls & Confirmation */}
                <div className="grid grid-cols-1 lg:grid-cols-12 gap-10">
                    
                    {/* Left side: Amounts Selection */}
                    <div className="lg:col-span-8 space-y-10 animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-200">
                        {/* Current Balance Tiny Card */}
                        <div className="flex items-center justify-between p-6 bg-white/[0.02] backdrop-blur-3xl rounded-[2rem] border border-white/5">
                            <div className="flex items-center gap-4">
                                <div className="w-10 h-10 rounded-xl bg-purple-500/10 flex items-center justify-center border border-purple-500/20">
                                    <Gem className="w-5 h-5 text-purple-400" />
                                </div>
                                <div>
                                    <div className="text-[9px] font-black uppercase tracking-widest text-zinc-500 text-left">Số dư hiện tại</div>
                                    <div className="text-xl font-black text-white italic">
                                        {isMounted ? (balance?.diamondBalance.toLocaleString() ?? '...') : '...'}
                                    </div>
                                </div>
                            </div>
                            <Activity className="w-5 h-5 text-zinc-800" />
                        </div>

                        {/* Preset Selection Grid */}
                        <div className="space-y-6 text-left">
                            <h2 className="text-lg font-black text-white uppercase italic tracking-tighter flex items-center gap-3">
                                <Sparkles className="w-4 h-4 text-amber-500" />
                                Chọn Mức Tiếp Nạp
                            </h2>
                            <div className="grid grid-cols-2 md:grid-cols-3 gap-4">
                                {PRESET_AMOUNTS.map((preset) => {
                                    const isSelected = !isCustom && selectedAmount === preset.vnd;
                                    const Icon = preset.icon;
                                    const diamondForThis = Math.floor(preset.vnd / EXCHANGE_RATE);
                                    const promoForThis = promotions
                                        .filter(p => p.isActive && p.minAmountVnd <= preset.vnd)
                                        .sort((a, b) => b.bonusDiamond - a.bonusDiamond)[0];

                                    return (
                                        <button
                                            key={preset.vnd}
                                            onClick={() => handlePresetSelect(preset.vnd)}
                                            className={`
                                                relative h-40 group p-6 rounded-[2.5rem] text-left transition-all duration-500 overflow-hidden
                                                border ${isSelected 
                                                    ? 'bg-purple-500/10 border-purple-500/40 ring-1 ring-purple-500/20 shadow-[0_0_40px_rgba(168,85,247,0.1)]' 
                                                    : 'bg-white/[0.02] border-white/5 hover:bg-white/[0.04] hover:border-white/10'
                                                }
                                            `}
                                        >
                                            <div className="relative z-10 flex flex-col h-full justify-between">
                                                <div className="flex items-center justify-between">
                                                    <div className={`p-2 rounded-xl border ${isSelected ? 'bg-purple-500/20 border-purple-500/30' : 'bg-white/5 border-white/10'} transition-all`}>
                                                        <Icon className={`w-5 h-5 ${isSelected ? 'text-white' : 'text-zinc-600 group-hover:text-zinc-400'}`} />
                                                    </div>
                                                    {promoForThis && (
                                                        <div className="bg-amber-500/90 text-white text-[8px] font-black uppercase tracking-widest px-2 py-0.5 rounded-full animate-pulse shadow-lg flex items-center gap-1">
                                                            <Coins className="w-2.5 h-2.5" />
                                                            +{promoForThis.bonusDiamond} Bonus
                                                        </div>
                                                    )}
                                                </div>
                                                <div>
                                                    <div className="text-2xl font-black text-white italic tracking-tighter mb-1">
                                                        {preset.label}
                                                    </div>
                                                    <div className="text-[10px] font-bold text-zinc-500 uppercase tracking-widest leading-none">
                                                        {isMounted ? preset.vnd.toLocaleString() : preset.vnd} VND
                                                    </div>
                                                    <div className="text-[11px] font-black text-purple-400 uppercase tracking-tighter mt-2">
                                                        👉 {isMounted ? diamondForThis.toLocaleString() : diamondForThis} 💎
                                                    </div>
                                                </div>
                                            </div>
                                            
                                            {/* Decor Glow */}
                                            <div className={`absolute -bottom-8 -right-8 w-24 h-24 bg-purple-500/10 blur-2xl rounded-full transition-opacity duration-700 ${isSelected ? 'opacity-100' : 'opacity-0 group-hover:opacity-100'}`} />
                                        </button>
                                    );
                                })}
                            </div>
                        </div>

                        {/* Custom Amount Input */}
                        <div className="space-y-4 text-left">
                             <div 
                                className={`
                                    p-8 rounded-[2.5rem] border transition-all duration-500 cursor-text group
                                    ${isCustom 
                                        ? 'bg-purple-500/5 border-purple-500/30 ring-1 ring-purple-500/15' 
                                        : 'bg-white/[0.01] border-white/5 hover:bg-white/[0.03] hover:border-white/10'
                                    }
                                `}
                                onClick={handleCustomFocus}
                            >
                                <label className="text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600 block mb-4 group-hover:text-zinc-400 transition-colors">
                                    Hoặc Nhập Số Năng Lượng Tùy Chỉnh (VND)
                                </label>
                                <div className="flex items-baseline gap-4">
                                    <input
                                        type="number"
                                        value={customAmount}
                                        onChange={(e) => {
                                            setCustomAmount(e.target.value);
                                            setIsCustom(true);
                                            setError(null);
                                            setOrderResult(null);
                                        }}
                                        onFocus={handleCustomFocus}
                                        placeholder="Min 10,000"
                                        className="bg-transparent border-none outline-none text-4xl font-black text-white w-full placeholder:text-zinc-900 italic tracking-tighter"
                                    />
                                    <span className="text-xl font-bold text-zinc-700 uppercase italic">VND</span>
                                </div>
                                
                                {isCustom && actualAmount > 0 && (
                                    <div className="mt-6 flex items-center gap-3 animate-in fade-in slide-in-from-left-2">
                                        <Gem className="w-4 h-4 text-purple-400" />
                                        <span className="text-xs font-black uppercase text-purple-300 tracking-tighter">
                                            {isMounted ? baseDiamond.toLocaleString() : baseDiamond} Diamond
                                        </span>
                                        {bonusGold > 0 && (
                                            <>
                                                <span className="text-zinc-700 mx-2">|</span>
                                                <Coins className="w-4 h-4 text-amber-500" />
                                                <span className="text-xs font-black uppercase text-amber-400 tracking-tighter">
                                                    + {isMounted ? bonusGold.toLocaleString() : bonusGold} Bonus Gold 🎁
                                                </span>
                                            </>
                                        )}
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>

                    {/* Right side: Confirmation Side-Card */}
                    <div className="lg:col-span-4 animate-in fade-in slide-in-from-right-8 duration-1000 delay-400">
                        <div className="sticky top-28 space-y-6">
                            <div className="relative group overflow-hidden bg-zinc-900/40 backdrop-blur-3xl rounded-[3rem] border border-white/10 p-8 shadow-2xl">
                                <div className="absolute top-0 right-0 p-6 opacity-[0.03] pointer-events-none group-hover:scale-110 group-hover:rotate-6 transition-transform duration-700">
                                    <CreditCard className="w-32 h-32" />
                                </div>

                                <h2 className="text-xl font-black text-white uppercase italic tracking-tighter mb-8 flex items-center gap-3">
                                    <Crown className="w-5 h-5 text-amber-500" />
                                    Xác Nhận Đơn Hàng
                                </h2>

                                <div className="space-y-6 mb-10">
                                    <div className="flex justify-between items-end">
                                        <div className="text-[10px] font-black uppercase tracking-widest text-zinc-600 text-left">Giá trị nạp</div>
                                        <div className="text-xl font-black text-white italic tracking-tighter">
                                            {isMounted ? actualAmount.toLocaleString() : actualAmount} <span className="text-[10px] text-zinc-500 not-italic ml-1">VND</span>
                                        </div>
                                    </div>
                                    
                                    <div className="flex justify-between items-end py-4 border-y border-white/5">
                                        <div className="text-[10px] font-black uppercase tracking-widest text-zinc-600 text-left">Số Diamond nhận</div>
                                        <div className="text-xl font-black text-purple-400 italic tracking-tighter">
                                            {isMounted ? baseDiamond.toLocaleString() : baseDiamond} <span className="text-[10px] text-zinc-600 not-italic ml-1">💎</span>
                                        </div>
                                    </div>

                                    {bonusGold > 0 && (
                                        <div className="flex justify-between items-end">
                                            <div className="text-[10px] font-black uppercase tracking-widest text-amber-500/60 text-left">Khuyến mãi tặng (Gold)</div>
                                            <div className="text-lg font-black text-amber-500 italic tracking-tighter">
                                                +{isMounted ? bonusGold.toLocaleString() : bonusGold} <span className="text-[10px] text-zinc-600 not-italic ml-1">🪙</span>
                                            </div>
                                        </div>
                                    )}

                                    <div className="pt-4 flex flex-col items-center gap-2">
                                        <div className="text-[10px] font-black uppercase tracking-[0.3em] text-zinc-600">Tổng cộng tài sản</div>
                                        <div className="flex items-center gap-4">
                                            <div className="text-3xl font-black text-white italic tracking-tighter">
                                                {isMounted ? baseDiamond.toLocaleString() : baseDiamond} 💎
                                            </div>
                                            {bonusGold > 0 && (
                                                <>
                                                    <div className="text-zinc-800 font-bold text-xl">+</div>
                                                    <div className="text-3xl font-black text-amber-500 italic tracking-tighter">
                                                        {isMounted ? bonusGold.toLocaleString() : bonusGold} 🪙
                                                    </div>
                                                </>
                                            )}
                                        </div>
                                    </div>
                                </div>

                                {error && (
                                    <div className="mb-6 p-4 rounded-2xl bg-rose-500/10 border border-rose-500/20 text-rose-400 text-[10px] font-bold uppercase tracking-widest flex items-center gap-3 animate-in zoom-in-95">
                                        <AlertCircle className="w-4 h-4 flex-shrink-0" />
                                        {error}
                                    </div>
                                )}

                                {orderResult && !error && (
                                    <div className="mb-6 p-4 rounded-2xl bg-emerald-500/10 border border-emerald-500/20 text-emerald-400 space-y-2 animate-in slide-in-from-top-4">
                                        <div className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest">
                                            <CheckCircle2 className="w-4 h-4" />
                                            Đơn hàng đã sẵn sàng
                                        </div>
                                        {orderResult.paymentUrl && (
                                            <a
                                                href={orderResult.paymentUrl}
                                                target="_blank"
                                                rel="noopener noreferrer"
                                                className="flex items-center justify-center gap-2 py-2 px-4 bg-emerald-500 text-black rounded-xl text-[10px] font-black uppercase tracking-widest hover:scale-105 transition-all"
                                            >
                                                Thanh toán ngay <ExternalLink className="w-3 h-3" />
                                            </a>
                                        )}
                                    </div>
                                )}

                                <button
                                    onClick={handleDeposit}
                                    disabled={!isValid || isSubmitting}
                                    className={`
                                        w-full py-5 rounded-[2rem] font-black text-[11px] uppercase tracking-[0.3em] transition-all duration-500
                                        flex items-center justify-center gap-3 shadow-2xl
                                        ${isValid && !isSubmitting
                                            ? 'bg-white text-black hover:scale-[1.03] active:scale-95 group-hover:bg-purple-500 group-hover:text-white'
                                            : 'bg-zinc-800 text-zinc-600 cursor-not-allowed opacity-40'
                                        }
                                    `}
                                >
                                    {isSubmitting ? (
                                        <>
                                            <Loader2 className="w-4 h-4 animate-spin" />
                                            Đang kích hoạt...
                                        </>
                                    ) : (
                                        <>
                                            <Zap className="w-4 h-4" />
                                            Tiếp Nạp Diamond
                                        </>
                                    )}
                                </button>
                                
                                <p className="text-[8px] font-black uppercase tracking-[0.2em] text-zinc-700 text-center mt-6 leading-relaxed">
                                    Mã hóa bảo mật bởi TarotNow Financial Protocol
                                </p>
                            </div>
                            
                            {/* Summary Note */}
                            <div className="p-6 bg-white/[0.01] rounded-3xl border border-white/5 space-y-3">
                                <div className="text-[9px] font-black uppercase tracking-widest text-zinc-600">Ghi chú giao dịch</div>
                                <p className="text-[10px] font-medium text-zinc-500 leading-relaxed">
                                    • 1 Diamond tương đương 1,000 VND.<br/>
                                    • Diamond dùng cho bài cao cấp, Gold dùng cho điểm danh & quà tặng.<br/>
                                    • Giao dịch được xử lý hoàn toàn tự động sau 1-3 phút.
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </main>

            <style dangerouslySetInnerHTML={{
                __html: `
                @keyframes float {
                    0% { transform: translateY(0) rotate(0deg); opacity: 0; }
                    15% { opacity: 0.15; }
                    85% { opacity: 0.15; }
                    100% { transform: translateY(-100vh) rotate(360deg); opacity: 0; }
                }
                .animate-float {
                    animation-name: float;
                    animation-timing-function: linear;
                    animation-iteration-count: infinite;
                }
                @keyframes slow-pulse {
                    0%, 100% { opacity: 0.05; transform: scale(1); }
                    50% { opacity: 0.12; transform: scale(1.1); }
                }
                .animate-slow-pulse {
                    animation: slow-pulse 15s ease-in-out infinite;
                }
            `}} />
        </div>
    );
}
