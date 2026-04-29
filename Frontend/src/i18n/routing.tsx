import { forwardRef, type ComponentPropsWithoutRef } from 'react';
import { defineRouting } from 'next-intl/routing';
import { createNavigation } from 'next-intl/navigation';

export const routing = defineRouting({
 locales: ['vi', 'en', 'zh'],

 defaultLocale: 'vi',
 localePrefix: 'always',
});

const { Link: IntlLink, usePathname, useRouter, redirect, permanentRedirect, getPathname } =
 createNavigation(routing);

type IntlLinkProps = ComponentPropsWithoutRef<typeof IntlLink>;

export const Link = forwardRef<HTMLAnchorElement, IntlLinkProps>(function LinkWithDefaultPrefetch(
 props,
 ref
) {
 const { prefetch, ...rest } = props;
 return <IntlLink ref={ref} prefetch={prefetch ?? false} {...rest} />;
});

export { usePathname, useRouter, redirect, permanentRedirect, getPathname };
