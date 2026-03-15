import { ReactNode } from "react";
import Link from "next/link";

/**
 * Layout cho khu vực Admin.
 *
 * Lưu ý kỹ thuật (Next.js 16):
 * - Trong Next.js 16, `params` truyền vào Layout là một Promise,
 *   bắt buộc phải `await` trước khi sử dụng.
 * - Phiên bản cũ dùng destructuring đồng bộ `{ params: { locale } }`
 *   → gây lỗi TypeScript "Property 'locale' is missing in type 'Promise<...>'".
 * - Đã sửa: chuyển layout thành async function và await params.
 */
export default async function AdminLayout({ children, params }: { children: ReactNode, params: Promise<{ locale: string }> }) {
    /* Await params vì Next.js 16 truyền params dưới dạng Promise */
    const { locale } = await params;

    return (
        <div className="flex h-screen bg-[#0F1219] text-gray-300">
            {/* Sidebar — Thanh điều hướng bên trái cho Admin Portal */}
            <aside className="w-64 bg-[#1A1F2B] border-r border-[#2D3748] flex flex-col">
                <div className="p-6 border-b border-[#2D3748]">
                    <h2 className="text-2xl font-extrabold text-[#DFF2CB] drop-shadow-md">Admin Portal</h2>
                </div>
                <nav className="flex-1 p-4 space-y-2">
                    <Link href={`/${locale}/admin/users`} className="block px-4 py-3 rounded-lg hover:bg-[#2D3748] transition-colors font-medium">
                        Quản Lý Người Dùng
                    </Link>
                    <Link href={`/${locale}/admin/deposits`} className="block px-4 py-3 rounded-lg hover:bg-[#2D3748] transition-colors font-medium">
                        Quản Lý Giao Dịch
                    </Link>
                    <Link href={`/${locale}/admin/promotions`} className="block px-4 py-3 rounded-lg hover:bg-[#2D3748] transition-colors font-medium">
                        Khuyến Mãi Nạp
                    </Link>
                </nav>
            </aside>

            {/* Main Content — Khu vực hiển thị nội dung chính */}
            <main className="flex-1 overflow-y-auto p-8">
                {children}
            </main>
        </div>
    );
}

