'use client';

/**
 * Trang Nạp Diamond (Deposit Page)
 * Tích hợp UserLayout và SectionHeader
 */

import React, { useEffect, useState } from 'react';
import { createDepositOrder, CreateDepositOrderResponse } from '@/actions/depositActions';
import { listPromotions, DepositPromotion } from '@/actions/promotionActions';
import { useWalletStore } from '@/store/walletStore';
import { 
    Gem, ArrowLeft, Sparkles, Zap, Crown, 
    CheckCircle2, Loader2, AlertCircle, ExternalLink,
    Activity, CreditCard, Coins
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';

import UserLayout from '@/components/layout/UserLayout';
import { SectionHeader, Button, GlassCard } from '@/components/ui';

const EXCHANGE_RATE = 1000;

const PRESET_AMOUNTS = [
    { vnd: 50_000, label: '50K', icon: Zap, color: 'text-[var(--info)]' },
    { vnd: 100_000, label: '100K', icon: Zap, color: 'text-[var(--info)]' },
    { vnd: 200_000, label: '200K', icon: Sparkles, color: 'text-[var(--purple-accent)]' },
    { vnd: 500_000, label: '500K', icon: Sparkles, color: 'text-[var(--purple-accent)]' },
    { vnd: 1_000_000, label: '1M', icon: Crown, color: 'text-[var(--warning)]' },
    { vnd: 2_000_000, label: '2M', icon: Crown, color: 'text-[var(--warning)]' },
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
        <UserLayout>
            <div className="max-w-5xl mx-auto px-6 pt-8 pb-32 font-sans relative">
                
                <button 
                    onClick={() => router.push('/wallet')}
                    className="group flex items-center gap-2 text-[var(--text-secondary)] hover:text-white transition-colors text-[10px] font-black uppercase tracking-[0.2em] mb-8 w-fit"
                >
                    <ArrowLeft className="w-3.5 h-3.5 transition-transform group-hover:-translate-x-1" />
                    Quay lại kho báu
                </button>

                {/* Header Section */}
                <SectionHeader
                    tag="Energy Supply"
                    tagIcon={<Zap className="w-3 h-3 text-[var(--warning)]" />}
                    title="Tiếp Nạp Diamond"
                    subtitle="Cung cấp năng lượng Diamond để thực hiện các giao thức tiên tri cao cấp nhất."
                    className="mb-12 animate-in fade-in slide-in-from-bottom-4 duration-1000 w-full"
                />

                {/* Grid Layout: Controls & Confirmation */}
                <div className="grid grid-cols-1 lg:grid-cols-12 gap-10">
                    
                    {/* Left side: Amounts Selection */}
                    <div className="lg:col-span-8 space-y-10 animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-200">
                        
                        {/* Current Balance Tiny Card */}
                        <GlassCard className="flex items-center justify-between !p-6 rounded-[2rem] border border-white/5">
                            <div className="flex items-center gap-4">
                                <div className="w-10 h-10 rounded-xl bg-[var(--purple-accent)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20">
                                    <Gem className="w-5 h-5 text-[var(--purple-accent)]" />
                                </div>
                                <div>
                                    <div className="text-[9px] font-black uppercase tracking-widest text-[var(--text-secondary)] text-left">Số dư hiện tại</div>
                                    <div className="text-xl font-black text-white italic">
                                        {isMounted ? (balance?.diamondBalance.toLocaleString() ?? '...') : '...'}
                                    </div>
                                </div>
                            </div>
                            <Activity className="w-5 h-5 text-zinc-800" />
                        </GlassCard>

                        {/* Preset Selection Grid */}
                        <div className="space-y-6 text-left">
                            <h2 className="text-lg font-black text-white uppercase italic tracking-tighter flex items-center gap-3">
                                <Sparkles className="w-4 h-4 text-[var(--warning)]" />
                                Chọn Mức Tiếp Nạp
                            </h2>
                            <div className="grid grid-cols-2 lg:grid-cols-3 gap-4">
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
                                                    ? 'bg-[var(--purple-accent)]/10 border-[var(--purple-accent)]/40 ring-1 ring-[var(--purple-accent)]/20 shadow-[0_0_40px_rgba(168,85,247,0.1)]' 
                                                    : 'bg-white/[0.02] border-white/5 hover:bg-white/[0.04] hover:border-white/10'
                                                }
                                            `}
                                        >
                                            <div className="relative z-10 flex flex-col h-full justify-between">
                                                <div className="flex items-center justify-between">
                                                    <div className={`p-2 rounded-xl border ${isSelected ? 'bg-[var(--purple-accent)]/20 border-[var(--purple-accent)]/30' : 'bg-white/5 border-white/10'} transition-all`}>
                                                        <Icon className={`w-5 h-5 ${isSelected ? 'text-white' : preset.color + ' group-hover:text-white'}`} style={isSelected ? {} : { opacity: 0.8 }} />
                                                    </div>
                                                    {promoForThis && (
                                                        <div className="bg-[var(--warning)] text-black text-[8px] font-black uppercase tracking-widest px-2 py-0.5 rounded-full animate-pulse shadow-lg flex items-center gap-1">
                                                            <Coins className="w-2.5 h-2.5" />
                                                            +{promoForThis.bonusDiamond} Bonus
                                                        </div>
                                                    )}
                                                </div>
                                                <div>
                                                    <div className="text-2xl font-black text-white italic tracking-tighter mb-1">
                                                        {preset.label}
                                                    </div>
                                                    <div className="text-[10px] font-bold text-[var(--text-secondary)] uppercase tracking-widest leading-none">
                                                        {isMounted ? preset.vnd.toLocaleString() : preset.vnd} VND
                                                    </div>
                                                    <div className="text-[11px] font-black text-[var(--purple-accent)] uppercase tracking-tighter mt-2">
                                                        👉 {isMounted ? diamondForThis.toLocaleString() : diamondForThis} 💎
                                                    </div>
                                                </div>
                                            </div>
                                            
                                            {/* Decor Glow */}
                                            <div className={`absolute -bottom-8 -right-8 w-24 h-24 bg-[var(--purple-accent)]/10 blur-2xl rounded-full transition-opacity duration-700 ${isSelected ? 'opacity-100' : 'opacity-0 group-hover:opacity-100'}`} />
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
                                        ? 'bg-[var(--purple-accent)]/5 border-[var(--purple-accent)]/30 ring-1 ring-[var(--purple-accent)]/15' 
                                        : 'bg-white/[0.01] border-white/5 hover:bg-white/[0.03] hover:border-white/10'
                                    }
                                `}
                                onClick={handleCustomFocus}
                            >
                                <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)] block mb-4 group-hover:text-[var(--text-primary)] transition-colors">
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
                                        className="bg-transparent border-none outline-none text-4xl font-black text-white w-full placeholder:text-zinc-800 italic tracking-tighter"
                                    />
                                    <span className="text-xl font-bold text-zinc-600 uppercase italic">VND</span>
                                </div>
                                
                                {isCustom && actualAmount > 0 && (
                                    <div className="mt-6 flex items-center gap-3 animate-in fade-in slide-in-from-left-2">
                                        <Gem className="w-4 h-4 text-[var(--purple-accent)]" />
                                        <span className="text-xs font-black uppercase text-[var(--purple-accent)] tracking-tighter">
                                            {isMounted ? baseDiamond.toLocaleString() : baseDiamond} Diamond
                                        </span>
                                        {bonusGold > 0 && (
                                            <>
                                                <span className="text-zinc-700 mx-2">|</span>
                                                <Coins className="w-4 h-4 text-[var(--warning)]" />
                                                <span className="text-xs font-black uppercase text-[var(--warning)] tracking-tighter">
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
                            <GlassCard className="relative group overflow-hidden !p-8 shadow-2xl">
                                <div className="absolute top-0 right-0 p-6 opacity-5 pointer-events-none group-hover:scale-110 group-hover:rotate-6 transition-transform duration-700">
                                    <CreditCard className="w-32 h-32 text-zinc-400" />
                                </div>

                                <h2 className="text-xl font-black text-white uppercase italic tracking-tighter mb-8 flex items-center gap-3">
                                    <Crown className="w-5 h-5 text-[var(--warning)]" />
                                    Xác Nhận Đơn Hàng
                                </h2>

                                <div className="space-y-6 mb-10">
                                    <div className="flex justify-between items-end">
                                        <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] text-left">Giá trị nạp</div>
                                        <div className="text-xl font-black text-white italic tracking-tighter">
                                            {isMounted ? actualAmount.toLocaleString() : actualAmount} <span className="text-[10px] text-[var(--text-tertiary)] not-italic ml-1">VND</span>
                                        </div>
                                    </div>
                                    
                                    <div className="flex justify-between items-end py-4 border-y border-white/5">
                                        <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] text-left">Số Diamond nhận</div>
                                        <div className="text-xl font-black text-[var(--purple-accent)] italic tracking-tighter">
                                            {isMounted ? baseDiamond.toLocaleString() : baseDiamond} <span className="text-[10px] drop-shadow-md not-italic ml-1">💎</span>
                                        </div>
                                    </div>

                                    {bonusGold > 0 && (
                                        <div className="flex justify-between items-end">
                                            <div className="text-[10px] font-black uppercase tracking-widest text-[var(--warning)] opacity-80 text-left">Khuyến mãi tặng (Gold)</div>
                                            <div className="text-lg font-black text-[var(--warning)] italic tracking-tighter">
                                                +{isMounted ? bonusGold.toLocaleString() : bonusGold} <span className="text-[10px] drop-shadow-md not-italic ml-1">🪙</span>
                                            </div>
                                        </div>
                                    )}

                                    <div className="pt-4 flex flex-col items-center gap-2">
                                        <div className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">Tổng cộng tài sản</div>
                                        <div className="flex items-center gap-4">
                                            <div className="text-3xl font-black text-white italic tracking-tighter">
                                                {isMounted ? baseDiamond.toLocaleString() : baseDiamond} 💎
                                            </div>
                                            {bonusGold > 0 && (
                                                <>
                                                    <div className="text-zinc-800 font-bold text-xl">+</div>
                                                    <div className="text-3xl font-black text-[var(--warning)] italic tracking-tighter">
                                                        {isMounted ? bonusGold.toLocaleString() : bonusGold} 🪙
                                                    </div>
                                                </>
                                            )}
                                        </div>
                                    </div>
                                </div>

                                {error && (
                                    <div className="mb-6 p-4 rounded-2xl bg-[var(--danger-bg)] border border-[var(--danger)] text-[var(--danger)] text-[10px] font-bold uppercase tracking-widest flex items-center gap-3 animate-in zoom-in-95">
                                        <AlertCircle className="w-4 h-4 flex-shrink-0" />
                                        {error}
                                    </div>
                                )}

                                {orderResult && !error && (
                                    <div className="mb-6 p-4 rounded-2xl bg-[var(--success-bg)] border border-[var(--success)] text-[var(--success)] space-y-2 animate-in slide-in-from-top-4">
                                        <div className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest">
                                            <CheckCircle2 className="w-4 h-4" />
                                            Đơn hàng đã sẵn sàng
                                        </div>
                                        {orderResult.paymentUrl && (
                                            <a
                                                href={orderResult.paymentUrl}
                                                target="_blank"
                                                rel="noopener noreferrer"
                                                className="flex items-center justify-center gap-2 py-2 px-4 bg-[var(--success)] text-white rounded-xl text-[10px] font-black uppercase tracking-widest hover:scale-105 transition-all"
                                            >
                                                Thanh toán ngay <ExternalLink className="w-3 h-3" />
                                            </a>
                                        )}
                                    </div>
                                )}

                                <Button
                                    variant="primary"
                                    onClick={handleDeposit}
                                    disabled={!isValid || isSubmitting}
                                    className="w-full h-14"
                                >
                                    {isSubmitting ? (
                                        <>
                                            <Loader2 className="w-4 h-4 animate-spin mr-2" />
                                            Đang kích hoạt...
                                        </>
                                    ) : (
                                        <>
                                            <Zap className="w-4 h-4 mr-2" />
                                            Tiếp Nạp Diamond
                                        </>
                                    )}
                                </Button>
                                
                                <p className="text-[8px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] text-center mt-6 leading-relaxed">
                                    Mã hóa bảo mật bởi TarotNow Financial Protocol
                                </p>
                            </GlassCard>
                            
                            {/* Summary Note */}
                            <div className="p-6 bg-white/[0.01] rounded-[1.5rem] border border-white/5 space-y-3">
                                <div className="text-[9px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Ghi chú giao dịch</div>
                                <p className="text-[10px] font-medium text-[var(--text-tertiary)] leading-relaxed">
                                    • 1 Diamond tương đương 1,000 VND.<br/>
                                    • Diamond dùng cho bài cao cấp, Gold dùng cho điểm danh & quà tặng.<br/>
                                    • Giao dịch được xử lý hoàn toàn tự động sau 1-3 phút.
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </UserLayout>
    );
}
