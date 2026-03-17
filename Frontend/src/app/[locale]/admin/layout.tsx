'use client';

import { ReactNode, use } from "react";
import Link from "next/link";
import { usePathname } from "next/navigation";
import {
 Users,
 CreditCard,
 Gift,
 History,
 LayoutDashboard,
 ShieldCheck,
 ChevronRight,
 LogOut,
 ScrollText,
 Banknote,
 Scale
} from "lucide-react";
import AstralBackground from "@/components/layout/AstralBackground";

interface AdminLayoutProps {
 children: ReactNode;
 params: Promise<{ locale: string }>;
}

export default function AdminLayout({ children, params }: AdminLayoutProps) {
 const { locale } = use(params);
 const pathname = usePathname();

 const menuItems = [
 { name: "Tổng Quan", href: `/${locale}/admin`, icon: LayoutDashboard },
 { name: "Người Dùng", href: `/${locale}/admin/users`, icon: Users },
 { name: "Giao Dịch", href: `/${locale}/admin/deposits`, icon: CreditCard },
 { name: "Khuyến Mãi", href: `/${locale}/admin/promotions`, icon: Gift },
 { name: "Xem Bài", href: `/${locale}/admin/readings`, icon: History },
 { name: "Đơn Xin Reader", href: `/${locale}/admin/reader-requests`, icon: ScrollText },
 { name: "Rút Tiền", href: `/${locale}/admin/withdrawals`, icon: Banknote },
 { name: "Tranh Chấp", href: `/${locale}/admin/disputes`, icon: Scale },
 ];

 const isActive = (href: string) => {
 if (href === `/${locale}/admin`) {
 return pathname === href;
 }
 return pathname.startsWith(href);
 };

 return (
 <div className="flex h-screen bg-[var(--bg-void)] text-[var(--text-primary)] overflow-hidden">
 {/* ##### PREMIUM BACKGROUND SYSTEM ##### */}
 {/* Sử dụng component AstralBackground chung thay vì copy-paste */}
 <AstralBackground variant="subtle" />

 {/* Sidebar — Dark Surface Navigation */}
 <aside className="relative z-20 w-72 h-full bg-[var(--bg-glass)] border-r border-[var(--border-subtle)] flex flex-col shadow-2xl">
 {/* Header Logo Admin */}
 <div className="p-8 mb-4">
 <div className="flex items-center gap-3 group px-4 py-3 rounded-2xl bg-[var(--bg-elevated)] border border-[var(--border-default)] shadow-[var(--shadow-card)] overflow-hidden relative">
 <div className="absolute inset-0 bg-gradient-to-r from-[var(--purple-accent)]/10 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-500" />
 <div className="w-10 h-10 rounded-xl bg-[var(--purple-accent)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20 group-hover:scale-110 transition-transform duration-500">
 <ShieldCheck className="w-6 h-6 text-[var(--purple-accent)]" />
 </div>
 <div className="relative z-10">
 <h2 className="text-sm font-black text-[var(--text-ink)] tracking-widest uppercase italic">Admin</h2>
 <div className="text-[10px] font-bold text-[var(--purple-muted)] tracking-tighter uppercase leading-none">Management</div>
 </div>
 </div>
 </div>

 {/* Navigation Links */}
 <nav className="flex-1 px-4 space-y-2 overflow-y-auto no-scrollbar">
 <div className="px-6 mb-4">
 <span className="text-[9px] font-black uppercase tracking-[0.3em] text-[var(--text-muted)]">Main Control</span>
 </div>

 {menuItems.map((item) => {
 const active = isActive(item.href);
 const Icon = item.icon;

 return (
 <Link
 key={item.href}
 href={item.href}
 className={[
 "group flex items-center justify-between px-6 py-4 rounded-2xl transition-all duration-300 relative overflow-hidden",
 active
 ? "bg-[var(--bg-elevated)] text-[var(--text-ink)] shadow-[var(--glow-purple-sm)] border border-[var(--border-hover)]"
 : "hover:bg-[var(--bg-surface-hover)] hover:text-[var(--text-ink)] text-[var(--text-secondary)] border border-transparent"
 ].join(" ")}
 >
 <div className="flex items-center gap-4 relative z-10 font-bold">
 <Icon
 className={[
 "w-5 h-5 transition-all duration-300",
 active ? "text-[var(--purple-accent)] scale-110" : "group-hover:text-[var(--text-ink)]"
 ].join(" ")}
 />
 <span
 className={[
 "text-[11px] uppercase tracking-widest",
 active ? "font-black" : "font-bold"
 ].join(" ")}
 >
 {item.name}
 </span>
 </div>

 {active && (
 <div className="relative z-10">
 <ChevronRight className="w-4 h-4 text-[var(--purple-muted)]" />
 </div>
 )}

 {/* Hover Indicator */}
 {active && (
 <div className="absolute left-0 top-1/4 bottom-1/4 w-1 bg-[var(--purple-accent)] rounded-r-full shadow-[0_0_10px_var(--purple-accent)]" />
 )}
 </Link>
 );
 })}
 </nav>

 {/* Footer Exit Portal */}
 <div className="p-8 border-t border-[var(--border-subtle)]">
 <Link
 href={`/${locale}/`}
 className="flex items-center justify-center gap-3 px-6 py-4 rounded-2xl bg-[var(--bg-elevated)] border border-[var(--border-subtle)] hover:bg-[var(--danger)]/10 hover:border-[var(--danger)]/20 hover:text-[var(--danger)] transition-all group"
 >
 <LogOut className="w-5 h-5 text-[var(--text-secondary)] group-hover:text-[var(--danger)] transition-colors" />
 <span className="text-[10px] font-black uppercase tracking-widest">Thoát Portal</span>
 </Link>
 </div>
 </aside>

 {/* Main Content */}
 <main className="relative z-10 flex-1 overflow-y-auto custom-scrollbar">
 <div className="min-h-full p-8 md:p-12 animate-in fade-in duration-700">
 {children}
 </div>
 </main>
 </div>
 );
}
