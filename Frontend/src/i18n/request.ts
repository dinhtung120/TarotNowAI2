import { getRequestConfig } from 'next-intl/server';
import { routing } from './routing';

export default getRequestConfig(async ({ requestLocale }) => {
    // Đợi requestLocale (Next.js 15 recommendation)
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    let locale = await (requestLocale as any);

    // Validate xem locale nhận được có hợp lệ không
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    if (!locale || !routing.locales.includes(locale as any)) {
        locale = routing.defaultLocale;
    }

    return {
        locale,
        // Tải JSON messages tương ứng với ngôn ngữ
        messages: (await import(`../../messages/${locale}.json`)).default
    };
});
