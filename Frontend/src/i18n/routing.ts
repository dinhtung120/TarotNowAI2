
import { defineRouting } from 'next-intl/routing';
import { createNavigation } from 'next-intl/navigation';

export const routing = defineRouting({
 locales: ['vi', 'en', 'zh'],

 defaultLocale: 'vi'
});

export const { Link, usePathname, useRouter } =
 createNavigation(routing);
