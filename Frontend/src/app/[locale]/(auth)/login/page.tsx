import { LoginPage } from '@/features/auth/public';
import { redirectAuthenticatedAuthEntry } from '@/shared/server/auth/redirectAuthenticatedAuthEntry';

export default async function LoginRoutePage({
 params,
}: {
 params: Promise<{ locale: string }>;
}) {
 const { locale } = await params;
 await redirectAuthenticatedAuthEntry({ locale });

 return <LoginPage />;
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
