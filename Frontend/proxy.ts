export { default, default as proxy } from './src/proxy';

export const config = {
 matcher: ['/((?!api|_next|_vercel|favicon.ico|.*\\..*).*)'],
};
