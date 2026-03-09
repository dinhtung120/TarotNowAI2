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
