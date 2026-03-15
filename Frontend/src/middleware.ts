import createMiddleware from 'next-intl/middleware';
import { routing } from './i18n/routing';

export default createMiddleware(routing);

export const config = {
// Bỏ qua các đường dẫn không cần xử lý i18n (như api, static files, _next)
    matcher: ['/((?!api|_next|_vercel|.*\\..*).*)']
};
