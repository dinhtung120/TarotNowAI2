'use client';

import React, { useEffect, useState } from 'react';
import { createDepositOrder, CreateDepositOrderResponse } from '@/actions/depositActions';
import { listPromotions, DepositPromotion } from '@/actions/promotionActions';
import { useWalletStore } from '@/store/walletStore';
import { 
    Gem, ArrowLeft, Sparkles, Zap, Crown, 
    Gift, CheckCircle2, Loader2, AlertCircle, ExternalLink
} from 'lucide-react';
import Link from 'next/link';

/**
 * Trang Nạp Diamond cho User thường.
 *
 * Tại sao cần trang riêng?
 * - Spec Phase 1.6: "Nạp tiền tối thiểu 1 kênh (VietQR/Bank)".
 * - Trước đây chỉ có admin panel quản lý deposits, user không có giao diện nạp.
 *
 * Kiến trúc trang:
 * 1. Hiển thị số dư Diamond hiện tại (from walletStore)
 * 2. Các mức nạp preset (50k, 100k, 200k, 500k, 1M, 2M) + input tùy chỉnh
 * 3. Bảng tỷ giá VND → Diamond + khuyến mãi đang hoạt động
 * 4. Nút "Nạp ngay" → gọi createDepositOrder → redirect sang paymentUrl
 *
 * Design: Kế thừa dark glassmorphism từ WalletPage, sử dụng gradient accent
 * indigo-cyan giống Diamond card, thêm micro-animations cho UX premium.
 */

// ======================================================================
// CONFIG: Các mức nạp tiền preset + tỷ giá quy đổi
// Tỷ giá mặc định: 1 Diamond = 1.000 VND (có thể điều chỉnh từ backend)
// ======================================================================
const EXCHANGE_RATE = 1000; // 1 Diamond = 1.000 VND

/** Các mức nạp nhanh với icon tương ứng mức giá */
const PRESET_AMOUNTS = [
    { vnd: 50_000, label: '50K', icon: Zap },
    { vnd: 100_000, label: '100K', icon: Zap },
    { vnd: 200_000, label: '200K', icon: Sparkles },
    { vnd: 500_000, label: '500K', icon: Sparkles },
    { vnd: 1_000_000, label: '1M', icon: Crown },
    { vnd: 2_000_000, label: '2M', icon: Crown },
];

export default function DepositPage() {
    // ======================================================================
    // STATE MANAGEMENT
    // walletStore: lấy số dư hiện tại từ global store (Zustand)
    // selectedAmount: số VND user chọn / nhập
    // customAmount: cho phép user nhập số tiền tùy chỉnh
    // ======================================================================
    const { balance, fetchBalance } = useWalletStore();
    const [selectedAmount, setSelectedAmount] = useState<number>(100_000); // Default 100K VND
    const [customAmount, setCustomAmount] = useState<string>('');
    const [isCustom, setIsCustom] = useState(false);

    const [promotions, setPromotions] = useState<DepositPromotion[]>([]);
    const [isLoadingPromos, setIsLoadingPromos] = useState(true);

    const [isSubmitting, setIsSubmitting] = useState(false);
    const [orderResult, setOrderResult] = useState<CreateDepositOrderResponse | null>(null);
    const [error, setError] = useState<string | null>(null);

    // ======================================================================
    // EFFECTS: Kéo dữ liệu ban đầu
    // ======================================================================
    useEffect(() => {
        fetchBalance();

        /**
         * Lấy danh sách khuyến mãi đang hoạt động.
         * Tại sao dùng onlyActive=true? → Chỉ hiển thị promo đang chạy cho user,
         * admin mới cần thấy promo đã tắt.
         */
        const fetchPromos = async () => {
            setIsLoadingPromos(true);
            const data = await listPromotions(true);
            setPromotions(data ?? []);
            setIsLoadingPromos(false);
        };
        fetchPromos();
    }, [fetchBalance]);

    // ======================================================================
    // COMPUTED VALUES
    // Tính toán số Diamond nhận được + bonus từ khuyến mãi
    // ======================================================================

    /** Số tiền VND thực tế (từ preset hoặc custom input) */
    const actualAmount = isCustom ? (parseInt(customAmount) || 0) : selectedAmount;

    /** Số Diamond cơ bản = VND / tỷ giá */
    const baseDiamond = Math.floor(actualAmount / EXCHANGE_RATE);

    /**
     * Tìm khuyến mãi tốt nhất phù hợp với mức nạp.
     * Logic: lọc tất cả promo có minAmountVnd <= actualAmount,
     * sắp xếp theo bonusDiamond giảm dần → lấy cái đầu tiên.
     *
     * Tại sao chọn mechanism này? → Spec quy định auto-apply promotion
     * tốt nhất cho user, không cần nhập mã coupon.
     */
    const bestPromotion = promotions
        .filter(p => p.isActive && p.minAmountVnd <= actualAmount)
        .sort((a, b) => b.bonusDiamond - a.bonusDiamond)[0];

    /** Tổng Diamond = cơ bản + bonus (nếu có promo) */
    const bonusDiamond = bestPromotion?.bonusDiamond ?? 0;
    const totalDiamond = baseDiamond + bonusDiamond;

    /** Kiểm tra mức nạp hợp lệ (tối thiểu 10.000 VND) */
    const isValid = actualAmount >= 10_000;

    // ======================================================================
    // HANDLERS
    // ======================================================================

    /**
     * Chọn mức nạp preset → tắt chế độ custom, reset error
     */
    const handlePresetSelect = (amount: number) => {
        setSelectedAmount(amount);
        setIsCustom(false);
        setCustomAmount('');
        setError(null);
        setOrderResult(null);
    };

    /**
     * Chuyển sang chế độ nhập số tiền tùy chỉnh
     */
    const handleCustomFocus = () => {
        setIsCustom(true);
        setError(null);
        setOrderResult(null);
    };

    /**
     * Xử lý nạp tiền: gọi API tạo order → redirect sang trang thanh toán.
     *
     * Flow chi tiết:
     * 1. Gọi createDepositOrder(amountVnd) → Backend tạo DepositOrder status=PENDING
     * 2. Backend trả về paymentUrl (VietQR / banking gateway)
     * 3. Frontend redirect tới paymentUrl
     * 4. Sau khi thanh toán, gateway callback webhook → Backend xử lý auto
     */
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

            // Nếu backend trả về paymentUrl → redirect sau 2 giây (cho user thấy thông báo)
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

    // ======================================================================
    // RENDER
    // ======================================================================
    return (
        <div className="min-h-screen bg-slate-950 text-slate-100 p-6 md:p-12 font-sans selection:bg-cyan-500/30">
            <div className="max-w-4xl mx-auto space-y-8">

                {/* ========== HEADER + BACK BUTTON ========== */}
                <div className="space-y-2">
                    <Link
                        href="/wallet"
                        className="inline-flex items-center gap-2 text-slate-400 hover:text-cyan-400 transition-colors text-sm font-medium mb-4"
                    >
                        <ArrowLeft className="w-4 h-4" />
                        Quay lại Ví
                    </Link>

                    <h1 className="text-4xl md:text-5xl font-extrabold tracking-tight bg-gradient-to-r from-cyan-400 via-indigo-400 to-purple-400 text-transparent bg-clip-text">
                        Nạp Diamond 💎
                    </h1>
                    <p className="text-slate-400 text-lg">
                        Chọn mức nạp và thanh toán nhanh chóng qua VietQR / Chuyển khoản ngân hàng.
                    </p>
                </div>

                {/* ========== CURRENT BALANCE MINI CARD ========== */}
                <div className="relative overflow-hidden rounded-2xl bg-gradient-to-r from-indigo-900/50 to-purple-900/30 border border-indigo-500/20 p-6 backdrop-blur-md">
                    <div className="absolute top-0 right-0 -mr-6 -mt-6 w-28 h-28 bg-indigo-500/15 rounded-full blur-3xl"></div>
                    <div className="relative z-10 flex items-center justify-between">
                        <div className="flex items-center gap-4">
                            <div className="p-3 bg-indigo-500/20 rounded-xl ring-1 ring-indigo-500/30">
                                <Gem className="w-6 h-6 text-cyan-300" />
                            </div>
                            <div>
                                <p className="text-sm text-indigo-300 font-medium">Số dư Diamond hiện tại</p>
                                <p className="text-3xl font-black text-white tracking-tight">
                                    {balance?.diamondBalance.toLocaleString() ?? '...'}
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

                {/* ========== CHỌN MỨC NẠP ========== */}
                <div className="space-y-4">
                    <h2 className="text-xl font-bold text-slate-200 flex items-center gap-2">
                        <Zap className="w-5 h-5 text-cyan-400" />
                        Chọn mức nạp
                    </h2>

                    {/* Grid các mức preset */}
                    <div className="grid grid-cols-2 md:grid-cols-3 gap-3">
                        {PRESET_AMOUNTS.map((preset) => {
                            const isSelected = !isCustom && selectedAmount === preset.vnd;
                            const Icon = preset.icon;
                            const diamondForThis = Math.floor(preset.vnd / EXCHANGE_RATE);

                            // Kiểm tra xem preset này có promo không
                            const promoForThis = promotions
                                .filter(p => p.isActive && p.minAmountVnd <= preset.vnd)
                                .sort((a, b) => b.bonusDiamond - a.bonusDiamond)[0];

                            return (
                                <button
                                    key={preset.vnd}
                                    onClick={() => handlePresetSelect(preset.vnd)}
                                    className={`
                                        relative overflow-hidden rounded-2xl p-5 text-left transition-all duration-300 group
                                        border
                                        ${isSelected
                                            ? 'bg-gradient-to-br from-indigo-600/40 to-cyan-600/20 border-cyan-400/50 ring-2 ring-cyan-400/30 shadow-lg shadow-cyan-500/10 scale-[1.02]'
                                            : 'bg-slate-900/60 border-slate-700/50 hover:border-indigo-500/40 hover:bg-slate-800/60'
                                        }
                                    `}
                                >
                                    {/* Badge khuyến mãi nếu có */}
                                    {promoForThis && (
                                        <div className="absolute top-2 right-2 bg-emerald-500/90 text-white text-[10px] font-bold px-2 py-0.5 rounded-full flex items-center gap-1 animate-pulse">
                                            <Gift className="w-3 h-3" />
                                            +{promoForThis.bonusDiamond}
                                        </div>
                                    )}

                                    <div className="flex items-center gap-3 mb-2">
                                        <Icon className={`w-5 h-5 ${isSelected ? 'text-cyan-300' : 'text-slate-500 group-hover:text-indigo-400'} transition-colors`} />
                                        <span className="text-2xl font-black text-white">{preset.label}</span>
                                    </div>
                                    <p className="text-sm text-slate-400">
                                        {preset.vnd.toLocaleString()} VND
                                    </p>
                                    <p className="text-sm font-semibold text-indigo-300 mt-1">
                                        = {diamondForThis.toLocaleString()} 💎
                                    </p>
                                </button>
                            );
                        })}
                    </div>

                    {/* Input tùy chỉnh */}
                    <div
                        className={`
                            rounded-2xl p-5 border transition-all cursor-text
                            ${isCustom
                                ? 'bg-gradient-to-br from-indigo-600/20 to-cyan-600/10 border-cyan-400/40 ring-2 ring-cyan-400/20'
                                : 'bg-slate-900/60 border-slate-700/50 hover:border-indigo-500/30'
                            }
                        `}
                        onClick={handleCustomFocus}
                    >
                        <label className="text-sm font-medium text-slate-400 block mb-2">
                            Hoặc nhập số tiền tùy chỉnh (VND)
                        </label>
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
                            placeholder="Ví dụ: 300000"
                            min={10000}
                            step={10000}
                            className="bg-transparent border-none outline-none text-3xl font-black text-white w-full placeholder:text-slate-600 [appearance:textfield] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none"
                        />
                        {isCustom && actualAmount > 0 && (
                            <p className="text-sm font-medium text-indigo-300 mt-2">
                                = {baseDiamond.toLocaleString()} 💎
                                {bonusDiamond > 0 && (
                                    <span className="text-emerald-400 ml-2">+ {bonusDiamond} bonus 🎁</span>
                                )}
                            </p>
                        )}
                    </div>
                </div>

                {/* ========== KHUYẾN MÃI ĐANG CÓ ========== */}
                {!isLoadingPromos && promotions.length > 0 && (
                    <div className="space-y-3">
                        <h2 className="text-lg font-bold text-slate-200 flex items-center gap-2">
                            <Gift className="w-5 h-5 text-emerald-400" />
                            Khuyến mãi đang hoạt động
                        </h2>
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                            {promotions.map((promo) => (
                                <div
                                    key={promo.id}
                                    className={`
                                        rounded-xl p-4 border transition-all
                                        ${bestPromotion?.id === promo.id && isValid
                                            ? 'bg-emerald-900/30 border-emerald-500/40 ring-1 ring-emerald-500/20'
                                            : 'bg-slate-900/40 border-slate-700/30'
                                        }
                                    `}
                                >
                                    <div className="flex items-center justify-between">
                                        <div>
                                            <p className="text-sm text-slate-400">
                                                Nạp từ <strong className="text-white">{promo.minAmountVnd.toLocaleString()} VND</strong>
                                            </p>
                                            <p className="text-lg font-bold text-emerald-400 mt-1">
                                                +{promo.bonusDiamond} Diamond 🎁
                                            </p>
                                        </div>
                                        {bestPromotion?.id === promo.id && isValid && (
                                            <CheckCircle2 className="w-5 h-5 text-emerald-400" />
                                        )}
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                )}

                {/* ========== TỔNG HỢP ĐƠN HÀNG + NÚT NẠP ========== */}
                <div className="rounded-3xl bg-gradient-to-br from-slate-900/80 to-slate-800/50 border border-slate-700/50 p-8 shadow-2xl backdrop-blur-md space-y-6">
                    <h2 className="text-xl font-bold text-slate-200">Xác nhận đơn nạp</h2>

                    <div className="space-y-3">
                        <div className="flex justify-between items-center py-2 border-b border-slate-700/50">
                            <span className="text-slate-400">Số tiền nạp</span>
                            <span className="font-bold text-white text-lg">
                                {actualAmount.toLocaleString()} VND
                            </span>
                        </div>

                        <div className="flex justify-between items-center py-2 border-b border-slate-700/50">
                            <span className="text-slate-400">Diamond nhận được</span>
                            <span className="font-bold text-indigo-300 text-lg">
                                {baseDiamond.toLocaleString()} 💎
                            </span>
                        </div>

                        {bonusDiamond > 0 && (
                            <div className="flex justify-between items-center py-2 border-b border-slate-700/50">
                                <span className="text-emerald-400 flex items-center gap-2">
                                    <Gift className="w-4 h-4" />
                                    Bonus khuyến mãi
                                </span>
                                <span className="font-bold text-emerald-400 text-lg">
                                    +{bonusDiamond.toLocaleString()} 💎
                                </span>
                            </div>
                        )}

                        <div className="flex justify-between items-center py-3">
                            <span className="text-white font-semibold text-lg">TỔNG CỘNG NHẬN</span>
                            <span className="font-black text-transparent bg-clip-text bg-gradient-to-r from-cyan-400 to-indigo-400 text-3xl">
                                {totalDiamond.toLocaleString()} 💎
                            </span>
                        </div>
                    </div>

                    {/* Error message */}
                    {error && (
                        <div className="flex items-center gap-3 p-4 rounded-xl bg-rose-900/30 border border-rose-500/30 text-rose-300">
                            <AlertCircle className="w-5 h-5 flex-shrink-0" />
                            <p className="text-sm">{error}</p>
                        </div>
                    )}

                    {/* Success message */}
                    {orderResult && (
                        <div className="flex items-start gap-3 p-4 rounded-xl bg-emerald-900/30 border border-emerald-500/30 text-emerald-300">
                            <CheckCircle2 className="w-5 h-5 flex-shrink-0 mt-0.5" />
                            <div className="text-sm space-y-1">
                                <p className="font-semibold">Đơn nạp đã được tạo thành công!</p>
                                <p className="text-emerald-400/80">
                                    Mã đơn: <code className="bg-emerald-800/40 px-1.5 py-0.5 rounded text-xs">{orderResult.orderId}</code>
                                </p>
                                {orderResult.paymentUrl && (
                                    <a
                                        href={orderResult.paymentUrl}
                                        target="_blank"
                                        rel="noopener noreferrer"
                                        className="inline-flex items-center gap-1.5 text-emerald-300 hover:text-white underline underline-offset-4 mt-1"
                                    >
                                        Mở trang thanh toán <ExternalLink className="w-3.5 h-3.5" />
                                    </a>
                                )}
                            </div>
                        </div>
                    )}

                    {/* Nút nạp chính */}
                    <button
                        onClick={handleDeposit}
                        disabled={!isValid || isSubmitting}
                        className={`
                            w-full py-4 rounded-2xl font-bold text-lg transition-all duration-300
                            flex items-center justify-center gap-3
                            ${isValid && !isSubmitting
                                ? 'bg-gradient-to-r from-indigo-600 to-cyan-600 hover:from-indigo-500 hover:to-cyan-500 text-white shadow-lg shadow-indigo-500/25 hover:shadow-indigo-500/40 hover:scale-[1.01] active:scale-[0.99]'
                                : 'bg-slate-800 text-slate-500 cursor-not-allowed'
                            }
                        `}
                    >
                        {isSubmitting ? (
                            <>
                                <Loader2 className="w-5 h-5 animate-spin" />
                                Đang xử lý...
                            </>
                        ) : (
                            <>
                                <Gem className="w-5 h-5" />
                                Nạp {totalDiamond.toLocaleString()} Diamond ngay
                            </>
                        )}
                    </button>

                    <p className="text-xs text-slate-500 text-center">
                        Bạn sẽ được chuyển đến trang thanh toán VietQR / Ngân hàng sau khi xác nhận.
                    </p>
                </div>

            </div>
        </div>
    );
}
