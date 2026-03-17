'use client';

import { ReactNode, use, useState, useEffect } from "react";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { 
    Users, 
    CreditCard, 
    Gift, 
    History, 
    LayoutDashboard, 
    Settings, 
    ShieldCheck,
    Sparkles,
    ChevronRight,
    LogOut
} from "lucide-react";

/**
 * Layout cho khu vực Admin - Astral Premium Redesign
 *
 * Các cải tiến:
 * 1. Glassmorphism Sidebar: Thanh điều hướng mờ kính với hiệu ứng Glass-blur.
 * 2. Astral Background: Tích hợp Nebula và Spiritual Particles cho toàn bộ khu vực admin.
 * 3. Active States: Hiệu ứng chọn mục rực rỡ với gradient và glow.
 * 4. Next.js 16 Support: Xử lý params dưới dạng Promise bằng React use().
 */

interface AdminLayoutProps {
    children: ReactNode;
    params: Promise<{ locale: string }>;
}

export default function AdminLayout({ children, params }: AdminLayoutProps) {
    const { locale } = use(params);
    const pathname = usePathname();
    const [mounted, setMounted] = useState(false);

    /**
     * Tại sao cần useEffect/mounted?
     * - Các hạt "Spiritual Particles" sử dụng Math.random() để tạo vị trí ngẫu nhiên.
     * - Nếu render ngay lập tức, Server và Client sẽ sinh ra số khác nhau -> lỗi Hydration mismatch.
     * - Giải pháp: Chỉ render các thành phần ngẫu nhiên sau khi component đã "mount" hoàn toàn ở Client.
     */
    useEffect(() => {
        setMounted(true);
    }, []);

    const menuItems = [
        { name: "Tổng Quan", href: `/${locale}/admin`, icon: LayoutDashboard },
        { name: "Người Dùng", href: `/${locale}/admin/users`, icon: Users },
        { name: "Giao Dịch", href: `/${locale}/admin/deposits`, icon: CreditCard },
        { name: "Khuyến Mãi", href: `/${locale}/admin/promotions`, icon: Gift },
        { name: "Xem Bài", href: `/${locale}/admin/readings`, icon: History },
    ];

    const isActive = (href: string) => {
        if (href === `/${locale}/admin`) {
            return pathname === href;
        }
        return pathname.startsWith(href);
    };

    return (
        <div className="flex h-screen bg-[#020108] text-zinc-300 font-sans overflow-hidden">
            {/* ##### PREMIUM BACKGROUND SYSTEM ##### */}
            <div className="fixed inset-0 z-0 pointer-events-none">
                <div className="absolute inset-0 bg-[url('https://grainy-gradients.vercel.app/noise.svg')] opacity-[0.03] mix-blend-overlay"></div>
                <div className="absolute top-0 -left-1/4 w-[70vw] h-[70vw] bg-purple-900/[0.05] blur-[120px] rounded-full" />
                <div className="absolute bottom-0 -right-1/4 w-[60vw] h-[60vw] bg-indigo-900/[0.05] blur-[130px] rounded-full" />
                
                {/* Spiritual Particles - Chỉ render ở Client để tránh Hydration Error */}
                {mounted && (
                    <div className="absolute inset-0">
                        {Array.from({ length: 15 }).map((_, i) => (
                            <div
                                key={i}
                                className="absolute w-[1px] h-[1px] bg-white rounded-full animate-float opacity-[0.1]"
                                style={{
                                    top: `${Math.random() * 100}%`,
                                    left: `${Math.random() * 100}%`,
                                    animationDuration: `${30 + Math.random() * 40}s`,
                                    animationDelay: `${-Math.random() * 20}s`,
                                }}
                            />
                        ))}
                    </div>
                )}
            </div>

            {/* Sidebar — Glassmorphism Side Navigation */}
            <aside className="relative z-20 w-72 h-full bg-white/[0.01] backdrop-blur-2xl border-r border-white/5 flex flex-col shadow-2xl">
                <div className="p-8 mb-4">
                    <div className="flex items-center gap-3 group px-4 py-3 rounded-2xl bg-white/[0.03] border border-white/10 shadow-xl overflow-hidden relative">
                        <div className="absolute inset-0 bg-gradient-to-r from-purple-500/10 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-500" />
                        <div className="w-10 h-10 rounded-xl bg-purple-500/10 flex items-center justify-center border border-purple-500/20 group-hover:scale-110 transition-transform duration-500">
                            <ShieldCheck className="w-6 h-6 text-purple-400" />
                        </div>
                        <div className="relative z-10">
                            <h2 className="text-sm font-black text-white tracking-widest uppercase italic">Admin</h2>
                            <div className="text-[10px] font-bold text-purple-400/70 tracking-tighter uppercase leading-none">Management</div>
                        </div>
                    </div>
                </div>

                <nav className="flex-1 px-4 space-y-2 overflow-y-auto custom-scrollbar">
                    <div className="px-6 mb-4">
                        <span className="text-[9px] font-black uppercase tracking-[0.3em] text-zinc-600">Main Control</span>
                    </div>
                    {menuItems.map((item) => {
                        const active = isActive(item.href);
                        const Icon = item.icon;
                        return (
                            <Link 
                                key={item.href}
                                href={item.href} 
                                className={`
                                    group flex items-center justify-between px-6 py-4 rounded-2xl transition-all duration-500 relative overflow-hidden
                                    ${active 
                                        ? "bg-purple-500/10 text-white shadow-[0_0_20px_rgba(168,85,247,0.1)] border border-purple-500/20" 
                                        : "hover:bg-white/[0.03] hover:text-white border border-transparent"
                                    }
                                `}
                            >
                                <div className="flex items-center gap-4 relative z-10 font-bold">
                                    <Icon className={`w-5 h-5 transition-all duration-500 ${active ? "text-purple-400 scale-110" : "text-zinc-500 group-hover:text-zinc-300"}`} />
                                    <span className={`text-[11px] uppercase tracking-widest ${active ? "font-black" : "font-bold"}`}>{item.name}</span>
                                </div>
                                {active && (
                                    <div className="relative z-10">
                                        <ChevronRight className="w-4 h-4 text-purple-400/50" />
                                    </div>
                                )}
                                
                                {/* Hover Indicator */}
                                {active && (
                                    <div className="absolute left-0 top-1/4 bottom-1/4 w-1 bg-purple-500 rounded-r-full shadow-[0_0_10px_#a855f7]" />
                                )}
                            </Link>
                        );
                    })}
                </nav>

                <div className="p-8 border-t border-white/5">
                    <Link 
                        href={`/${locale}/profile`}
                        className="flex items-center gap-3 px-6 py-4 rounded-2xl bg-white/[0.02] border border-white/5 hover:bg-rose-500/10 hover:border-rose-500/20 hover:text-rose-400 transition-all group"
                    >
                        <LogOut className="w-5 h-5 text-zinc-600 group-hover:text-rose-400 transition-colors" />
                        <span className="text-[10px] font-black uppercase tracking-widest">Thoát Portal</span>
                    </Link>
                </div>
            </aside>

            {/* Main Content — Khu vực hiển thị nội dung chính */}
            <main className="relative z-10 flex-1 overflow-y-auto">
                <div className="min-h-full p-8 md:p-12 animate-in fade-in duration-700">
                    {children}
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
                .custom-scrollbar::-webkit-scrollbar {
                    width: 4px;
                }
                .custom-scrollbar::-webkit-scrollbar-track {
                    background: transparent;
                }
                .custom-scrollbar::-webkit-scrollbar-thumb {
                    background: rgba(168, 85, 247, 0.1);
                    border-radius: 10px;
                }
                .custom-scrollbar::-webkit-scrollbar-thumb:hover {
                    background: rgba(168, 85, 247, 0.3);
                }
            `}} />
        </div>
    );
}

