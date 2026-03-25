import { Link } from '@/i18n/routing';

export default function LocaleNotFound() {
 return (
  <div className="mx-auto flex min-h-[60vh] max-w-2xl flex-col items-center justify-center gap-6 px-6 text-center">
   <h1 className="text-4xl font-black tracking-tight tn-text-primary">404</h1>
   <p className="tn-text-secondary">This page does not exist or may have been moved.</p>
   <Link
    href="/"
    className="rounded-xl px-4 py-2 text-sm font-semibold tn-btn tn-btn-primary"
   >
    Back to home
   </Link>
  </div>
 );
}
