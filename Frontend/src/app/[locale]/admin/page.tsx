'use client';

import React, { useEffect, useState } from 'react';
import { listUsers, listDeposits } from '@/actions/adminActions';
import { listPromotions } from '@/actions/promotionActions';
import { getAllHistorySessionsAdminAction } from '@/actions/historyActions';
import { 
    Users, 
    CreditCard, 
    Gift, 
    History, 
    TrendingUp, 
    ShieldCheck, 
    Activity,
    ArrowUpRight,
    Zap,
    Gem,
    Coins,
    ChevronRight,
    Loader2
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';

/**
 * Trang Dashboard Quản trị (Admin Dashboard)
 * 
 * Các tính năng:
 * 1. Quick Stats: Thống kê số lượng thực tế từ các API Admin.
 * 2. Visual Insight: Thẻ phân tích dạng Glassmorphism.
 * 3. Quick Actions: Lối tắt đến các phần quản lý quan trọng.
 * 4. Astral Design: Đồng bộ phong cách mờ kính và hiệu ứng chiều sâu.
 */

export default function AdminDashboardPage() {
    const router = useRouter();
    const [stats, setStats] = useState({
        users: 0,
        deposits: 0,
        promotions: 0,
        readings: 0
    });
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchStats = async () => {
            setLoading(true);
            try {
                // Fetch đồng thời các dữ liệu count để tối ưu tốc độ
                const [usersRes, depositsRes, promosRes, readingsRes] = await Promise.all([
                    listUsers(1, 1),
                    listDeposits(1, 1),
                    listPromotions(false),
                    getAllHistorySessionsAdminAction({ page: 1, pageSize: 1 })
                ]);

                setStats({
                    users: usersRes?.totalCount ?? 0,
                    deposits: depositsRes?.totalCount ?? 0,
                    promotions: promosRes?.length ?? 0,
                    readings: (readingsRes as any).success ? (readingsRes as any).data.totalCount : 0
                });
            } catch (err) {
                console.error("Dashboard stats fetch failed:", err);
            } finally {
                setLoading(false);
            }
        };

        fetchStats();
    }, []);

    const statCards = [
        { 
            name: "Tổng Người Dùng", 
            value: stats.users, 
            icon: Users, 
            color: "text-blue-400", 
            bg: "bg-blue-500/10",
            href: "/admin/users" 
        },
        { 
            name: "Lượt Nạp Tiền", 
            value: stats.deposits, 
            icon: CreditCard, 
            color: "text-emerald-400", 
            bg: "bg-emerald-500/10",
            href: "/admin/deposits" 
        },
        { 
            name: "Khuyến Mãi", 
            value: stats.promotions, 
            icon: Gift, 
            color: "text-amber-400", 
            bg: "bg-amber-500/10",
            href: "/admin/promotions" 
        },
        { 
            name: "Phiên Xem Bài", 
            value: stats.readings, 
            icon: History, 
            color: "text-purple-400", 
            bg: "bg-purple-500/10",
            href: "/admin/readings" 
        },
    ];

    if (loading) {
        return (
            <div className="h-[60vh] flex flex-col items-center justify-center space-y-4">
                <Loader2 className="w-10 h-10 text-purple-500 animate-spin" />
                <span className="text-[10px] font-black uppercase tracking-[0.3em] text-zinc-600">Khởi tạo Hệ thống Quản trị...</span>
            </div>
        );
    }

    return (
        <div className="space-y-12 animate-in fade-in slide-in-from-bottom-8 duration-1000">
            {/* Header Section */}
            <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
                <div className="space-y-4">
                    <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-purple-500/5 border border-purple-500/10 text-[9px] uppercase tracking-[0.2em] font-black text-purple-400 shadow-xl backdrop-blur-md text-left">
                        <TrendingUp className="w-3 h-3 text-emerald-500" />
                        System Intelligence
                    </div>
                    <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white uppercase italic text-left">
                        Cổng Tổng Quan HQ
                    </h1>
                    <p className="text-zinc-500 font-medium max-w-lg text-sm leading-relaxed text-left">
                        Theo dõi dòng chảy linh hồn và dữ liệu vận mệnh trên toàn hệ thống TarotNow AI.
                    </p>
                </div>

                <div className="flex items-center gap-4">
                    <div className="px-6 py-3 rounded-2xl bg-white/[0.03] border border-white/10 backdrop-blur-xl">
                        <div className="text-[8px] font-black uppercase tracking-widest text-zinc-600 text-left">Health Check</div>
                        <div className="flex items-center gap-2 text-xs font-bold text-emerald-400">
                            <Activity className="w-3 h-3 animate-pulse" />
                            Hệ thống ổn định
                        </div>
                    </div>
                </div>
            </div>

            {/* Quick Stats Grid */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                {statCards.map((stat, i) => (
                    <button
                        key={stat.name}
                        onClick={() => router.push(stat.href as any)}
                        className="group relative h-44 bg-white/[0.02] hover:bg-white/[0.04] backdrop-blur-3xl rounded-[2.5rem] border border-white/5 hover:border-white/10 p-8 text-left transition-all duration-500 shadow-2xl overflow-hidden"
                    >
                        <div className="absolute top-0 right-0 p-6 opacity-[0.05] group-hover:opacity-[0.08] transition-all duration-500 group-hover:scale-110 group-hover:rotate-12 translate-x-4 -translate-y-4">
                            <stat.icon size={120} />
                        </div>

                        <div className="relative z-10 h-full flex flex-col justify-between">
                            <div className={`w-10 h-10 rounded-xl ${stat.bg} border border-white/5 flex items-center justify-center transition-transform group-hover:scale-110`}>
                                <stat.icon className={`w-5 h-5 ${stat.color}`} />
                            </div>
                            
                            <div>
                                <div className="text-4xl font-black text-white italic tracking-tighter mb-1">
                                    {stat.value.toLocaleString()}
                                </div>
                                <div className="text-[10px] font-black uppercase tracking-widest text-zinc-500">
                                    {stat.name}
                                </div>
                            </div>
                        </div>
                    </button>
                ))}
            </div>

            {/* Mid Section: Insight & Actions */}
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-10">
                {/* System Notice - Gradient Card */}
                <div className="lg:col-span-2 relative group overflow-hidden bg-gradient-to-br from-purple-500/10 to-transparent backdrop-blur-3xl rounded-[3rem] border border-purple-500/20 p-10 shadow-2xl">
                    <div className="absolute top-0 right-0 p-10 opacity-10 pointer-events-none group-hover:scale-110 transition-transform duration-1000">
                        <Zap size={200} className="text-purple-400" />
                    </div>

                    <div className="relative z-10 space-y-6">
                        <div className="flex items-center gap-3">
                            <div className="w-8 h-8 rounded-lg bg-purple-500/20 flex items-center justify-center border border-purple-500/20">
                                <ShieldCheck className="w-5 h-5 text-purple-400" />
                            </div>
                            <h2 className="text-xl font-black text-white uppercase italic tracking-tighter">Bản tin Quản trị viên</h2>
                        </div>
                        
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                            <div className="p-6 rounded-2xl bg-black/40 border border-white/5 space-y-2">
                                <div className="text-[10px] font-black text-emerald-400 uppercase tracking-widest">Lời nhắc</div>
                                <p className="text-xs text-zinc-400 leading-relaxed text-left">Kiểm tra các đơn nạp tiền đang chờ và xác nhận thủ công nếu ví MOMO/VNPay bị trễ lệnh.</p>
                            </div>
                            <div className="p-6 rounded-2xl bg-black/40 border border-white/5 space-y-2">
                                <div className="text-[10px] font-black text-amber-500 uppercase tracking-widest">Thông báo</div>
                                <p className="text-xs text-zinc-400 leading-relaxed text-left">Đã cập nhật hệ thống Giao diện Admin Astral v2.0. Đảm bảo trải nghiệm quản lý tuyệt nhất.</p>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Quick Shortcuts */}
                <div className="space-y-6 text-left">
                    <h2 className="text-lg font-black text-white uppercase italic tracking-tighter flex items-center gap-3">
                        <Zap className="w-4 h-4 text-emerald-500" />
                        Lối tắt Nhanh
                    </h2>
                    
                    <div className="space-y-4">
                        <button 
                            onClick={() => router.push('/admin/users')}
                            className="w-full flex items-center justify-between p-5 rounded-2xl bg-white/[0.02] border border-white/5 hover:bg-white/[0.05] hover:border-purple-500/30 transition-all group"
                        >
                            <div className="flex items-center gap-4">
                                <Users className="w-5 h-5 text-zinc-600 group-hover:text-purple-400" />
                                <span className="text-[10px] font-black uppercase tracking-widest text-zinc-300">Quản lý User</span>
                            </div>
                            <ChevronRight className="w-4 h-4 text-zinc-800" />
                        </button>
                        
                        <button 
                            onClick={() => router.push('/admin/deposits')}
                            className="w-full flex items-center justify-between p-5 rounded-2xl bg-white/[0.02] border border-white/5 hover:bg-white/[0.05] hover:border-emerald-500/30 transition-all group"
                        >
                            <div className="flex items-center gap-4">
                                <CreditCard className="w-5 h-5 text-zinc-600 group-hover:text-emerald-400" />
                                <span className="text-[10px] font-black uppercase tracking-widest text-zinc-300">Dòng tiền Nạp</span>
                            </div>
                            <ChevronRight className="w-4 h-4 text-zinc-800" />
                        </button>

                        <button 
                            onClick={() => router.push('/admin/readings')}
                            className="w-full flex items-center justify-between p-5 rounded-2xl bg-white/[0.02] border border-white/5 hover:bg-white/[0.05] hover:border-amber-500/30 transition-all group"
                        >
                            <div className="flex items-center gap-4">
                                <History className="w-5 h-5 text-zinc-600 group-hover:text-amber-500" />
                                <span className="text-[10px] font-black uppercase tracking-widest text-zinc-300">Xem bài Mới</span>
                            </div>
                            <ChevronRight className="w-4 h-4 text-zinc-800" />
                        </button>
                    </div>
                </div>
            </div>
            
            {/* Revenue & Activity Insight */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-10">
                <div className="p-10 bg-white/[0.01] backdrop-blur-3xl rounded-[3rem] border border-white/5 relative overflow-hidden group">
                    <div className="absolute top-0 right-0 p-8 opacity-5 group-hover:scale-110 transition-transform">
                        <Gem size={120} className="text-purple-500" />
                    </div>
                    <div className="relative z-10 space-y-6">
                        <div className="flex items-center gap-3">
                            <div className="w-10 h-10 rounded-xl bg-purple-500/10 border border-purple-500/20 flex items-center justify-center">
                                <TrendingUp className="w-5 h-5 text-purple-400" />
                            </div>
                            <h3 className="text-xl font-black text-white uppercase italic tracking-tighter">Doanh Thu Ước Tính</h3>
                        </div>
                        
                        <div className="flex items-end gap-4">
                            <div className="text-5xl font-black text-white italic tracking-tighter drop-shadow-[0_0_15px_rgba(168,85,247,0.3)]">
                                {(stats.deposits * 50000).toLocaleString()} <span className="text-xl text-purple-500">VND</span>
                            </div>
                        </div>
                        <p className="text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">
                            Tính toán dựa trên {stats.deposits} lệnh nạp thành công
                        </p>
                    </div>
                </div>

                <div className="p-10 bg-white/[0.01] backdrop-blur-3xl rounded-[3rem] border border-white/5 relative overflow-hidden group">
                    <div className="absolute top-0 right-0 p-8 opacity-5 group-hover:scale-110 transition-transform">
                        <Activity size={120} className="text-emerald-500" />
                    </div>
                    <div className="relative z-10 space-y-6">
                        <div className="flex items-center gap-3">
                            <div className="w-10 h-10 rounded-xl bg-emerald-500/10 border border-emerald-500/20 flex items-center justify-center">
                                <Zap className="w-5 h-5 text-emerald-400" />
                            </div>
                            <h3 className="text-xl font-black text-white uppercase italic tracking-tighter">Luồng Hoạt Động</h3>
                        </div>
                        
                        <div className="flex items-end gap-4">
                            <div className="text-5xl font-black text-white italic tracking-tighter drop-shadow-[0_0_15px_rgba(16,185,129,0.3)]">
                                {stats.readings} <span className="text-xl text-emerald-500">Mệnh Đề</span>
                            </div>
                        </div>
                        <p className="text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">
                            Tổng số phiên giải mã vận mệnh đã thực hiện
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
}
