/*
 * ===================================================================
 * COMPONENT/FILE: Khai báo i18n Routing (routing.ts)
 * BỐI CẢNH (CONTEXT):
 *   Định nghĩa hằng số ngôn ngữ và các Hook Điều hướng (Navigation) tương thích với i18n.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Khai báo các ngôn ngữ hỗ trợ: `vi` (Việt), `en` (Anh), `zh` (Trung).
 *   - Sinh ra các hàm `Link`, `redirect`, `usePathname`, `useRouter` đã được bọc lại 
 *     để tự động chèn thêm `locale` hiện tại vào URL (Ví dụ: Chuyển hướng 
 *     đến `/home` sẽ tự biến thành `/vi/home`).
 * ===================================================================
 */
import { defineRouting } from 'next-intl/routing';
import { createNavigation } from 'next-intl/navigation';

export const routing = defineRouting({
 // Danh sách các ngôn ngữ hỗ trợ (vi, en, zh)
 locales: ['vi', 'en', 'zh'],

 // Fallback mặc định là vi
 defaultLocale: 'vi'
});

// APIs bọc lại các function của Next.js (để tự động chèn locale vào URL)
export const { Link, redirect, usePathname, useRouter } =
 createNavigation(routing);
