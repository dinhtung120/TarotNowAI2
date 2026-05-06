import type { ReactNode } from 'react';
import { NextIntlClientProvider } from 'next-intl';
import { getMessages } from 'next-intl/server';
import { AUTH_CLIENT_NAMESPACES, pickClientMessages } from '@/i18n/clientMessages';

interface AuthLayoutProps {
 children: ReactNode;
}

export default async function AuthLayout({ children }: AuthLayoutProps) {
 const messages = await getMessages();
 const authMessages = pickClientMessages(messages, AUTH_CLIENT_NAMESPACES);

 return <NextIntlClientProvider messages={authMessages}>{children}</NextIntlClientProvider>;
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
