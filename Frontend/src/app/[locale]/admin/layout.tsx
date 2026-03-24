/*
 * ===================================================================
 * FILE: layout.tsx (Admin Layout)
 * BỐI CẢNH (CONTEXT):
 *   Bộ khung giao diện tĩnh bọc ngoài tất cả các trang Admin.
 *   Chứa thanh điều hướng (Sidebar Nav) dùng chung.
 *
 * UI/UX:
 *   Áp dụng giao diện Glassmorphism với Background Astral chung cho toàn app.
 *   Responsive hỗ trợ Sidebar trên Desktop và Sidebar trượt (Drawer) trên Mobile.
 * ===================================================================
 */
'use client';

import { ReactNode, useEffect, useState } from "react";
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
    Scale,
    Menu,
    X,
} from "lucide-react";
import AstralBackground from "@/shared/components/layout/AstralBackground";
import { Link, usePathname } from "@/i18n/routing";
import { useTranslations } from "next-intl";

interface AdminLayoutProps {
    children: ReactNode;
}

export default function AdminLayout({ children }: AdminLayoutProps) {
    const t = useTranslations("Admin");
    const pathname = usePathname();
    const [mobileNavOpen, setMobileNavOpen] = useState(false);

    const menuItems = [
        { name: t("layout.menu.overview"), href: `/admin`, icon: LayoutDashboard },
        { name: t("layout.menu.users"), href: `/admin/users`, icon: Users },
        { name: t("layout.menu.deposits"), href: `/admin/deposits`, icon: CreditCard },
        { name: t("layout.menu.promotions"), href: `/admin/promotions`, icon: Gift },
        { name: t("layout.menu.readings"), href: `/admin/readings`, icon: History },
        { name: t("layout.menu.reader_requests"), href: `/admin/reader-requests`, icon: ScrollText },
        { name: t("layout.menu.withdrawals"), href: `/admin/withdrawals`, icon: Banknote },
        { name: t("layout.menu.disputes"), href: `/admin/disputes`, icon: Scale },
    ];

    const isActive = (href: string) => {
        if (href === `/admin`) {
            return pathname === href;
        }
        return pathname.startsWith(href);
    };

    useEffect(() => {
        if (!mobileNavOpen) return undefined;
        const previousOverflow = document.body.style.overflow;
        document.body.style.overflow = "hidden";
        return () => {
            document.body.style.overflow = previousOverflow;
        };
    }, [mobileNavOpen]);

    const renderSidebarContent = (mobile = false) => (
        <>
            {/* Header Logo Admin */}
            <div className="p-6 sm:p-8 mb-2 sm:mb-4">
                <div className="flex items-center gap-3 group px-4 py-3 rounded-2xl bg-[var(--bg-elevated)] border border-[var(--border-default)] shadow-[var(--shadow-card)] overflow-hidden relative">
                    <div className="absolute inset-0 bg-gradient-to-r from-[var(--purple-accent)]/10 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-500" />
                    <div className="w-10 h-10 rounded-xl bg-[var(--purple-accent)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20 group-hover:scale-110 transition-transform duration-500">
                        <ShieldCheck className="w-6 h-6 text-[var(--purple-accent)]" />
                    </div>
                    <div className="relative z-10">
                        <h2 className="text-sm font-black text-[var(--text-ink)] tracking-widest uppercase italic">{t("layout.title")}</h2>
                        <div className="text-[10px] font-bold text-[var(--purple-muted)] tracking-tighter uppercase leading-none">{t("layout.subtitle")}</div>
                    </div>
                </div>
            </div>

            {/* Navigation Links */}
            <nav className="flex-1 px-4 space-y-2 overflow-y-auto no-scrollbar">
                <div className="px-6 mb-4">
                    <span className="text-[9px] font-black uppercase tracking-[0.3em] text-[var(--text-muted)]">{t("layout.section_main")}</span>
                </div>

                {menuItems.map((item) => {
                    const active = isActive(item.href);
                    const Icon = item.icon;

                    return (
                        <Link
                            key={item.href}
                            href={item.href}
                            onClick={() => {
                                if (mobile) setMobileNavOpen(false);
                            }}
                            className={[
                                "group flex items-center justify-between px-5 sm:px-6 py-4 rounded-2xl transition-all duration-300 relative overflow-hidden",
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
            <div className="p-6 sm:p-8 border-t border-[var(--border-subtle)]">
                <Link
                    href={`/`}
                    onClick={() => {
                        if (mobile) setMobileNavOpen(false);
                    }}
                    className="flex items-center justify-center gap-3 px-6 py-4 rounded-2xl bg-[var(--bg-elevated)] border border-[var(--border-subtle)] hover:bg-[var(--danger)]/10 hover:border-[var(--danger)]/20 hover:text-[var(--danger)] transition-all group"
                >
                    <LogOut className="w-5 h-5 text-[var(--text-secondary)] group-hover:text-[var(--danger)] transition-colors" />
                    <span className="text-[10px] font-black uppercase tracking-widest">{t("layout.exit_portal")}</span>
                </Link>
            </div>
        </>
    );

    return (
        <div className="flex h-dvh bg-[var(--bg-void)] text-[var(--text-primary)] overflow-hidden">
            {/* ##### PREMIUM BACKGROUND SYSTEM ##### */}
            {/* Sử dụng component AstralBackground chung thay vì copy-paste */}
            <AstralBackground variant="subtle" />

            {/* Sidebar — Desktop */}
            <aside className="relative z-20 hidden lg:flex w-72 h-full bg-[var(--bg-glass)] border-r border-[var(--border-subtle)] flex-col shadow-2xl">
                {renderSidebarContent()}
            </aside>

            {/* Sidebar — Mobile Drawer */}
            {mobileNavOpen && (
                <div className="lg:hidden fixed inset-0 z-40">
                    <button
                        type="button"
                        className="absolute inset-0 bg-black/45"
                        onClick={() => setMobileNavOpen(false)}
                        aria-label="Close menu"
                    />
                    <aside className="absolute left-0 top-0 h-full w-[min(22rem,86vw)] bg-[var(--bg-glass)] border-r border-[var(--border-subtle)] shadow-2xl flex flex-col">
                        <div className="px-4 pt-4">
                            <button
                                type="button"
                                onClick={() => setMobileNavOpen(false)}
                                className="ml-auto flex h-11 w-11 items-center justify-center rounded-xl tn-surface hover:tn-surface-strong tn-text-secondary hover:tn-text-primary transition-colors"
                                aria-label="Close menu"
                            >
                                <X className="w-4 h-4" />
                            </button>
                        </div>
                        {renderSidebarContent(true)}
                    </aside>
                </div>
            )}

            {/* Main Content */}
            <main className="relative z-10 flex-1 min-w-0 min-h-0 overflow-y-auto custom-scrollbar">
                <div className="lg:hidden sticky top-0 z-30 bg-[var(--bg-glass)]/95 backdrop-blur border-b border-[var(--border-subtle)] px-4 py-3 flex items-center justify-between">
                    <button
                        type="button"
                        onClick={() => setMobileNavOpen(true)}
                        className="inline-flex items-center gap-2 px-3 py-2 rounded-xl tn-surface hover:tn-surface-strong tn-text-secondary hover:tn-text-primary transition-colors min-h-11"
                    >
                        <Menu className="w-4 h-4" />
                        <span className="text-[10px] font-black uppercase tracking-widest">{t("layout.section_main")}</span>
                    </button>
                    <span className="text-[11px] font-black uppercase tracking-widest tn-text-muted">{t("layout.title")}</span>
                </div>
                <div className="min-h-full p-4 sm:p-6 md:p-8 lg:p-12 animate-in fade-in duration-700">
                    {children}
                </div>
            </main>
        </div>
    );
}
