import { defineRouting } from 'next-intl/routing';
import { createNavigation } from 'next-intl/navigation';

export const routing = defineRouting({
 locales: ['vi', 'en', 'zh'],

 defaultLocale: 'vi',
 localePrefix: 'always'
});

export const { Link, usePathname, useRouter } =
 createNavigation(routing);
